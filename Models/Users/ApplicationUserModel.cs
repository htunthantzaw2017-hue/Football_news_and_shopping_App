namespace ManchesterUnitedApp.Models.Users
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public byte[]? ProfileImage { get; set; }
        public bool IsAdmin { get; set; }
    }
}
