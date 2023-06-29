using ApiHelper;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Gruppe2API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {
        [HttpGet("GetStadium")]
        public async Task<Stadium?> GetStadium(int stadiumId)
        {
            Helper helper = new Helper();
            return await helper.GetStadium(stadiumId);
        }

        [HttpGet("GetSeats")]
        public async Task<List<Seat>?> GetMatches(int stadiumId, int matchId, int sectionId)
        {
            Helper helper = new Helper();
            return await helper.GetSeats(stadiumId, matchId, sectionId);
        }

        [HttpPost("BuySeats")]
        public async Task<bool> PostSeats(List<TakenSeat> seat)
        {
            Helper helper = new Helper();
            return await helper.BuySeat(seat);
        }
    }
}