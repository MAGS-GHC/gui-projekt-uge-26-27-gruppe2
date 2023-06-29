using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;

namespace Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StadiumSection> Sections { get; set; }
        public List<MatchInfo> MatchList { get; set; }

        public Stadium()
        {
            Sections = new List<StadiumSection>();
            MatchList = new List<MatchInfo>();
        }
    }
}