namespace MoTimeAPI.ViewModels
{
    public class UserUpdateViewModel
    {
        public int UserId { get; set; }
        public int TitleId { get; set; }
        public int UserRoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string FileName { get; set; }
    }
}
