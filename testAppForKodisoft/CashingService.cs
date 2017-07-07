using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using testAppForKodisoft.Models;
using testAppForKodisoft.Services;

namespace testAppForKodisoft
{
    public class CashingService
    {
        private RequestDelegate _next;
        private IMemoryCache _memoryCache;
        private IMyCollectionService _collectionService;
        private IParseRSS _rss;

        public CashingService(RequestDelegate next, IMemoryCache memCache, IMyCollectionService collectionService, IParseRSS rss)
        {
            _next = next;
            _memoryCache = memCache;
            _collectionService = collectionService;
            _rss = rss;
        }

        public async Task Invoke(HttpContext context)
        {
            IList<string> cacheKey = _collectionService.getAllNames();

            IList<Feeds> feeds=null;
            foreach (var key in cacheKey)
            {
                if (!_memoryCache.TryGetValue(key, out feeds))
                {
                    feeds = _rss.ParseCategories(_collectionService.GetCustomFeeds(key));

                    // cashe for 5 minutes
                    _memoryCache.Set(key, feeds,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
            }

            //if(feeds!=null)
            //    await context.Response.HttpContext

        }
    }

    public static class DbCacheExtensions
    {
        public static IApplicationBuilder UseCaching(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CashingService>();
        }
    }

}
