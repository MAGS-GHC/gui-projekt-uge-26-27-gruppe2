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
                Price = Convert.ToDouble(reader["Price"])
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

        private TempOrder MapTempTicket(IDataReader reader)
        {

            var order = new TempOrder
            {
                Email = Convert.ToString(reader["Email"]),
                SeatId = Convert.ToInt32(reader["SeatId"]),
                TicketOrderId = Convert.ToInt32(reader["TicketOrderId"]),
                TotalPrice = Convert.ToDouble(reader["TotalPrice"]),
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

        public async Task<int> BuySeat(List<TakenSeat> seats, string email, double price)
        {
            List<TakenSeat> takenSeats = await CheckTakenSeats(seats);
            foreach (TakenSeat tSeat in takenSeats)
            {
                if (seats.Any(s => s.SeatId == tSeat.SeatId && s.MatchId == tSeat.MatchId))
                {
                    return -1;
                }
            }

            int ticketOrderId = await SetTicketOrder(email, price);
            if (ticketOrderId == -1) return -1;

            string query = $@"INSERT IGNORE INTO TakenSeat (MatchId, SeatId, TicketOrderId) VALUES";
            seats.ForEach(s =>
            {
                query += $" ({s.MatchId},{s.SeatId},{ticketOrderId}),";
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

            if (!inserted) return -1;

            return ticketOrderId;
        }



        async Task<int> SetTicketOrder(string email, double price)
        {
            string query = $"INSERT INTO TicketOrder (Email, TotalPrice) VALUES ('{email}',{price});";

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
                id = Convert.ToInt32(reader["Id"]);
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


        public async Task<TicketOrder> GetTicketOrder(int id)
        {
            string getQuery = $"SELECT t1.Email, t1.TotalPrice, t2.SeatId, t2.TicketOrderId FROM TicketOrder t1 INNER JOIN TakenSeat t2  ON t1.Id = t2.TicketOrderId WHERE t1.Id = {id}";

            MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(getQuery, connection);

            await connection.OpenAsync();

            DbDataReader reader = await command.ExecuteReaderAsync();

            List<TempOrder> tempOrders = new();

            while (reader.Read())
            {
                tempOrders.Add(MapTempTicket(reader));
            }

            connection.Close();

            TicketOrder ticketOrder = new TicketOrder();
            ticketOrder.Id = tempOrders[0].TicketOrderId;
            ticketOrder.TotalPrice = tempOrders[0].TotalPrice;
            ticketOrder.Email = tempOrders[0].Email;

            tempOrders.ForEach(t => { ticketOrder.Seats.Add(t.SeatId); });
            return ticketOrder;
        }
    }
}