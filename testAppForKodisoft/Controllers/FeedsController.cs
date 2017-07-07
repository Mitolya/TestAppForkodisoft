using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using testAppForKodisoft.Services;

namespace testAppForKodisoft.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/{feed}")]
    public class FeedsController : Controller
    {
        private IMyCollectionService _service;

        public FeedsController(IMyCollectionService service)
        {
            _service = service;
        }

        // GET:api/Feeds/{feed}
        [HttpGet("")]
        public IActionResult Get(string feed)
        {
            try
            {
                var custfeeds = _service.GetCustomFeeds(feed);
                return Ok(custfeeds.ToList());
            }
            catch (Exception e)
            {
                return BadRequest($"No such feeds {feed}");
            }
        }

        // POST: Feeds/{feed}
        [HttpPost("")]
        public async Task<IActionResult> Post(string feed, [FromBody]string value)
        {
            _service.AddCustomFeed(feed, value);
            if (await _service.SaveChangesAsync())
                return Ok(_service.GetCustomFeeds(feed));

            return BadRequest($"Can't add new custom feed");


        }

        // DELETE:api/Feeds/{feed}
        [HttpDelete("", Name = "deletefeed")]
        public async Task<IActionResult> Delete(string feed, [FromBody]string value)
        {
            _service.DeleteCustomFeed(feed, value);
            if (await _service.SaveChangesAsync())
                return Ok(_service.GetCustomFeeds(feed));
            return BadRequest($"Can't delete new custom feed");
        }

    }
}
