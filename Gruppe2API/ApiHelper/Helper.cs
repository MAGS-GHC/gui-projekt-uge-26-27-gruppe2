using Models;
using MySql.Data.MySqlClient;
using static System.Collections.Specialized.BitVector32;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

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
                MatchList = new List<MatchInfo>()
            };

            return stadium;
        }

        private MatchInfo MapMatch(IDataReader reader)
        {
            var match = new MatchInfo
            {
                Id = Convert.ToInt32(reader["MatchId"]),
                SoldSeats = Convert.ToInt32(reader["SoldSeats"]),
                MatchDate = Convert.ToDateTime(reader["MatchDate"]),
                HomeTeam = Convert.ToString(reader["HomeTeam"]),
                AwayTeam = Convert.ToString(reader["AwayTeam"]),

            };

            return match;
        }

        private StadiumSection MapStadiumSection(IDataReader reader)
        {
            var section = new StadiumSection
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = Convert.ToString((string)reader["Name"]),
                StandingSection = Convert.ToInt32(reader["StandingSection"]) == 1 ? true : false,
                Rows = Convert.ToInt32(reader["Rows"]),


            };

            return section;
        }

        public Seat MapSeat(IDataReader reader)
        {
            var seat = new Seat
            {
                Id = Convert.ToInt32(reader["Id"]),
                RowId = Convert.ToInt32(reader["RowId"]),
                RowNumber = Convert.ToInt32(reader["RowNumber"]),
                SeatNumber = Convert.ToInt32(reader["SeatNumber"]),
            };
            return seat;
        }

        public TakenSeat MapTakenSeat(IDataReader reader)
        {
            var seat = new TakenSeat
            {
                Id = Convert.ToInt32(reader["Id"]),
                MatchId = Convert.ToInt32(reader["MatchId"]),
                SeatId = Convert.ToInt32(reader["SeatId"]),
            };
            return seat;
        }


        private List<StadiumSection> GetStadiumSection(int stadiumId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $"SELECT * FROM StadiumSection WHERE StadiumId = {stadiumId}";
                List<StadiumSection> sections = new List<StadiumSection>();
                var command = new MySqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    sections.Add(MapStadiumSection(reader));

                }

                return sections;
            }
        }


        public List<MatchInfo> GetMatches(int stadiumId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $"SELECT * FROM MatchInfo WHERE StadiumId = {stadiumId}";
                List<MatchInfo> matches = new List<MatchInfo>();
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
                var query = $"SELECT * FROM Stadium WHERE Id = {stadiumId}";
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
                    stadium.Sections = GetStadiumSection(stadium.Id);
                }

                return stadium;
            }
        }




        public List<Seat> GetSeats(int stadiumId, int matchId, int sectionId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $"SELECT s.* FROM Seat s JOIN Row r ON s.RowId = r.Id JOIN StadiumSection ss ON r.SectionId = ss.Id JOIN Stadium st ON ss.StadiumId = st.Id WHERE ss.Id = {sectionId}  AND st.Id = {stadiumId};";
                List<Seat> allSeats = new();
                var command = new MySqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    allSeats.Add(MapSeat(reader));
                }

                List<TakenSeat> takenSeats = GetTakenSeats(stadiumId, matchId, sectionId);

                takenSeats.ForEach(t =>
                {
                    allSeats.FirstOrDefault(s => s.Id == t.SeatId).IsTaken = true;
                });

                return allSeats;
            }
        }


        public List<TakenSeat> GetTakenSeats(int stadiumId, int matchId, int sectionId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var query = $@"SELECT ts.* FROM TakenSeat ts
                         JOIN Seat s ON ts.SeatId = s.Id      
                         JOIN Row r ON s.RowId = r.Id 
                         JOIN StadiumSection ss ON r.SectionId = ss.Id
                         JOIN `MatchInfo` mi ON ts.MatchId = mi.Id
                         WHERE ss.Id = {sectionId}
                         AND ss.StadiumId = {stadiumId} 
                         AND ts.MatchId = {matchId}";

                List<TakenSeat> seats = new();
                var command = new MySqlCommand(query, connection);

                connection.Open();

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    seats.Add(MapTakenSeat(reader));
                }

                return seats;
            }
        }
    }
}