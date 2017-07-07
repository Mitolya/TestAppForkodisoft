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
    [Route("api/Category")]
    public class CategoryController : Controller
    {
        private IParseRSS _rss;
        private IMyCollectionService _service;

        public CategoryController(IParseRSS rss,IMyCollectionService service)
        {
            _rss = rss;
            _service = service;
        }
        // GET: api/Category/5
        [HttpGet("{category}", Name = "Get")]
        public IActionResult Get(string category)
        {
            try
            {
                var list = _service.GetCustomFeeds(category);

                return Ok(_rss.ParseCategories(list));
            }
            catch (Exception e)
            {
                return BadRequest("wrong");
            }
        }
        
        
    }
}
