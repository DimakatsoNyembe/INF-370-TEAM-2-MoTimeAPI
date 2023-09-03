using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoTimeAPI.Models;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using MoTimeAPI.UtilityService;
using MoTimeAPI.Helpers;
using System.Security.Cryptography;
using MoTimeAPI.Helpers.Dtos;
using MoTimeAPI.ViewModels;
using MoTimeAPI.Repos;
using System.Reflection;
using MoTime.ViewModel;
using System.Net.Mail;
using System.Net;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;

namespace MoTimeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly MoTimeDatabaseContext _dbContext;

        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly Dictionary<int, string> FileTypeMappings = new Dictionary<int, string>
        {
             //Add mappings for different HelpTypes
            { 1, "Image/jpeg" }, // For PDFs
            { 2, "Image/JPG" },       // For videos (assuming mp4 format)
            { 3, "Image/png" },       // For photos (assuming png format)
             //Add more mappings as needed for different file types and HelpTypes
        };
        public AuthenticationController(IRepository repository, IConfiguration configuration, MoTimeDatabaseContext dbContext, IConfiguration config, IEmailService emailService, IWebHostEnvironment webHostEnvironment)
        {

            _configuration = config;
            _repository = repository;
            _dbContext = dbContext;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login([FromBody] UserViewModel uvm)
        {
            try
            {
                var user = await _dbContext.Users
                    .Include(u => u.UserRole)
                    .Include(p => p.Password)
                    .FirstOrDefaultAsync(x => x.EmailAddress == uvm.EmailAddress);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                if (user.Password == null || string.IsNullOrEmpty(user.Password.Password1))
                {
                    return BadRequest(new { Message = "Password is not set for this user" });
                }

                var passwordMatches = PasswordHash.VerifyPassword(uvm.Password, user.Password.Password1);

                if (!passwordMatches)
                {
                    return BadRequest(new { Message = "Password is incorrect. Please enter a correct password!" });
                }

                var role = user.UserRole;

                user.Token = GenerateJWTToken(user, role);

                return Ok(new
                {
                    Token = user.Token,
                    Message = "Login Success!"
                });
            }
            catch (Exception ex)
            {
                // Log exception details for debugging
                Console.WriteLine($"An error occurred: {ex}");
                return StatusCode(500, new { Message = "Internal Server Error" });
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] UserViewModel uvm)
        {

            try
            {
                if (uvm == null)
                {
                    return BadRequest(new
                    {
                        Message = "The user does not exist!"
                    });
                }
                //Check if the email address already exists
                if (await CheckUserNameExistAsync(uvm.EmailAddress))
                {
                    return BadRequest(new
                    {
                        Message = "The email address you've entered already exists"
                    });
                }
                //Check Password strength
                var pass = CheckPasswordStrength(uvm.Password);
                if (!string.IsNullOrEmpty(pass))
                {
                    return BadRequest(new
                    {
                        Message = pass
                    });
                }
                if (uvm.ProfilePicture == null || uvm.ProfilePicture.Length == 0)
                {
                    return BadRequest(new
                    {
                        Message = "Please upload a profile picture"
                    });
                }
                byte[] fileBytes;
                string fileName;

                using (var memoryStream = new MemoryStream())
                {
                    await uvm.ProfilePicture.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                    fileName = uvm.ProfilePicture.FileName;
                }

                var role = await _repository.GetAllUserRoleAsync();
                var roles = role.FirstOrDefault(r => r.UserRoleId == uvm.UserRoleId);
                if (roles == null)
                {
                    return BadRequest(new
                    {
                        Message = "Please select a role for the user!"
                    });
                }

                var title = await _repository.GetAllTitlesAsync();
                var titles = title.FirstOrDefault(r => r.TitleId == uvm.TitleId);
                if (titles == null)
                {
                    return BadRequest(new
                    {
                        Message = "Please select a title"
                    });
                }
                var registrationToken = Guid.NewGuid().ToString();
                var registrationTokenExpiration = DateTime.UtcNow.AddHours(24);

                var hashedPassword = PasswordHash.HashPassword(uvm.Password);
                var newPassword = new Password
                {
                    Password1 = hashedPassword
                };
                await _dbContext.Passwords.AddAsync(newPassword);
                await _dbContext.SaveChangesAsync();
                //Convert UserViewModel to User entity
                var newUser = new User
                {
                    TitleId = uvm.TitleId,
                    FirstName = uvm.FirstName,
                    LastName = uvm.LastName,
                    EmailAddress = uvm.EmailAddress,
                    UserRoleId = uvm.UserRoleId,
                    ProfilePicture = fileBytes,
                    Token = registrationToken, // Store the registration token
                    TokenExpiration = registrationTokenExpiration,
                    FileName = fileName,
                    PasswordId = newPassword.PasswordId, // Associate the user with the newly created password
                    IsActive = uvm.IsActive
                };
                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                //Send email notification
                var activationLink = $"http://localhost:4200/activate-account?token={registrationToken}";
                string subject = "MoTime Account Registration!";
                string message = $"Hello {uvm.FirstName},\n\nAn account has been successfully created for you on MoTime.\n\n" +
                    $"Please click the following link to activate your account:\n{activationLink}\n\n" +
                    $"Kind regards,\nMoTime";

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("dimakatsoxx99@gmail.com", "kjmdejsiejrronuh");
                    smtpClient.EnableSsl = true;

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress("dimakatsoxx99@gmail.com"),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(uvm.EmailAddress);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                return Ok(new
                {
                    Message = "User has been successfully registered. Email notification sent."
                });
            }
            catch (Exception)
            {
                //Handle exceptions and return appropriate response
                return BadRequest(new
                {
                    Message = "An error occurred while registering the user."
                });
            }

        }

        private Task<bool> CheckUserNameExistAsync(string emailAddress)
            => _dbContext.Users.AnyAsync(x => x.EmailAddress == emailAddress);

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append("Minimum characters for password should be 8" + Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[0-9]")))
                sb.Append("Password should be alphanumeric" + Environment.NewLine);
            if (!Regex.IsMatch(password, "[<,>,?,{, },[,@, #, $, %, ^, &, *, (, ), +, _,',:,//,||,\\ ]"))
                sb.Append("Password should contain special characters" + Environment.NewLine);
            return sb.ToString();
        }
        private string GenerateJWTToken(User user, UserRole role)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("y+VRv[&)0XhxJ<sk=yUpW{yE5CH@xh");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.UserRole.UserRoleName),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("userId", user.UserId.ToString())
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync(); // Assuming _dbContext is your DbContext instance
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id); // Assuming _dbContext is your DbContext instance

                if (user == null)
                    return NotFound("The user does not exist");

                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "There was an error retrieving the user. Please try again");
            }
        }
        [HttpPut]
        [Route("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] UserViewModel uuv)
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        Message = "User not found."
                    });
                }

                var userRole = await _repository.GetAllUserRoleAsync();
                var userRoles = userRole.FirstOrDefault(r => r.UserRoleId == uuv.UserRoleId);
                if (userRoles == null)
                {
                    return BadRequest(new
                    {
                        Messsage = "Invalid role. Please try again"
                    });
                }
                var title = await _repository.GetAllTitlesAsync();
                var titles = title.FirstOrDefault(r => r.TitleId == uuv.TitleId);
                if (titles == null)
                {
                    return BadRequest(new
                    {
                        Messsage = "Invalid title. Please try again"
                    });
                }
                if (uuv.ProfilePicture != null && uuv.ProfilePicture.Length > 0)
                {
                    byte[] fileBytes;
                    string fileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        await uuv.ProfilePicture.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                        fileName = uuv.ProfilePicture.FileName;
                    }

                    existingUser.ProfilePicture = fileBytes;
                    existingUser.FileName = fileName;
                }
                //Update the user's properties
                existingUser.TitleId = uuv.TitleId;
                existingUser.FirstName = uuv.FirstName;
                existingUser.LastName = uuv.LastName;
                existingUser.UserRoleId = uuv.UserRoleId;


                //Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "User details updated successfully."
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "An error occurred while updating user details."
                });
            }
        }
        [HttpPut]
        [Route("UpdatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] PasswordUpdateViewModel puv)
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(id);

                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        Message = "User not found."
                    });
                }

                var existingPassword = await _dbContext.Passwords.FindAsync(existingUser.PasswordId);
                if (existingPassword == null)
                {
                    return BadRequest(new
                    {
                        Message = "Password data not found."
                    });
                }

                // Hash the entered old password for comparison
                //var hashedOldPassword = PasswordHash.HashPassword(puv.OldPassword);
                //var hashedStoredPassword = PasswordHash.HashPassword(existingPassword.Password1);

                //// Verify the hashed old password matches the stored hashed password
                //if (hashedOldPassword != hashedStoredPassword)
                //{
                //    return BadRequest(new
                //    {
                //        Message = "Old password is incorrect."
                //    });
                //}

                // Check new password strength
                var pass = CheckPasswordStrength(puv.NewPassword);
                if (!string.IsNullOrEmpty(pass))
                {
                    return BadRequest(new
                    {
                        Message = pass
                    });
                }

                // Update the stored hashed password
                existingPassword.Password1 = PasswordHash.HashPassword(puv.NewPassword);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Password updated successfully."
                });
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("DbUpdateException: " + dbEx);
                return BadRequest(new
                {
                    Message = "A database error occurred while updating the password."
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                return BadRequest(new
                {
                    Message = "An error occurred while updating the password."
                });
            }
        }
        [HttpGet("profile-picture/{userId}")]
        public async Task<IActionResult> GetProfilePicture(int userId)
        {
            try
            {
                // Retrieve the user's profile picture from the database based on the user ID
                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null || user.ProfilePicture == null || user.ProfilePicture.Length == 0)
                {
                    return NotFound("Profile picture not found.");
                }

                // Return the profile picture as a response
                return File(user.ProfilePicture, "image/jpeg"); // Adjust the content type as needed
            }
            catch (Exception)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("ViewAccount/{userId}")]
        public async Task<IActionResult> ViewAccount(int userId)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound(new
                    {
                        Message = "User not found."
                    });
                }

                //You may want to exclude sensitive information like password hashes from the response

                var userResponse = new
                {
                    user.UserId,
                    user.TitleId,
                    user.FirstName,
                    user.LastName,
                    user.EmailAddress,
                    user.UserRoleId,
                    user.FileName,
                    //Exclude PasswordId and other sensitive fields here
                };

                return Ok(userResponse);
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "An error occurred while fetching user account details."
                });
            }
        }
        [HttpPut]
        [Route("DeactivateAccount/{userId}")]
        public async Task<IActionResult> DeactivateAccount(int userId) // Change the parameter name from "id" to "userId"
        {
            try
            {
                var existingUser = await _dbContext.Users.FindAsync(userId);

                if (existingUser == null)
                {
                    return NotFound(new
                    {
                        Message = "User not found."
                    });
                }

                //You might want to add a property like IsActive to your User entity
                existingUser.IsActive = false;

                await _dbContext.SaveChangesAsync();

                return Ok(new
                {
                    Message = "User account deactivated."
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Message = "An error occurred while deactivating the user account."
                });
            }
        }

        [HttpPost("send-reset-email")]
        public async Task<IActionResult> SendEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.EmailAddress == email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "Email address does not exist!"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken = emailToken;
            user.ResetPasswordExpiry = DateTime.UtcNow.AddMinutes(15);
            string from = _configuration["EmailSettings:From"];
            var emailModel = new EmailModel(email, "Reset Password!", EmailBody.EmailStringBody(email, emailToken));
            _emailService.SendEmail(emailModel);
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Email Sent"

            });
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
            var user = await _dbContext.Users.Include(p => p.Password).AsNoTracking().FirstOrDefaultAsync(a => a.EmailAddress == resetPasswordDto.Email);
            if (user == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = "User not found"


                });
            }
            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = (DateTime)user.ResetPasswordExpiry;
            if (tokenCode != resetPasswordDto.EmailToken)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Invalid reset link"
                });
            }
            user.Password.Password1 = PasswordHash.HashPassword(resetPasswordDto.NewPassword);
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(new
            {
                StatusCode = 200,
                Message = "Password successfully reset"
            });
        }
        //******************************************************************************************************************************
        //************************************ Roles***********************************************************************************
        [HttpGet]
        [Route("GetAllUserRoles")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            try
            {
                var results = await _repository.GetAllUserRoleAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetUserRole/{id}")]
        public async Task<IActionResult> GetUserRole(int id)
        {
            try
            {
                var result = await _repository.GetUserRoleByIdAsync(id);
                if (result == null) return NotFound(new
                {
                    Message = "The user role does not exist"
                });
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("AddUserRole")]
        public async Task<IActionResult> AddUserRole(RoleViewModel rvm)
        {
            var userRole = new UserRole { UserRoleName = rvm.UserRoleName, UserRoleDescription = rvm.UserRoleDescription };
            try
            {
                _repository.Add(userRole);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(userRole);
        }

        [HttpPut]
        [Route("EditUserRole/{id}")]
        public async Task<ActionResult<RoleViewModel>> EditUserRole(int id, RoleViewModel rvm)
        {
            try
            {
                var existingUserRole = await _repository.GetUserRoleByIdAsync(id);
                if (existingUserRole == null) return NotFound($"The user role does not exist");
                existingUserRole.UserRoleName = rvm.UserRoleName;
                existingUserRole.UserRoleDescription = rvm.UserRoleDescription;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingUserRole);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteUserRole/{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            try
            {
                var existingUserRole = await _repository.GetUserRoleByIdAsync(id);
                if (existingUserRole == null) return NotFound($"The user role does not exist");
                _repository.Delete(existingUserRole);
                if (await _repository.SaveChangesAsync())
                    return Ok(existingUserRole);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpGet]
        [Route("GetAllTitles")]
        public async Task<IActionResult> GetAllTitles()
        {
            try
            {
                var results = await _repository.GetAllTitlesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        [Route("GetAllPasswords")]
        public async Task<IActionResult> GetAllPasswords()
        {
            try
            {
                var results = await _repository.GetAllPasswordsAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        // GET: api/employees/byUserId/{userId}
        [HttpGet("byUserId/{userId}")]
        public ActionResult<Employee> GetEmployeeByUserId(int userId)
        {
            var employee = _dbContext.Employees.SingleOrDefault(e => e.User.UserId == userId);
            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        private string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshtoken = Convert.ToBase64String(tokenBytes);

            var tokenIsInUser = _dbContext.Users
            .Any(_ => _.RefreshToken == refreshtoken);

            if (tokenIsInUser)
            {
                return CreateRefreshToken();
            }
            return refreshtoken;
        }


    }
}

