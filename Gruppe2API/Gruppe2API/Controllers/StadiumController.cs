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
        public async Task<int> PostSeats(List<TakenSeat> seat, string email, double price)
        {
            return await helper.BuySeat(seat, email, price);
        }


        [HttpGet("GetTicketOrder")]
        public async Task<TicketOrder> GetTicketOrder (int id)
        {
            return await helper.GetTicketOrder(id);
        }
    }
}