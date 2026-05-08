namespace ManchesterUnitedApp.Models.Users
{
    public class RoleGroupedUsersModel
    {
        public IList<ApplicationUserModel> Admins { get; set; } = new List<ApplicationUserModel>();
        public IList<ApplicationUserModel> Users { get; set; } = new List<ApplicationUserModel>();
    }
}
