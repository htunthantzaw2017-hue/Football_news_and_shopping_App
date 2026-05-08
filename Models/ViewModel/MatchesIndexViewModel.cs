using System.Collections.Generic;

namespace ManchesterUnitedApp.Models.ViewModel
{
    public class MatchesIndexViewModel
    {
        public IEnumerable<MatchViewModel> UpcomingMatches { get; set; }
        public IEnumerable<MatchViewModel> MatchResults { get; set; }
    }
}
