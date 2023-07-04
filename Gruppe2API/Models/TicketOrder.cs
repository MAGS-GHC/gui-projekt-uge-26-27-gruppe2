﻿using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TicketOrder
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string OrderNumber { get => $"Order#{Id}"; }
        public List<TakenSeat> TakenSeats { get; set; }
    }
}
