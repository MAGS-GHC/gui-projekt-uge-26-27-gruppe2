using Models;
using Newtonsoft.Json;
namespace BookingSide.APICon
{
    public class ApiÁccess
    {
       

        //Ny HttpClient
        

        public async Task<Stadium?> GetStadium(int id)
        {
            string url = $"https://localhost:44342/Stadium/GetStadium?stadiumId={id}";
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<Stadium>(await response.Content.ReadAsStringAsync());            
        }

        public async Task<List<Seat>?> GetSeats(int stadiumId, int matchId, int sectionId)
        {
            string url = $"https://localhost:44342/Stadium/GetSeats?stadiumId={stadiumId}&matchId={matchId}&sectionId={sectionId}";
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            return JsonConvert.DeserializeObject<List<Seat>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<TicketOrder?> GetStadium(List<TakenSeat> seatList, string email)
        {
            string url = "https://localhost:44342/Stadium/GetStadium?stadiumId=1";
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(new BuySeatsModel { Seats = seatList, Email = email})));            
            return JsonConvert.DeserializeObject<TicketOrder>(await response.Content.ReadAsStringAsync());
        }

    }
}
