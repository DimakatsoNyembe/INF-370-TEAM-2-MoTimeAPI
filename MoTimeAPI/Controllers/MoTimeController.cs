using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoTime.ViewModel;
using MoTimeAPI.Models;
using MoTimeAPI.Repos;
using MoTimeAPI.ViewModels;
using System.Net.Http.Headers;
using System.Dynamic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using Humanizer;
using System.Numerics;


namespace MoTimeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoTimeController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly MoTimeDatabaseContext _dbContext;

        private readonly Dictionary<int, string> FileTypeMappings = new Dictionary<int, string>
        {
            // Add mappings for different HelpTypes
            { 1, "application/pdf" }, // For PDFs
            { 2, "Video/mp4" },       // For videos (assuming mp4 format)
            { 3, "Image/png" },       // For photos (assuming png format)
            // Add more mappings as needed for different file types and HelpTypes
        };

        public MoTimeController(IRepository repository, MoTimeDatabaseContext dbContext)
        {
            _repository = repository;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        [Route("GetAllClaimItems")]
        public async Task<IActionResult> GetAllClaimItems()
        {
            try
            {
                var results = await _repository.GetAllClaimItemAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        // GET: api/MoTime/GetClaimItem/5
        [HttpGet]
        [Route("GetClaimItem/{id}")]
        public async Task<IActionResult> GetClaimItem(int id)
        {
            try
            {
                var result = await _repository.GetClaimItemAsync(id);

                if (result == null) return NotFound("Claim item does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        // POST: api/MoTime/AddClaimItem
        [HttpPost]
        [Route("AddClaimItem")]
        public async Task<IActionResult> AddClaimItem(ClaimItemViewModel claimItemVM)
        {
            var claimItem = new ClaimItem
            {
                ClaimTypeId = claimItemVM.ClaimTypeId,
                ClaimItemName = claimItemVM.ClaimItemName
            };

            try
            {
                _repository.Add(claimItem);
                await _repository.SaveChangesAsync();

                return Ok(claimItem);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        // PUT: api/MoTime/UpdateClaimItem/5
        [HttpPut]
        [Route("UpdateClaimItem/{id}")]
        public async Task<IActionResult> UpdateClaimItem(int id, ClaimItemViewModel claimItemVM)
        {
            var claimItem = await _repository.GetClaimItemAsync(id);

            if (claimItem == null)
            {
                return NotFound("Claim item does not exist");
            }

            claimItem.ClaimTypeId = claimItemVM.ClaimTypeId;
            claimItem.ClaimItemName = claimItemVM.ClaimItemName;

            try
            {
                await _repository.SaveChangesAsync();

                return Ok(claimItem);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        // DELETE: api/MoTime/DeleteClaimItem/5
        [HttpDelete]
        [Route("DeleteClaimItem/{id}")]
        public async Task<IActionResult> DeleteClaimItem(int id)
        {
            var claimItem = await _repository.GetClaimItemAsync(id);

            if (claimItem == null)
            {
                return NotFound("Claim item does not exist");
            }

            try
            {
                _repository.Delete(claimItem);
                await _repository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }


        // project allocation
        [HttpGet]
        [Route("GetAllProjectAllocations")]
        public async Task<IActionResult> GetAllProjectAllocations()
        {
            try
            {
                var results = await _repository.GetAllProjectAllocationAsync();

                dynamic allocations = results.Select(p => new
                {
                    p.ProjectAllocationId,
                    p.EmployeeId,
                    p.ProjectId,
                    p.IsEligibleToClaim,
                    p.ClaimableAmount,
                    p.TotalNumHours,
                    p.BillableOverTime,
                    p.IsOperationalManager,
                    p.IsProjectManager,
                    pName = p.Project.ProjectName,
                    claim = p.ClaimItem.ClaimItemName,
                    firstName = p.Employee.User.FirstName,
                    lastName = p.Employee.User.LastName
                });

                return Ok(allocations);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetProjectAllocation/{Id}")]
        public async Task<IActionResult> GetProjectAllocationAsync(int Id)
        {
            try
            {
                var result = await _repository.GetProjectAllocationAsync(Id);

                if (result == null)
                    return NotFound("Project allocation does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddProjectAllocation")]
        public async Task<IActionResult> AddProjectAllocation(ProjectAllocationViewModel avm)
        {

            var allocation = new ProjectAllocation
            {
                ProjectId = avm.ProjectId,
                EmployeeId = avm.EmployeeId,
                IsEligibleToClaim = avm.IsEligibleToClaim,
                ClaimItemId = avm.ClaimItemId,
                ClaimableAmount = avm.ClaimableAmount,
                TotalNumHours = avm.TotalNumHours,
                BillableOverTime = avm.BillableOverTime,
                IsOperationalManager = avm.IsOperationalManager,
                IsProjectManager = avm.IsProjectManager
            };

            try
            {
                _repository.Add(allocation);
                await _repository.SaveChangesAsync();
            }

            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }

            return Ok(allocation);
        }



        [HttpDelete]
        [Route("DeleteProjectAllocation/{Id}")]
        public async Task<IActionResult> DeleteProjectAllocation(int Id)
        {
            try
            {
                var existing = await _repository.GetProjectAllocationAsync(Id);
                if (existing == null) return NotFound($"The Project Allocation does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }



        // client
        [HttpGet]
        [Route("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var results = await _repository.GetAllClientAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetClient/{Id}")]
        public async Task<IActionResult> GetClientAsync(int Id)
        {
            try
            {
                var result = await _repository.GetClientAsync(Id);

                if (result == null) return NotFound("Client does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }


        [HttpPost]
        [Route("AddClient")]
        public async Task<IActionResult> AddClient(ClientViewModel cvm)
        {
            var client = new Client
            {
                //AdminId = cvm.AdminId,
                Account = cvm.Account,
                AccountManager = cvm.AccountManager,
                Department = cvm.Department,
                SiteCode = cvm.SiteCode,
                ProjectCode = cvm.ProjectCode
            };
            try
            {
                _repository.Add(client);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(client);
        }

        [HttpPut]
        [Route("EditClient/{Id}")]
        public async Task<ActionResult<ClientViewModel>> EditClient(int Id, ClientViewModel cvm)
        {
            try
            {
                var existing = await _repository.GetClientAsync(Id);
                if (existing == null) return NotFound($"The client does not exist");


                //existing.AdminId = cvm.AdminId;
                existing.Account = cvm.Account;
                existing.AccountManager = cvm.AccountManager;
                existing.SiteCode = cvm.SiteCode;
                existing.ProjectCode = cvm.ProjectCode;
                existing.Department = cvm.Department;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existing);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteClient/{Id}")]
        public async Task<IActionResult> DeleteClient(int Id)
        {
            try
            {
                var existing = await _repository.GetClientAsync(Id);
                if (existing == null) return NotFound($"The client does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }

        // project
        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var results = await _repository.GetAllProjectAsync();

                dynamic projects = results.Select(p => new
                {
                    p.ProjectId,
                    p.ProjectName,
                    p.StartDate,
                    p.EndDate,
                    p.AdminId,
                    pStatus = p.ProjectStatus.ProjectStatusName,
                    pClient = p.Client.Account


                });

                return Ok(projects);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetProject/{Id}")]
        public async Task<IActionResult> GetProjectAsync(int Id)
        {
            try
            {
                var result = await _repository.GetProjectAsync(Id);

                if (result == null)
                    return NotFound("Project does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddProject")]
        public async Task<IActionResult> AddProject(ProjectViewModel pvm)
        {

            var project = new Project
            {

                ClientId = pvm.ClientId,
                //AdminId = pvm.AdminId,
                ProjectStatusId = pvm.ProjectStatusId,
                ProjectName = pvm.ProjectName,
                StartDate = pvm.StartDate,
                EndDate = pvm.EndDate

            };

            try
            {
                _repository.Add(project);
                await _repository.SaveChangesAsync();
            }

            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }

            return Ok(project);
        }

        [HttpPut]
        [Route("EditProject/{Id}")]
        public async Task<ActionResult<ProjectViewModel>> EditProject(int Id, ProjectViewModel pvm)
        {
            try
            {
                var existing = await _repository.GetProjectAsync(Id);
                if (existing == null) return NotFound($"The project does not exist");

                existing.ClientId = pvm.ClientId;
                //existing.AdminId = pvm.AdminId;
                existing.ProjectStatusId = pvm.ProjectStatusId;
                existing.ProjectName = pvm.ProjectName;
                existing.StartDate = pvm.StartDate;
                existing.EndDate = pvm.EndDate;


                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existing);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteProject/{Id}")]
        public async Task<IActionResult> DeleteProject(int Id)
        {
            try
            {
                var existing = await _repository.GetProjectAsync(Id);
                if (existing == null) return NotFound($"The Project does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }
        // resource types
        [HttpGet]
        [Route("GetAllResourceTypes")]
        public async Task<IActionResult> GetAllResourceTypes()
        {
            try
            {
                var results = await _repository.GetAllResourceTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetResourceType/{Id}")]
        public async Task<IActionResult> GetResourceTypeAsync(int Id)
        {
            try
            {
                var result = await _repository.GetResourceTypeAsync(Id);

                if (result == null) return NotFound("Resource type does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }


        [HttpPost]
        [Route("AddResourceType")]
        public async Task<IActionResult> AddResourceType(ResourceTypeViewModel ervm)
        {
            var type = new ResourceType
            {
                ResourceTypeName = ervm.ResourceTypeName,
                ResourceTypeDescription = ervm.ResourceTypeDescription
            };
            try
            {
                _repository.Add(type);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(type);
        }

        [HttpPut]
        [Route("EditResourceType/{Id}")]
        public async Task<ActionResult<ResourceTypeViewModel>> EditResourceType(int Id, ResourceTypeViewModel ervm)
        {
            try
            {
                var existing = await _repository.GetResourceTypeAsync(Id);
                if (existing == null) return NotFound($"The resource type does not exist");


                existing.ResourceTypeName = ervm.ResourceTypeName;
                existing.ResourceTypeDescription = ervm.ResourceTypeDescription;
                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existing);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteResourceType/{Id}")]
        public async Task<IActionResult> DeleteResourceType(int Id)
        {
            try
            {
                var existing = await _repository.GetResourceTypeAsync(Id);
                if (existing == null) return NotFound($"The resource type does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }


        // resource
        [HttpGet]
        [Route("GetAllResources")]
        public async Task<IActionResult> GetAllResources()
        {
            try
            {
                var results = await _repository.GetAllResourceAsync();

                dynamic resources = results.Select(p => new
                {
                    p.ResourceId,
                    p.ResourceName,
                    p.ResourceDescription,
                    pType = p.ResourceType.ResourceTypeName

                });

                return Ok(resources);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetResource/{Id}")]
        public async Task<IActionResult> GetResourceAsync(int Id)
        {
            try
            {
                var result = await _repository.GetResourceAsync(Id);

                if (result == null)
                    return NotFound("Resource does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpPost]
        [Route("AddResource")]
        public async Task<IActionResult> AddResource(ResourceViewModel rvm)
        {

            var resource = new Resource
            {
                ResourceName = rvm.ResourceName,
                ResourceDescription = rvm.ResourceDescription,
                ResourceTypeId = rvm.ResourceTypeId

            };

            try
            {
                _repository.Add(resource);
                await _repository.SaveChangesAsync();
            }

            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }

            return Ok(resource);
        }

        [HttpPut]
        [Route("EditResource/{Id}")]
        public async Task<ActionResult<ResourceViewModel>> EditResource(int Id, ResourceViewModel rvm)
        {
            try
            {
                var existing = await _repository.GetResourceAsync(Id);
                if (existing == null) return NotFound($"The resource does not exist");

                existing.ResourceName = rvm.ResourceName;
                existing.ResourceDescription = rvm.ResourceDescription;
                existing.ResourceTypeId = rvm.ResourceTypeId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existing);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteResource/{Id}")]
        public async Task<IActionResult> DeleteResource(int Id)
        {
            try
            {
                var existing = await _repository.GetResourceAsync(Id);
                if (existing == null) return NotFound($"The resource does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }



        // project status

        [HttpGet]
        [Route("GetAllProjectStatus")]
        public async Task<IActionResult> GetAllProjectStatus()
        {
            try
            {
                var results = await _repository.GetAllProjectStatusAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetProjectStatus/{Id}")]
        public async Task<IActionResult> GetProjectStatusAsync(int Id)
        {
            try
            {
                var result = await _repository.GetProjectStatusAsync(Id);

                if (result == null) return NotFound("Project status does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }





        // EmployeeResource

        [HttpGet]
        [Route("GetAllEmployeeResource")]
        public async Task<IActionResult> GetAllEmployeeResource()
        {
            try
            {
                var er = await _repository.GetAllEmployeeResourceAsync();
                var erVM = er.Select(r => new EmployeeResourceViewModel
                {
                    EmployeeId = Convert.ToInt32(r.EmployeeId),
                    ResourceId = Convert.ToInt32(r.ResourceId),
                    ResourceLevelId = Convert.ToInt32(r.ResourceLevelId)
                }).ToList();
                return Ok(erVM);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
            }
        }

        [HttpGet]
        [Route("GetEmployeeResourceById/{id}")]
        public async Task<IActionResult> GetEmployeeResourceById(int id)
        {
            try
            {
                var er = await _repository.GetEmployeeResourceByIdAsync(id);

                if (er == null)
                {
                    return NotFound("This employee resource information cannot be found");
                }

                return Ok(er);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
            }
        }

        [HttpPost]
        [Route("AddEmployeeResource")]
        public async Task<IActionResult> AddEmployeeResource(EmployeeResourceViewModel evm)
        {
            var er = new EmployeeResource
            {
                EmployeeId = Convert.ToInt32(evm.EmployeeId),
                ResourceId = Convert.ToInt32(evm.ResourceId),
                ResourceLevelId = Convert.ToInt32(evm.ResourceLevelId)
            };
            try
            {
                _repository.Add(er);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(er);
        }


        //[HttpGet]
        //[Route("GetAllTimesheet")]
        //public async Task<IActionResult> GetAllTimesheet()
        //{
        //    try
        //    {
        //        var results = await _repository.GetAllTimesheetAsync();
        //        return Ok(results);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        //[HttpGet]
        //[Route("GetTimesheet/{id}")]
        //public async Task<IActionResult> GetTimesheet(int id)
        //{
        //    try
        //    {
        //        var result = await _repository.GetTimesheetByIdAsync(id);
        //        if (result == null) return NotFound("Timesheet does not exist");
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}





        // ProjectRequest

        //[HttpGet]
        //[Route("GetAllProjectRequest")]
        //public async Task<IActionResult> GetAllProjectRequest()
        //{
        //    try
        //    {
        //        var projectr = await _repository.GetAllProjectRequestAsync();
        //        var projectrVM = projectr.Select(p => new ProjectRequestViewModel
        //        {
        //            ProjectRequestDescription = p.ProjectRequestDescription,
        //            ProjectRequestDate = p.ProjectRequestDate,
        //            EmployeeId = Convert.ToInt32(p.EmployeeId),
        //            ProjectRequestStatusId = Convert.ToInt32(p.ProjectRequestStatusId)
        //        }).ToList();
        //        return Ok(projectrVM);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
        //    }
        //}

        //[HttpGet]
        //[Route("GetProjectRequestById/{id}")]
        //public async Task<IActionResult> GetProjectRequestById(int id)
        //{
        //    try
        //    {
        //        var projectr = await _repository.GetProjectRequestByIdAsync(id);

        //        if (projectr == null)
        //        {
        //            return NotFound("This project request information cannot be found");
        //        }

        //        return Ok(projectr);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
        //    }
        //}

        //[HttpPost]
        //[Route("AddProjectRequest")]
        //public async Task<IActionResult> AddProjectRequest(ProjectRequestViewModel pvm)
        //{
        //    var projectr = new ProjectRequest
        //    {
        //        ProjectRequestDescription = pvm.ProjectRequestDescription,
        //        ProjectRequestDate = pvm.ProjectRequestDate,
        //        EmployeeId = Convert.ToInt32(pvm.EmployeeId),
        //        ProjectRequestStatusId = Convert.ToInt32(pvm.ProjectRequestStatusId)
        //    };
        //    try
        //    {
        //        _repository.Add(projectr);
        //        await _repository.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest("Invalid transaction");
        //    }
        //    return Ok(projectr);
        //}

        // division

        [HttpGet]
        [Route("GetAllDivisions")]
        public async Task<IActionResult> GetAllDivisions()
        {
            try
            {
                var results = await _repository.GetAllDivisionAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetDivision/{Id}")]
        public async Task<IActionResult> GetDivisionAsync(int Id)
        {
            try
            {
                var result = await _repository.GetDivisionAsync(Id);

                if (result == null) return NotFound("Division does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }



        [HttpGet]
        [Route("GetAllManagerTypes")]
        public async Task<IActionResult> GetAllManagerTypes()
        {
            try
            {
                var results = await _repository.GetAllManagerTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetAllEmployeeStatuses")]
        public async Task<IActionResult> GetAllEmployeeStatuses()
        {
            try
            {
                var results = await _repository.GetAllEmployeeStatusAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetAllCalendars")]
        public async Task<IActionResult> GetAllCalendars()
        {
            try
            {
                var results = await _repository.GetAllCalendarAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpGet]
        [Route("GetCalendar/{Id}")]
        public async Task<IActionResult> GetCalendarAsync(int Id)
        {
            try
            {
                var result = await _repository.GetCalendarAsync(Id);

                if (result == null) return NotFound("Calendar does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }

        [HttpPost]
        [Route("AddCalendar")]
        public async Task<IActionResult> AddCalendar(CalendarViewModel cvm)
        {
            var citem = new Models.Calendar
            {

                CalendarItemName = cvm.CalendarItemName,
                Date = cvm.Date,
                Location = cvm.Location,
                CalendarItemDescription = cvm.CalendarItemDescription,
                StartTime = cvm.StartTime,
                EndTime = cvm.EndTime
            };
            try
            {
                _repository.Add(citem);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(citem);
        }


        [HttpPut]
        [Route("EditCalendar/{Id}")]
        public async Task<ActionResult<CalendarViewModel>> EditCalendar(int Id, CalendarViewModel cModel)
        {
            try
            {
                var existingCalendar = await _repository.GetCalendarAsync(Id);
                if (existingCalendar == null) return NotFound($"The calendar does not exist");


                existingCalendar.CalendarItemName = cModel.CalendarItemName;
                existingCalendar.Date = cModel.Date;
                existingCalendar.Location = cModel.Location;
                existingCalendar.CalendarItemDescription = cModel.CalendarItemDescription;
                existingCalendar.StartTime = cModel.StartTime;
                existingCalendar.EndTime = cModel.EndTime;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingCalendar);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteCalendar/{Id}")]
        public async Task<IActionResult> DeleteCalendar(int Id)
        {
            try
            {
                var existing = await _repository.GetCalendarAsync(Id);
                if (existing == null) return NotFound($"The calendar does not exist");

                _repository.Delete(existing);
                if (await _repository.SaveChangesAsync())
                    return Ok(existing);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }

        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<ActionResult> GetAllUsers()
        //{
        //    //return Ok(await _dbContext.Users.ToListAsync());

        //    try
        //    {
        //        var results = await _repository.GetAllUsersAsync();
        //        return Ok(results);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal Server Error. Please contact support.");
        //    }
        //}

        [HttpGet]
        [Route("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            try
            {
                var results = await _repository.GetAllEmployeeAsync();

                dynamic employees = results.Select(p => new
                {
                    p.EmployeeId,
                    p.UserId,
                    pFirstName = p.User.FirstName,
                    pLastName = p.User.LastName,
                    pResource = p.Resource.ResourceName,
                    pStatus = p.EmployeeStatus.EmployeeStatusName,
                    pType = p.EmployeeType.EmployeeTypeName,
                    pRegion = p.Region.RegionName,
                    pDivision = p.Division.DivisionName

                });

                return Ok(employees);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetEmployee/{Id}")]
        public async Task<IActionResult> GetEmployeeAsync(int Id)
        {
            try
            {
                var result = await _repository.GetEmployeeAsync(Id);

                if (result == null)
                    return NotFound("Employee does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }


        [HttpPost]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee(EmployeeViewModel employeeViewModel)
        {
            // Map the EmployeeViewModel to the Employee model
            var employee = new Employee
            {

                UserId = employeeViewModel.UserId,
                ResourceId = employeeViewModel.ResourceId,
                EmployeeTypeId = employeeViewModel.EmployeeTypeId,
                EmployeeStatusId = employeeViewModel.EmployeeStatusId,
                RegionId = employeeViewModel.RegionId,
                DivisionId = employeeViewModel.DivisionId,
                ManagerTypeId = employeeViewModel.ManagerTypeId
            };

            try
            {
                // Add the employee to the DbContext
                _repository.Add(employee);
                await _repository.SaveChangesAsync();
            }

            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }

            // Return a success response
            return Ok(employee);
        }

        [HttpPut]
        [Route("EditEmployee/{Id}")]
        public async Task<ActionResult<EmployeeViewModel>> EditEmployee(int Id, EmployeeViewModel eModel)
        {
            try
            {
                var existingEmployee = await _repository.GetEmployeeAsync(Id);
                if (existingEmployee == null) return NotFound($"The employee does not exist");

                existingEmployee.UserId = eModel.UserId;
                existingEmployee.ResourceId = eModel.ResourceId;
                existingEmployee.EmployeeTypeId = eModel.EmployeeTypeId;
                existingEmployee.ManagerTypeId = eModel.ManagerTypeId;
                existingEmployee.EmployeeStatusId = eModel.EmployeeStatusId;
                existingEmployee.DivisionId = eModel.DivisionId;
                existingEmployee.RegionId = eModel.RegionId;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingEmployee);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteEmployee/{Id}")]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            try
            {
                var existingEmployee = await _repository.GetEmployeeAsync(Id);
                if (existingEmployee == null) return NotFound($"The employee does not exist");

                _repository.Delete(existingEmployee);
                if (await _repository.SaveChangesAsync())
                    return Ok(existingEmployee);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }


        [HttpGet]
        [Route("GetAllEmployeeTypes")]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            try
            {
                var results = await _repository.GetAllEmployeeTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetEmployeeType/{etypeId}")]
        public async Task<IActionResult> GetEmployeeTypeAsync(int etypeId)
        {
            try
            {
                var result = await _repository.GetEmployeeTypeAsync(etypeId);

                if (result == null) return NotFound("Employee type does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }


        [HttpPost]
        [Route("AddEmployeeType")]
        public async Task<IActionResult> AddEmployeeType(EmployeeTypeViewModel etvm)
        {
            var etype = new EmployeeType
            {
                EmployeeTypeName = etvm.EmployeeTypeName,
                EmployeeTypeDescription = etvm.EmployeeTypeDescription
            };
            try
            {
                _repository.Add(etype);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(etype);
        }

        [HttpPut]
        [Route("EditEmployeeType/{etypeId}")]
        public async Task<ActionResult<EmployeeTypeViewModel>> EditEmployeeType(int etypeId, EmployeeTypeViewModel typeModel)
        {
            try
            {
                var existingType = await _repository.GetEmployeeTypeAsync(etypeId);
                if (existingType == null) return NotFound($"The employee type does not exist");


                existingType.EmployeeTypeName = typeModel.EmployeeTypeName;
                existingType.EmployeeTypeDescription = typeModel.EmployeeTypeDescription;
                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingType);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpDelete]
        [Route("DeleteEmployeeType/{etypeId}")]
        public async Task<IActionResult> DeleteEmployeeType(int etypeId)
        {
            try
            {
                var existingType = await _repository.GetEmployeeTypeAsync(etypeId);
                if (existingType == null) return NotFound($"The employee type does not exist");

                _repository.Delete(existingType);
                if (await _repository.SaveChangesAsync())
                    return Ok(existingType);
            }

            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }

            return BadRequest("Your request is invalid.");
        }


        [HttpGet]
        [Route("GetAllRegions")]
        public async Task<IActionResult> GetAllRegions()
        {
            try
            {
                var results = await _repository.GetAllRegionAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetRegion/{Id}")]
        public async Task<IActionResult> GetRegionAsync(int Id)
        {
            try
            {
                var result = await _repository.GetRegionAsync(Id);

                if (result == null) return NotFound("Region does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support");
            }
        }


        [HttpGet]
        [Route("GetAllHelpType")]
        public async Task<IActionResult> GetAllHelpType()
        {
            try
            {
                var results = await _repository.GettAllHelpTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetHelpType/{id}")]
        public async Task<IActionResult> GetHelpType(int id)
        {
            try
            {
                var result = await _repository.GetHelpTypeAsync(id);
                if (result == null) return NotFound("Help type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("AddHelpType")]
        public async Task<IActionResult> AddHelpType(HelpTypeViewModel hvm)
        {
            var helpType = new HelpType { HelpTypeName = hvm.HelpTypeName, HelpTypeDescription = hvm.HelpTypeDescription };
            try
            {
                _repository.Add(helpType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(helpType);
        }

        [HttpPut]
        [Route("EditHelpType/{id}")]
        public async Task<ActionResult<HelpTypeViewModel>> EditHelpType(int id, HelpTypeViewModel hvm)
        {
            try
            {
                var existingHelpType = await _repository.GetHelpTypeAsync(id);
                if (existingHelpType == null) return NotFound($"The help type does not exist");
                existingHelpType.HelpTypeName = hvm.HelpTypeName;
                existingHelpType.HelpTypeDescription = hvm.HelpTypeDescription;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingHelpType);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteHelpType/{id}")]
        public async Task<IActionResult> DeleteHelpType(int id)
        {
            try
            {
                var existingHelpType = await _repository.GetHelpTypeAsync(id);
                if (existingHelpType == null) return NotFound($"The help type does not exist");
                _repository.Delete(existingHelpType);
                if (await _repository.SaveChangesAsync())
                    return Ok(existingHelpType);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }

        // GET: api/motime/timesheetstatus
        [HttpGet("timesheetstatus")]
        public async Task<IActionResult> GetAllTimesheetStatus()
        {
            try
            {
                var timesheetStatuses = await _repository.GettAllTimeSheetStatusAsync();
                return Ok(timesheetStatuses);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
            }
        }

        // GET: api/motime/timesheetstatus/{id}
        [HttpGet("timesheetstatus/{id}")]
        public async Task<IActionResult> GetTimesheetStatus(int id)
        {
            try
            {
                var timesheetStatus = await _repository.GetTimeSheetStatusAsync(id);

                if (timesheetStatus == null)
                {
                    return NotFound("Timesheet status not found");
                }

                return Ok(timesheetStatus);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error, please contact support");
            }
        }
        //Leave
        [HttpGet]
        [Route("GetAllLeaveType")]
        public async Task<IActionResult> GetAllLeaveType()
        {
            try
            {
                var results = await _repository.GettAllLeaveTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        [Route("GetLeaveType /{id}")]
        public async Task<IActionResult> GetLeaveType(int id)
        {
            try
            {
                var result = await _repository.GetLeaveTypeAsync(id);
                if (result == null) return NotFound("leave type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost]
        [Route("AddLeaveType")]
        public async Task<IActionResult> AddLeaveType(LeaveTypeViewModel lvm)
        {
            var levaeType = new LeaveType { LeaveTypeName = lvm.LeaveTypeName, LeaveTypeDescription = lvm.LeaveTypeDescription };
            try
            {
                _repository.Add(levaeType);
                await _repository.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
            return Ok(levaeType);
        }

        [HttpPut]
        [Route("EditLeaveType/{id}")]
        public async Task<ActionResult<LeaveTypeViewModel>> EditLeaveType(int id, LeaveTypeViewModel lvm)
        {
            try
            {
                var existingLeaveType = await _repository.GetLeaveTypeAsync(id);
                if (existingLeaveType == null) return NotFound($"The leave type does not exist");
                existingLeaveType.LeaveTypeName = lvm.LeaveTypeName;
                existingLeaveType.LeaveTypeDescription = lvm.LeaveTypeDescription;

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(existingLeaveType);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }


        [HttpDelete]
        [Route("DeleteLeaveType/{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            try
            {
                var existingLeaveType = await _repository.GetLeaveTypeAsync(id);
                if (existingLeaveType == null) return NotFound($"The leave type does not exist");
                _repository.Delete(existingLeaveType);
                if (await _repository.SaveChangesAsync())
                    return Ok(existingLeaveType);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
            return BadRequest("Your request is invalid.");
        }

        [HttpGet]
        [Route("GetAllClaimTypes")]
        public async Task<IActionResult> GetAllClaimTypes()
        {
            try
            {
                var results = await _repository.GetAllClaimTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        // GET: api/MoTime/GetClaimType/5
        [HttpGet]
        [Route("GetClaimType/{id}")]
        public async Task<IActionResult> GetClaimType(int id)
        {
            try
            {
                var result = await _repository.GetClaimTypeAsync(id);

                if (result == null) return NotFound("Claim type does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        // POST: api/MoTime/AddClaimType
        [HttpPost]
        [Route("AddClaimType")]
        public async Task<IActionResult> AddClaimType(ClaimTypeViewModel claimTypeVM)
        {
            var claimType = new ClaimType
            {
                ClaimTypeName = claimTypeVM.ClaimTypeName,
                ClaimTypeDescription = claimTypeVM.ClaimTypeDescription
            };

            try
            {
                _repository.Add(claimType);
                await _repository.SaveChangesAsync();

                return Ok(claimType);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        // PUT: api/MoTime/UpdateClaimType/5
        [HttpPut]
        [Route("UpdateClaimType/{id}")]
        public async Task<IActionResult> UpdateClaimType(int id, ClaimTypeViewModel claimTypeVM)
        {
            var claimType = await _repository.GetClaimTypeAsync(id);

            if (claimType == null)
            {
                return NotFound("Claim type does not exist");
            }

            claimType.ClaimTypeName = claimTypeVM.ClaimTypeName;
            claimType.ClaimTypeDescription = claimTypeVM.ClaimTypeDescription;

            try
            {
                await _repository.SaveChangesAsync();

                return Ok(claimType);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        // DELETE: api/MoTime/DeleteClaimType/5
        [HttpDelete]
        [Route("DeleteClaimType/{id}")]
        public async Task<IActionResult> DeleteClaimType(int id)
        {
            var claimType = await _repository.GetClaimTypeAsync(id);

            if (claimType == null)
            {
                return NotFound("Claim type does not exist");
            }

            try
            {
                _repository.Delete(claimType);
                await _repository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }


        //// claim item
        //[HttpGet]
        //[Route("GetAllClaimItem")]
        //public async Task<IActionResult> GetAllClaimItem()
        //{
        //    try
        //    {
        //        var results = await _repository.GetAllClaimItemAsync();
        //        return Ok(results);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        //[HttpGet]
        //[Route("GetClaimItem /{id}")]
        //public async Task<IActionResult> GetClaimItem(int id)
        //{
        //    try
        //    {
        //        var result = await _repository.GetClaimItemAsync(id);
        //        if (result == null) return NotFound("Claim item does not exist");
        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}

        ////////////////////////////////////////////////////////////GET HELP///////////////////////////////

        [HttpGet]
        [Route("GetAllHelp")]
        public async Task<IActionResult> GetAllHelp()
        {
            try
            {
                var results = await _repository.GetAllHelpAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet]
        [Route("GetHelp/{id}")]
        public async Task<IActionResult> GetHelp(int id)
        {
            try
            {
                var result = await _repository.GetHelpTypeAsync(id);
                if (result == null) return NotFound("Help does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("AddHelp")]
        public async Task<IActionResult> AddHelp([FromForm] HelpViewModel helpVM)
        {
            try
            {
                if (helpVM.Material == null || helpVM.Material.Length == 0)
                    return BadRequest("No file uploaded");

                var helpType = await _repository.GettAllHelpTypeAsync();
                var helpTypes = helpType.FirstOrDefault(h => h.HelpTypeId == helpVM.HelpTypeId);

                if (helpTypes == null)
                    return BadRequest("Invalid help type");

                byte[] fileBytes;
                string fileName;

                using (var memoryStream = new MemoryStream())
                {
                    await helpVM.Material.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                    fileName = helpVM.Material.FileName;
                }

                var help = new Help
                {
                    HelpName = helpVM.HelpName,
                    HelpDescription = helpVM.HelpDescription,
                    HelpTypeId = helpVM.HelpTypeId,
                    Material = fileBytes,
                    FileName = fileName
                };

                _repository.Add(help);
                await _repository.SaveChangesAsync();

                return Ok(help);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("EditHelp/{id}")]
        public async Task<IActionResult> EditHelp(int id, [FromForm] HelpViewModel helpVM)
        {
            try
            {
                var help = await _repository.GetHelpByIdAsync(id);
                if (help == null)
                    return NotFound("Help does not exist");

                var helpType = await _repository.GettAllHelpTypeAsync();
                var helpTypes = helpType.FirstOrDefault(h => h.HelpTypeId == helpVM.HelpTypeId);

                if (helpTypes == null)
                    return BadRequest("Invalid help type");

                if (helpVM.Material != null && helpVM.Material.Length > 0)
                {
                    byte[] fileBytes;
                    string fileName;

                    using (var memoryStream = new MemoryStream())
                    {
                        await helpVM.Material.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                        fileName = helpVM.Material.FileName;
                    }

                    help.Material = fileBytes;
                    help.FileName = fileName;
                }

                help.HelpName = helpVM.HelpName;
                help.HelpDescription = helpVM.HelpDescription;
                help.HelpTypeId = helpVM.HelpTypeId;

                await _repository.SaveChangesAsync();

                return Ok(help);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("DeleteHelp/{id}")]
        public async Task<IActionResult> DeleteHelp(int id)
        {
            try
            {
                var help = await _repository.GetHelpByIdAsync(id);
                if (help == null)
                    return NotFound("Help does not exist");

                _repository.Delete(help);
                await _repository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }
        [HttpGet]
        [Route("DownloadHelp/{id}")]
        public async Task<IActionResult> DownloadHelp(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest("Invalid help ID");

                var help = await _repository.GetHelpByIdAsync(id.Value);
                if (help == null)
                    return NotFound("Help does not exist");

                // Check if the HelpType exists in the dictionary, if not, use a default content type
                string contentType = FileTypeMappings.TryGetValue(help.HelpTypeId ?? 0, out string mappedContentType)
                    ? mappedContentType
                    : "application/octet-stream"; // A default content type if the HelpType is not found in the dictionary

                // Set the Content-Disposition header to suggest the filename when downloading
                var contentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = help.FileName // Set the actual filename here
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                // You can set other response headers if needed (e.g., Cache-Control)

                // Return the file content with the appropriate content type
                return File(help.Material, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("PlayVideo/{id}")]
        public async Task<IActionResult> PlayVideo(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest("Invalid video ID");

                var video = await _repository.GetHelpByIdAsync(id.Value);
                if (video == null)
                    return NotFound("Video does not exist");

                // Set the content type to the appropriate video format (e.g., "video/mp4")
                string contentType = "video/mp4"; // Adjust according to your video format

                // Set the response headers for streaming
                Response.Headers.Add("Accept-Ranges", "bytes");
                Response.Headers.Add("Content-Type", contentType);

                // Stream the video content directly to the response stream
                return new FileContentResult(video.Material, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //LEAVE
        [HttpGet]
        [Route("GetAllLevae")]
        public async Task<IActionResult> GetAllLeave()
        {
            try
            {
                var results = await _repository.GetAllLeaveAsync();

                // Map the results to HelpViewModel objects
                var viewModelList = results.Select(Leave => new LeaveViewModel
                {
                    EmployeeId = Leave.Employee.EmployeeId,
                    LeaveTypeId = Leave.LeaveType.LeaveTypeId,
                    StartDate = Leave.StartDate,
                    EndDate = Leave.EndDate
                }).ToList();

                return Ok(viewModelList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }



        [HttpGet]
        [Route("GetLeave/{id}")]
        public async Task<IActionResult> GetLeave(int id)
        {
            try
            {
                var result = await _repository.GetLeaveByIdAsync(id);
                if (result == null)
                    return NotFound("Leave does not exist");

                var viewModel = new LeaveViewModel
                {
                    EmployeeId = result.Employee.EmployeeId,
                    LeaveTypeId = result.LeaveType.LeaveTypeId,
                    StartDate = result.StartDate,
                    EndDate = result.EndDate
                };

                return Ok(viewModel);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost]
        [Route("AddLeave")]
        public async Task<IActionResult> AddLeave(LeaveViewModel LVM)
        {
            try
            {
                if (LVM.EmployeeId == null)
                    return BadRequest("Employee ID is required.");

                if (LVM.LeaveTypeId == null)
                    return BadRequest("Leave type ID is required.");

                if (LVM.StartDate == null || LVM.EndDate == null)
                    return BadRequest("Start date and end date are required.");

                var leaveType = await _repository.GetLeaveTypeAsync(LVM.LeaveTypeId.Value);

                if (leaveType == null)
                    return BadRequest("Invalid leave type.");

                var employee = await _repository.GetEmployeeAsync(LVM.EmployeeId.Value);

                if (employee == null)
                    return BadRequest("Invalid employee.");

                var leave = new Leave
                {
                    Employee = employee,
                    LeaveType = leaveType,
                    StartDate = LVM.StartDate.Value,
                    EndDate = LVM.EndDate.Value
                };

                _repository.Add(leave);
                await _repository.SaveChangesAsync();

                return Ok(leave);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }
        }
        [HttpPut]
        [Route("EditLeave")]
        public async Task<IActionResult> EditLeave(int id, LeaveViewModel leaveVM)
        {
            try
            {
                var leave = await _repository.GetLeaveByIdAsync(id);
                if (leave == null)
                    return NotFound("Leave does not exist");

                if (leaveVM.EmployeeId != null)
                {
                    var employee = await _repository.GetEmployeeAsync(leaveVM.EmployeeId.Value);

                    if (employee == null)
                        return BadRequest("Invalid employee ID");

                    leave.Employee = employee;
                }

                if (leaveVM.LeaveTypeId != null)
                {
                    var leaveType = await _repository.GetLeaveTypeAsync(leaveVM.LeaveTypeId.Value);

                    if (leaveType == null)
                        return BadRequest("Invalid leave type ID");

                    leave.LeaveType = leaveType;
                }

                if (leaveVM.StartDate != null)
                    leave.StartDate = leaveVM.StartDate.Value;

                if (leaveVM.EndDate != null)
                    leave.EndDate = leaveVM.EndDate.Value;

                await _repository.SaveChangesAsync();

                return Ok(leave);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        [HttpDelete]
        [Route("deleteleave")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            try
            {
                var leave = await _repository.GetLeaveByIdAsync(id);
                if (leave == null)
                    return NotFound("Leave does not exist");

                _repository.Delete(leave);
                await _repository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }
        [HttpGet]
        [Route("GetAllTimesheet")]
        public async Task<IActionResult> GetAllTimesheetAsync()
        {
            try
            {
                var results = await _repository.GetAllTimesheetAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetTimesheet /{id}")]
        public async Task<IActionResult> GetTimesheetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetTimesheetByIdAsync(id);
                if (result == null) return NotFound("Timesheet does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetAllProjectRequests")]
        public async Task<IActionResult> GetAllProjectRequests()
        {
            try
            {
                var results = await _repository.GetAllProjectRequestAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost]
        [Route("LogProjectRequest")]
        public async Task<IActionResult> LogProjectRequest(ProjectRequestViewModel pvm)
        {
            try
            {
                if (pvm.EmployeeId == null)
                    return BadRequest("Employee ID is required.");

                if (pvm.ProjectRequestStatusId == null)
                    return BadRequest("Project Request Status ID is required.");

                if (pvm.ProjectRequestDescription == null)
                    return BadRequest("Please enter project request description");
                var employee = await _repository.GetEmployeeAsync(pvm.EmployeeId.Value);

                if (employee == null)
                    return BadRequest("Invalid employee.");

                var projectRequestStatus = await _repository.GetProjectRequestStatusAsync(pvm.ProjectRequestStatusId.Value);
                if (projectRequestStatus == null)
                {
                    return BadRequest("Invalid Project Request Status");
                }

                var projectRequest = new ProjectRequest
                {
                    Employee = employee,
                    ProjectRequestStatus = projectRequestStatus,
                    ProjectRequestDate = DateTime.Today,
                    ProjectRequestDescription = pvm.ProjectRequestDescription,
                    ProjectRequestStatusId = 1
                };

                _repository.Add(projectRequest);
                await _repository.SaveChangesAsync();

                return Ok(projectRequest);
            }
            catch (DbUpdateException ex)
            {
                // Log or examine the inner exception
                if (ex.InnerException != null)
                {
                    // Log or print the inner exception's message and details
                    var innerExceptionMessage = ex.InnerException.Message;
                    var innerExceptionStackTrace = ex.InnerException.StackTrace;
                    // Handle the error appropriately
                }
                return BadRequest("Error while saving changes to the database.");
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }
        }

        [HttpGet]
        [Route("GetProjectRequestsByUserId/{id}")]
        public async Task<IActionResult> GetProjectRequestsByUserId(int id)
        {
            try
            {
                var requests = await _repository.GetProjectRequestByIdAsync(id);
                return Ok(requests);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        // Implement this method to get the logged-in user's ID
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            // Default user ID if not authenticated or an issue occurred
            return -1; // Example value
        }
        //private int GetUserIdFromEmployeeId(int employeeId)
        //{
        //    // Assuming you have access to your database context
        //    var user = _dbContext.Employees
        //        .Where(e => e.EmployeeId == employeeId)
        //        .Select(e => e.UserId)
        //        .FirstOrDefault();

        //    return user;
        //}

        [HttpGet]
        [Route("GetAllProjectRequestStatus")]
        public async Task<IActionResult> GetAllProjectRequestStatus()
        {
            try
            {
                var results = await _repository.GetAllProjectRequestStatusesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetProjectStatusById/{id}")]
        public async Task<IActionResult> GetProjectRequestStatusByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetProjectRequestStatusAsync(id);
                if (result == null) return NotFound("The project Request status does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetAllTaskStatuses")]
        public async Task<IActionResult> GetAllTaskStatuses()
        {
            try
            {
                var results = await _repository.GettAllTaskStatusAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetTaskStatusById/{id}")]
        public async Task<IActionResult> GetTaskStatusAsync(int id)
        {
            try
            {
                var result = await _repository.GetTaskStatusAsync(id);
                if (result == null) return NotFound("The task status does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetAllTaskPriorities")]
        public async Task<IActionResult> GetAllTaskPriorities()
        {
            try
            {
                var results = await _repository.GettAllTaskPriorityAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetTaskPriorityById/{id}")]
        public async Task<IActionResult> GetTaskPriorityByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetTaskPriorityAsync(id);
                if (result == null) return NotFound("The task priority does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet]
        [Route("GetAllTaskTypes")]
        public async Task<IActionResult> GetAllTaskTypes()
        {
            try
            {
                var results = await _repository.GettAllTaskTypeAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetTaskTypeById/{id}")]
        public async Task<IActionResult> GetTaskTypeByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetTaskTypeAsync(id);
                if (result == null) return NotFound("The task type does not exist");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                var results = await _repository.GetAllTasksAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        //[HttpGet]
        //[Route("GetAllTasks")]
        //public async Task<IActionResult> GetAllTasks()
        //{
        //    try
        //    {
        //        var results = await _repository.GetAllTasksAsync();

        //        // Map the results to TaskViewModel objects
        //        var viewModelList = results.Select(Taski => new TaskViewModel
        //        {
        //            EmployeeId = Taski.Employee?.EmployeeId, // Add null check with '?'
        //            ProjectId = Taski.Project?.ProjectId,
        //            TaskStatusId = Taski.TaskStatus?.TaskStatusId,
        //            PriorityId = Taski.Priority?.PriorityId,
        //            TaskTypeId = Taski.TaskType?.TaskTypeId,
        //            TaskName = Taski.TaskName,
        //            TaskDescription = Taski.TaskDescription,
        //            DueDate = Taski.DueDate,
        //            IsComplete = Taski.IsComplete
        //        }).ToList();

        //        return Ok(viewModelList);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal server error.");
        //    }
        //}



        [HttpGet]
        [Route("GetTaskById/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var result = await _repository.GetTaskByIdAsync(id);
                if (result == null)
                {
                    return NotFound("Where??");
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask(TaskViewModel tvm)
        {
            try
            {
                if (tvm.EmployeeId == null)
                    return BadRequest("Employee ID is required.");

                if (tvm.TaskStatusId == null)
                    return BadRequest("Task Status ID is required.");

                if (tvm.TaskTypeId == null)
                    return BadRequest("Task Type ID is required.");

                if (tvm.PriorityId == null)
                    return BadRequest("Priority ID is required.");

                if (tvm.TaskName == null)
                    return BadRequest("Please enter task name");

                if (tvm.TaskDescription == null)
                    return BadRequest("Task description is required.");

                if (tvm.DueDate == null)
                    return BadRequest("The due date is required.");

                var taskStatus = await _repository.GetTaskStatusAsync(tvm.TaskStatusId.Value);

                if (taskStatus == null)
                    return BadRequest("Invalid task status.");

                var taskType = await _repository.GetTaskTypeAsync(tvm.TaskTypeId.Value);

                if (taskType == null)
                    return BadRequest("Invalid task type.");

                var priority = await _repository.GetTaskPriorityAsync(tvm.PriorityId.Value);

                if (priority == null)
                    return BadRequest("Invalid priority.");

                var employee = await _repository.GetEmployeeAsync(tvm.EmployeeId.Value);

                if (employee == null)
                    return BadRequest("Invalid employee.");

                var project = await _repository.GetProjectAsync(tvm.ProjectId.Value);
                if (project == null)
                {
                    return BadRequest("Invalid Project");
                }

                var task = new Models.Task
                {
                    Employee = employee,
                    Project = project,
                    TaskStatus = taskStatus,
                    TaskType = taskType,
                    Priority = priority,
                    TaskName = tvm.TaskName,
                    TaskDescription = tvm.TaskDescription,
                    DueDate = tvm.DueDate.Value,
                    IsComplete = tvm.IsComplete.Value
                };

                _repository.Add(task);
                await _repository.SaveChangesAsync();

                return Ok(task);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction.");
            }
        }
        [HttpPut]
        [Route("EditTask/{id}")]
        public async Task<IActionResult> EditTask(int id, TaskViewModel tvm)
        {
            try
            {
                var task = await _repository.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound("The task does not exist");

                if (tvm.EmployeeId != null)
                {
                    var employee = await _repository.GetEmployeeAsync(tvm.EmployeeId.Value);

                    if (employee == null)
                        return BadRequest("Invalid employee ID");

                    task.Employee = employee;
                }
                if (tvm.ProjectId != null)
                {
                    var project = await _repository.GetProjectAsync(tvm.ProjectId.Value);

                    task.Project = project;
                }

                if (tvm.TaskStatusId != null)
                {
                    var taskStatus = await _repository.GetTaskStatusAsync(tvm.TaskStatusId.Value);

                    if (taskStatus == null)
                        return BadRequest("Invalid task status ID");

                    task.TaskStatus = taskStatus;
                }
                if (tvm.TaskTypeId != null)
                {
                    var taskType = await _repository.GetTaskTypeAsync(tvm.TaskTypeId.Value);

                    if (taskType == null)
                        return BadRequest("Invalid task type ID");

                    task.TaskType = taskType;
                }
                if (tvm.PriorityId != null)
                {
                    var priority = await _repository.GetTaskPriorityAsync(tvm.PriorityId.Value);

                    if (priority == null)
                        return BadRequest("Invalid task priority ID");

                    task.Priority = priority;
                }
                if (tvm.TaskName != null)
                    task.TaskName = task.TaskName;

                if (tvm.TaskDescription != null)
                    task.TaskDescription = task.TaskDescription;

                if (tvm.DueDate != null)
                    task.DueDate = tvm.DueDate.Value;

                if (tvm.IsComplete != null)
                    task.IsComplete = tvm.IsComplete.Value;
                await _repository.SaveChangesAsync();

                return Ok(task);
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        [HttpDelete]
        [Route("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var task = await _repository.GetTaskByIdAsync(id);
                if (task == null)
                    return NotFound("The task you're trying to delete does not exist");

                _repository.Delete(task);
                await _repository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Invalid transaction");
            }
        }

        //***********************************************************************************
        //**************************** REPORTS **********************************************
        [HttpGet]
        [Route("GetControlBreakReport/{selectedProjectName}")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetControlBreakReport(string selectedProjectName)
        {
            try
            {
                var reportData = await GenerateControlBreakReport(selectedProjectName);
                return Ok(reportData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private async Task<IEnumerable<dynamic>> GenerateControlBreakReport(string selectedProjectName)
        {
            var reportData = new List<dynamic>();

            // Query ProjectAllocations with selected project name
            var projectAllocations = await _dbContext.ProjectAllocations
                .Include(pa => pa.Employee)
                .Where(pa => pa.Project.ProjectName == selectedProjectName)
                .ToListAsync();

            if (projectAllocations.Any())
            {
                var groupedData = projectAllocations
                    .GroupBy(pa => pa.Project.ProjectName)
                    .ToList();

                foreach (var group in groupedData)
                {
                    var projectTeamReport = new
                    {
                        ProjectName = group.Key,
                        EmployeeReports = group.Select(pa => new
                        {
                            EmployeeId = pa.Employee.EmployeeId,
                            EmployeeName = pa.Employee.User.FirstName,
                            TotalHours = pa.TotalNumHours
                        }).ToList(),
                        TotalTeamHours = group.Sum(pa => pa.TotalNumHours)
                    };

                    reportData.Add(projectTeamReport);
                }
            }

            return reportData;
        }
        //***********************************************************************************************
        //*************************     UPLOADING THE CLIENTS *******************************************
        [HttpPost]
        [Route("UploadClients")]
        public async Task<IActionResult> UploadClients(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid transaction");

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var clients = ExcelHelper.ParseClientsFromStream(stream);

                    foreach (var client in clients)
                    {
                        _repository.Add(client);
                    }

                    await _repository.SaveChangesAsync();
                    return Ok(new
                    {
                        Message = "Client List Successfully Uploaded!"
                    });
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                // Log or print inner exception details for troubleshooting
                Console.WriteLine(innerException.Message);
                return BadRequest("Yeah your error is with the database");
                // Handle the exception
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }
        [HttpGet]
        [Route("DownloadClients")]
        public async Task<IActionResult> DownloadClients()
        {
            try
            {
                var clientsArray = await _repository.GetAllClientAsync(); // Await the asynchronous operation
                var clientsList = clientsArray.ToList(); // Convert array to List<Client>
                var excelBytes = ExcelHelper.GenerateExcelBytes(clientsList);

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "clients.xlsx");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetITDepartment")]
        public async Task<IActionResult> GetITDepartment()
        {
            try
            {
                var results = await _repository.GetITDepartment();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        //  events
        [HttpGet]
        [Route("GetAllEventReports")]
        public async Task<IActionResult> GetAllEventReports()
        {

            try
            {
                var results = await _repository.GetAllEventReportAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetEventReport/{Id}")]
        public async Task<IActionResult> GetEventReportAsync(int Id)
        {
            try
            {
                var result = await _repository.GetEventReportAsync(Id);

                if (result == null)
                    return NotFound("Event does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        // max hours
        [HttpGet]
        [Route("GetAllMaxHours")]
        public async Task<IActionResult> GetAllMaximumHours()
        {

            try
            {
                var results = await _repository.GetAllMaximumHourAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }

        [HttpGet]
        [Route("GetMaximumHour/{Id}")]
        public async Task<IActionResult> GetMaximumHourAsync(int Id)
        {
            try
            {
                var result = await _repository.GetMaximumHourAsync(Id);

                if (result == null)
                    return NotFound("Project allocation does not exist");

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error. Please contact support.");
            }
        }



    }

}

