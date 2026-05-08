using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManchesterUnitedApp.Models.Player
{
    public class PlayerEntryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Range(1,100, ErrorMessage = "age must greater than 0")]
        public int Age { get; set; }
        public DateTime DOB { get; set; }
        public string Nationality { get; set; } = string.Empty;
        [Range(1, 1000, ErrorMessage = "height must greater than 0")]
        public string Height { get; set; } = string.Empty;
        [Range(1, 1000, ErrorMessage = "weight must greater than 0")]
        public string Weight { get; set; } = string.Empty;
        [Range(1, 1000, ErrorMessage = "Jersey must greater than 0")]
        public int JerseyNumber { get; set; }
        public Guid GenderTypeId { get; set; }
        public Guid TeamId { get; set; }
        public Guid PlayerPositionId { get; set; }
        public Guid PlayerStatusId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile Photo { get; set; }
        public byte[]? ImageData { get; set; }
        public string ImageType { get; set; } = string.Empty;
    }
}