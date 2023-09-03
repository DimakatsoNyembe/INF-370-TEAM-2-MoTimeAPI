//using MoTimeAPI.Models;

namespace MoTimeAPI.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public int UserRoleId { get; set; }
        public int PasswordId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Token { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiry { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string FileName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

    }
}
