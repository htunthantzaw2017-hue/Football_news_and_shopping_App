namespace ManchesterUnitedApp.Data.Entities
{
    public class Base
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTimeOffset UpdatedOn { get; set; }
    }
}
