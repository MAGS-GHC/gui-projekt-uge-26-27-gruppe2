using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TempOrder
    {
        public string Email { get; set; }
        public int SeatId { get; set; }
        public int TicketOrderId { get; set; }

        public double TotalPrice { get; set; }
    }
}
