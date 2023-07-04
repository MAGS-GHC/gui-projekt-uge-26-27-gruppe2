using Models;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Data.Common;
using System.Web;

namespace ApiHelper
{
    public class Helper
    {
        string connectionString;

        public Helper()
        {
            string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            string appSettingsContent = File.ReadAllText(appSettingsPath);
            JObject appSettingsJson = JObject.Parse(appSettingsContent);
            connectionString = (string)appSettingsJson["ConnectionStrings"]["MyConn"];
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
                Id = Convert.ToInt32(reader["Id"]),
                StadiumId = Convert.ToInt32(reader["StadiumId"]),
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

        private Seat MapSeat(IDataReader reader)
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

        private TicketOrder MapTicketOrder(IDataReader reader)
        {
            var order = new TicketOrder
            {
                Id = Convert.ToInt32(reader["Id"]),
                Email = Convert.ToString(reader["Email"]),
            };
            return order;
        }

        private TakenSeat MapTakenSeat(IDataReader reader)
        {
            var seat = new TakenSeat
            {
                Id = Convert.ToInt32(reader["Id"]),
                MatchId = Convert.ToInt32(reader["MatchId"]),
                SeatId = Convert.ToInt32(reader["SeatId"]),
                TickerOrderId = Convert.ToInt32(reader["TicketOrderId"])
            };
            return seat;
        }

        private async Task<List<StadiumSection>> GetStadiumSection(int stadiumId)
        {
            string query = $"SELECT * FROM StadiumSection WHERE StadiumId = {stadiumId}";
            List<StadiumSection> sections = new List<StadiumSection>();
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);
            await connection.OpenAsync();
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                sections.Add(MapStadiumSection(reader));
            }

            connection.Close();

            return sections;

        }

        public async Task<List<MatchInfo>> GetMatches(int stadiumId)
        {
            string query = $"SELECT * FROM MatchInfo WHERE StadiumId = {stadiumId}";
            List<MatchInfo> matches = new List<MatchInfo>();
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);
            await connection.OpenAsync();
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                matches.Add(MapMatch(reader));
            }
            connection.Close();
            return matches;

        }

        public async Task<Stadium> GetStadium(int stadiumId)
        {
            string query = $"SELECT * FROM Stadium WHERE Id = {stadiumId}";
            Stadium stadium = null;
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);
            await connection.OpenAsync();
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                if (stadium == null)
                {
                    stadium = MapStadium(reader);
                }
            }

            if (stadium != null)
            {
                stadium.MatchList = await GetMatches(stadium.Id);
                stadium.Sections = await GetStadiumSection(stadium.Id);
            }
            connection.Close();
            return stadium;
        }

        public async Task<List<Seat>> GetSeats(int stadiumId, int matchId, int sectionId)
        {

            string query = $@"SELECT s.* FROM Seat s JOIN Row r ON s.RowId = r.Id 
                            JOIN StadiumSection ss ON r.SectionId = ss.Id 
                            JOIN Stadium st ON ss.StadiumId = st.Id 
                            WHERE ss.Id = {sectionId}  AND st.Id = {stadiumId};";

            List<Seat> allSeats = new();

            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            await connection.OpenAsync();

            DbDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                allSeats.Add(MapSeat(reader));
            }

            List<TakenSeat> takenSeats = await GetTakenSeats(stadiumId, matchId, sectionId);

            takenSeats.ForEach(t =>
            {
                allSeats.FirstOrDefault(s => s.Id == t.SeatId).IsTaken = true;
            });

            connection.Close();
            return allSeats;

        }

        public async Task<List<TakenSeat>> GetTakenSeats(int stadiumId, int matchId, int sectionId)
        {
            string query = $@"SELECT ts.* FROM TakenSeat ts
                         JOIN Seat s ON ts.SeatId = s.Id      
                         JOIN Row r ON s.RowId = r.Id 
                         JOIN StadiumSection ss ON r.SectionId = ss.Id
                         JOIN `MatchInfo` mi ON ts.MatchId = mi.Id
                         WHERE ss.Id = {sectionId}
                         AND ss.StadiumId = {stadiumId} 
                         AND ts.MatchId = {matchId}";

            List<TakenSeat> seats = new();

            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            await connection.OpenAsync();

            DbDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                seats.Add(MapTakenSeat(reader));
            }
            connection.Close();
            return seats;

        }

        public async Task<TicketOrder> BuySeat(List<TakenSeat> seats, string email)
        {
            List<TakenSeat> takenSeats = await CheckTakenSeats(seats);
            foreach (TakenSeat tSeat in takenSeats)
            {
                if (seats.Any(s => s.SeatId == tSeat.SeatId && s.MatchId == tSeat.MatchId))
                {
                    return null;
                }
            }

            int ticketOrderId = await SetTicketOrder(email);
            if (ticketOrderId == -1) return null;

            string query = $@"INSERT IGNORE INTO TakenSeat (MatchId, SeatId, TicketOrderId) VALUES";
            seats.ForEach(s =>
            {
                query += $" ({s.MatchId},{s.SeatId}),{ticketOrderId},";
            });
            query = query.Remove(query.Length - 1, 1);
            query += ";";
            bool inserted = false;
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            await connection.OpenAsync();

            var rowAffected = await command.ExecuteNonQueryAsync();

            inserted = rowAffected == seats.Count;
            connection.Close();

            if (!inserted) return null;
            TicketOrder ticketOrder = await GetTicketOrderId(ticketOrderId);
            return ticketOrder;
        }

        async Task<TicketOrder> GetTicketOrderId(int id)
        {
            string query = $"SELECT * FROM TicketOrder Where Id = {id}";
            TicketOrder ticketOrder = new TicketOrder();
            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);
            await connection.OpenAsync();
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                ticketOrder = MapTicketOrder(reader);
            }

            connection.Close();
            return ticketOrder;
        }

        async Task<int> SetTicketOrder(string email)
        {
            string query = $"INSERT INTO TicketOrder (Email) VALUES ('{email}');";

            string idQuery = $"SELECT LAST_INSERT_ID() AS Id;";

            int id = -1;
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(query, connection);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            command = new MySqlCommand(idQuery, connection);

            var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                if (id != -1)
                {
                    id = Convert.ToInt32(reader["Id"]);
                }
            }
            connection.Close();
            return id;
        }

        private async Task<List<TakenSeat>> CheckTakenSeats(List<TakenSeat> seats)
        {
            string getQuery = $"SELECT * FROM TakenSeat Where";

            seats.ForEach(s =>
            {
                getQuery += $" MatchId = {s.MatchId} AND SeatId = {s.SeatId} OR";
            });
            getQuery = getQuery.Remove(getQuery.Length - 2, 2);
            getQuery += ";";

            List<TakenSeat> takenSeats = new List<TakenSeat>();

            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(getQuery, connection);
            await connection.OpenAsync();
            DbDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                takenSeats.Add(MapTakenSeat(reader));
            }
            connection.Close();
            return takenSeats;
        }
    }
}