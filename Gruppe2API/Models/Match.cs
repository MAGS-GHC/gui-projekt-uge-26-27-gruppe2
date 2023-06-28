using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Match
    {
        public int MatchId { get; set; }

        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }

        public DateTime MatchDate { get; set; }

        public int SoldSeats { get; set; }
    }
}
