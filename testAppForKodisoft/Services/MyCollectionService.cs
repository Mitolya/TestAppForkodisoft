using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using testAppForKodisoft.Models;

namespace testAppForKodisoft.Services
{
    public class MyCollectionService:IMyCollectionService
    {
        
        private DataContext _context;

        public MyCollectionService(DataContext context)
        {
            _context = context;
            
        }

        public List<string> getAllNames()
        {
            return _context.Myfeeds.Select(x=>x.Name).ToList();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
        public IList<MyCollection> GetFeeds()
        {
            var mylist = new List<MyCollection>();
            foreach (var var in _context.Myfeeds.ToList())
            {
                mylist.Add(Mapper.Map<MyCollection>(var));
            }
            
            return mylist;
        }

        public void AddFeedName(string name)
        {
            var feed = new MyCollection(){Name = name,Feeds = new List<string>()};
            _context.Myfeeds.Add(Mapper.Map<MyEntityDb>(feed));
            
        }

        public void DeleteFeed(string name)
        {
            var feed = _context.Myfeeds.FirstOrDefault(x => x.Name == name);
            if (feed != null)
            {
                _context.Myfeeds.Remove(feed);
            }
            
        }

        public void RenameFeed(string id, string value)
        {
            var feed = _context.Myfeeds.FirstOrDefault(x => x.Name == id);
            
            if (feed!=null)
            {
                feed.Name = value;
            }
            
        }

        public IList<string> GetCustomFeeds(string name)
        {
            var feed = _context.Myfeeds.FirstOrDefault(x=>x.Name==name);
            var feedlist =Mapper.Map<MyCollection>(feed);
            return feedlist.Feeds.ToList();

        }

        public void AddCustomFeed(string name, string value)
        {
            var feed = _context.Myfeeds.FirstOrDefault(x => x.Name == name);
            if (feed!=null && value!=string.Empty)
            {
                if (Enum.IsDefined(typeof(FeedSites),value) )
                {
                    if (feed.SerializedListOfStrings.Equals(""))
                        feed.SerializedListOfStrings=value;
                    else
                    {
                        feed.SerializedListOfStrings = string.Concat(string.Concat(feed.SerializedListOfStrings, ";"), value);
                    }
                    _context.Myfeeds.Update(feed);
                }
            }
            
        }

        public void DeleteCustomFeed(string name, string value)
        {
            var feed = _context.Myfeeds.FirstOrDefault(x => x.Name == name);
            
            if (feed != null)
            {
                if (Enum.IsDefined(typeof(FeedSites),value) && feed.SerializedListOfStrings!=string.Empty)
                {
                    feed.SerializedListOfStrings=feed.SerializedListOfStrings.Replace(value, "");
                    //if(feed.SerializedListOfStrings.EndsWith(""))
                    _context.Myfeeds.Update(feed);
                    //mapped.Feeds.Remove(value);
                    //_context.Myfeeds.Update(Mapper.Map<MyEntityDb>(mapped));
                    ////_context.SaveChangesAsync();
                }
            }
        }
    }
}
