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
    [Route("api/[controller]")]
    public class MyFeedsController : Controller
    {
        private IMyCollectionService _service;

        public MyFeedsController(IMyCollectionService service)
        {
            _service = service;
        }
        // GET: api/MyFeeds
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var list = _service.GetFeeds().ToList();
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest("Failed to load data");
            }
        }

        
        
        // POST: api/MyFeeds
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            try
            {
                _service.AddFeedName(value);
                if( await _service.SaveChangesAsync())
                    return Ok(_service.GetFeeds());
                return BadRequest("Failed to create new feed");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to create new feed");
            }
            

        }
        
        // PUT: api/MyFeeds/{feed}
        [HttpPut("{feed}")]
        public async Task<IActionResult> Put(string feed, [FromBody]string value)
        {
            try
            {
                _service.RenameFeed(feed, value);
                if(await _service.SaveChangesAsync())
                    return Ok(_service.GetFeeds());
                return BadRequest("Failed to rename");

            }
            catch (Exception e)
            {
                return BadRequest("Failed to rename");
            }
        }

        // DELETE: api/MyFeeds/{feed}
        [HttpDelete("{feed}")]
        public async Task<IActionResult> DeleteAsync(string feed)
        {
            try
            {
                _service.DeleteFeed(feed);
                if ( await _service.SaveChangesAsync())
                    return Ok(_service.GetFeeds());
                return BadRequest($"Failed to delete {feed}");
            }
            catch (Exception e)
            {
                return BadRequest($"Failed to delete {feed}");
            }
        }
    }
}
