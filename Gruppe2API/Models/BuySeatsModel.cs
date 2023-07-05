using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuySeatsModel
    {
        public List<TakenSeat> Seats { get; set; }
        public string Email { get; set; }
    }
}
