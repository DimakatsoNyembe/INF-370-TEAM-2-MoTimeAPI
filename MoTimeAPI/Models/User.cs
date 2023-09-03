using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? TitleId { get; set; }

    public int? PasswordId { get; set; }

    public int? UserRoleId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string Token { get; set; }

    public string ResetPasswordToken { get; set; }

    public DateTime? ResetPasswordExpiry { get; set; }

    public byte[] ProfilePicture { get; set; }

    public string FileName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? TokenExpiration { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Password Password { get; set; }

    public virtual ICollection<SystemAdministrator> SystemAdministrators { get; set; } = new List<SystemAdministrator>();

    public virtual Title Title { get; set; }

    public virtual UserRole UserRole { get; set; }

    public virtual ICollection<UserUserRole> UserUserRoles { get; set; } = new List<UserUserRole>();
}
