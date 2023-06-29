using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Gruppe2API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {
        [HttpGet("GetStadium")]
        public Stadium? GetStadium(int stadiumId)
        {
            return new ApiHelper.Helper().GetStadium(stadiumId);
        }

        [HttpGet("GetSeats")]
        public List<Seat>? GetMatches(int stadiumId, int matchId, int sectionId)
        {
            return new ApiHelper.Helper().GetSeats(stadiumId, matchId, sectionId);
        }

    }
}