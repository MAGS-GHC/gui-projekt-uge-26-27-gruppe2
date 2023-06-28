using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Gruppe2API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StadiumController : ControllerBase
    {
        [HttpGet(Name = "GetStadium")]
        public Stadium? GetStadium(int stadiumId)
        {
            return new ApiHelper.Helper().GetStadium(stadiumId);
        }

        //[HttpGet(Name = "GetMatches")]
        //public List<Match>? GetMatches(int stadiumId)
        //{
        //    return new ApiHelper.Helper().GetMatches(stadiumId);
        //}

    }
}