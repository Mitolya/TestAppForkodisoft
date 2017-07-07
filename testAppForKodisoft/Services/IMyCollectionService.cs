using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testAppForKodisoft.Models;

namespace testAppForKodisoft.Services
{
    public interface IMyCollectionService
    {
        List<string> getAllNames();
        Task<bool> SaveChangesAsync();
        IList<MyCollection> GetFeeds();
        void AddFeedName(string name);
        void DeleteFeed(string name);
        void RenameFeed(string id, string value);
        IList<string> GetCustomFeeds(string name);
        void AddCustomFeed(string name, string value);
        void DeleteCustomFeed(string feed, string value);
    }
}
