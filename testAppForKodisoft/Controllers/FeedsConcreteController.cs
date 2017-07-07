using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAppForKodisoft.Models;
using testAppForKodisoft.Services;

namespace testAppForKodisoft.Controllers
{
    [Produces("application/json")]
    [Route("api/")]
    public class FeedsConcreteController : Controller
    {
        private IParseRSS _rss;

        public FeedsConcreteController(IParseRSS rss)
        {
            _rss = rss;
        }
        // GET: api/FeedsConcrete
        [HttpGet("feedsConcrete/{concrete}")]
        public IActionResult Get(string concrete)
        {
            return Ok(_rss.Parce(concrete));
        }

       

    }
}
