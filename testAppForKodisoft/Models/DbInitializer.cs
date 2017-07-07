using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace testAppForKodisoft.Models
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Myfeeds.Any())
            {
                return;
            }


            var feeds = new List<MyCollection>()
            {
                new MyCollection{Name = "Car1",Feeds = new List<string>(){FeedSites.autoblog.ToString(),FeedSites.engadget.ToString()}},
                new MyCollection{Name = "Car2",Feeds = new List<string>(){FeedSites.autoblog.ToString()}}
            };

            foreach (var c in feeds)
            {
                context.Myfeeds.Add(Mapper.Map<MyEntityDb>(c));
            }
            context.SaveChanges();
        }
    }
}
