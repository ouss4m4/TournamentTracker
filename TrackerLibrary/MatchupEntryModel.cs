using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represents one team in the matchup
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// Represents the score for this particular team
        /// </summary>
        public TeamModel Winner { get; set; }
        /// <summary>
        /// Represents the matchup tha this team came 
        /// from as the winner.
        /// </summary>
        public int MatchupRound { get; set; }

    }
}
