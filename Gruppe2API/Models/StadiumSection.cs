using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public  class StadiumSection
    {
        public int Id { get; set; }
        public int StadionId { get; set; }
        public string Name { get; set; }
        public int Rows { get; set; }
        public bool StandingSection { get; set; }

        public double Price { get; set; }
    }
}
