using Models;
using MySql.Data.MySqlClient;
using static System.Collections.Specialized.BitVector32;
using System.Data;

namespace ApiHelper
{
    public class Helper
    {
        string connectionString;

        // Other methods and properties...

        public Helper()
        {
            connectionString = "Server=sql7.freemysqlhosting.net; User ID=sql7629046; Password=23deIfAcse; Database=sql7629046";
        }

        private Stadium MapStadium(IDataReader reader)
        {
            var stadium = new Stadium
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString(reader["Name"]),
                MatchList = new List<Match>()
            };

            return stadium;
        }

        private Match MapMatch(IDataReader reader)
        {
            var match = new Match
            {
                MatchId = Convert.ToInt32(reader["MatchId"]),
                SoldSeats = Convert.ToInt32(reader["SoldSeats"]),
                MatchDate = Convert.ToDateTime(reader["MatchDate"]),
                HomeTeam = Convert.ToString(reader["HomeTeam"]),
                AwayTeam = Convert.ToString(reader["AwayTeam"]),

            };

            return match;
        }


        public List<Match> GetMatches(int stadiumId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $"SELECT * FROM MatchInfo WHERE StadionId = {stadiumId}";
                List<Match> matches = new List<Match>();
                var command = new MySqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    matches.Add(MapMatch(reader));

                }

                return matches;
            }
        }


        public Stadium GetStadium(int stadiumId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $"SELECT * FROM Stadion WHERE Id = {stadiumId}";
                Stadium stadium = null;
                var command = new MySqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    if (stadium == null)
                    {
                        stadium = MapStadium(reader);
                    }

                   
                }

                if (stadium != null)
                {
                    stadium.MatchList = GetMatches(stadium.Id);
                }

                return stadium;
            }
        }



    }
}