using ApiHelper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;

namespace Gruppe2API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {
        Helper helper;
        public StadiumController() {
            helper = new Helper();
        }

        [HttpGet("GetStadium")]
        public async Task<Stadium?> GetStadium(int stadiumId)
        {
            return await helper.GetStadium(stadiumId);
        }

        [HttpGet("GetSeats")]
        public async Task<List<Seat>?> GetMatches(int stadiumId, int matchId, int sectionId)
        {
            return await helper.GetSeats(stadiumId, matchId, sectionId);
        }

        [HttpPost("BuySeats")]
        public async Task<TicketOrder> PostSeats(string jsonString)
        {            
            BuySeatsModel buy = JsonConvert.DeserializeObject<BuySeatsModel>(jsonString);
            return await helper.BuySeat(buy.Seats, buy.Email);
        }
    }
}