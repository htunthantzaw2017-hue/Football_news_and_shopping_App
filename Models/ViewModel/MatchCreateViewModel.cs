using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManchesterUnitedApp.Models.ViewModel
{
    public class MatchCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the home team name.")]
        [Display(Name = "Home Team")]
        public string HomeTeamName { get; set; }

        [NotMapped]
        [Display(Name = "Home Team Logo (Upload New)")]
        public IFormFile? HomeLogoFile { get; set; }


        [Required(ErrorMessage = "Please enter the away team name.")]
        [Display(Name = "Away Team")]
        public string AwayTeamName { get; set; }

        [NotMapped]
        [Display(Name = "Away Team Logo (Upload New)")]
        public IFormFile? AwayLogoFile { get; set; }


        [Required(ErrorMessage = "Please specify the match date and time.")]
        [Display(Name = "Match Date (UTC)")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime MatchDate { get; set; }

        [Display(Name = "Home Score")]
        [Range(0, 99, ErrorMessage = "Score must be non-negative.")]
        public int? HomeScore { get; set; }

        [Display(Name = "Away Score")]
        [Range(0, 99, ErrorMessage = "Score must be non-negative.")]
        public int? AwayScore { get; set; }

        [Display(Name = "Match Finished?")]
        public bool IsFinished { get; set; } = false;
    }
}
