using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Data.Entities.MatchEntity
{
    public class MatchEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HomeTeamName { get; set; }

        public string? HomeTeamLogoPath { get; set; }

        [Required]
        public string AwayTeamName { get; set; }

        public string? AwayTeamLogoPath { get; set; }

        [Required]
        public DateTime MatchDate { get; set; }

        public int? HomeScore { get; set; }

        public int? AwayScore { get; set; }

        public bool IsFinished { get; set; } = false;
    }
}
