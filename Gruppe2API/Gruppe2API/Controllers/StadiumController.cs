using ApiHelper;
using Microsoft.AspNetCore.Mvc;
using Models;


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
        public async Task<TicketOrder> PostSeats(List<TakenSeat> seat, string email)
        {
            return await helper.BuySeat(seat, email);
        }
    }
}