using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TakenSeat
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int SeatId { get; set; }
    }
}
