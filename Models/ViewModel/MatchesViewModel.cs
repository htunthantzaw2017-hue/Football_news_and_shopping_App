using System.ComponentModel.DataAnnotations;

namespace ManchesterUnitedApp.Models.ViewModel
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public string HomeTeamName { get; set; }
        public string? HomeTeamLogoPath { get; set; }

        public string AwayTeamName { get; set; }
        public string? AwayTeamLogoPath { get; set; }

        [Display(Name = "Match Date")]
        public DateTime MatchDate { get; set; }

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public bool IsFinished { get; set; }

        // Helper property for UI logic
        public string DisplayScore => IsFinished ? $"{HomeScore} - {AwayScore}" : "VS";
    }
}
