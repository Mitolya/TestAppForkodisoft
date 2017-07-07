using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using testAppForKodisoft.Models;

namespace testAppForKodisoft.Services
{
    public class ParceRSS:IParseRSS
    {
        public IList<Feeds> ParseCategories(IEnumerable<string> category)
        {
            FeedSites feed;
            var feeds = new List<Feeds>();
            foreach (var cat in category)
            {
                Enum.TryParse(cat, true, out feed);
                switch (feed)
                {
                    case FeedSites.autoblog:
                        feeds.AddRange(Mapper.Map<IEnumerable<Feeds>>(ParceAutoblog()));
                        break;
                    case FeedSites.engadget:
                        feeds.AddRange(Mapper.Map<IEnumerable<Feeds>>(ParseEngadged()));
                        break;
                    default:
                        throw new NotSupportedException($"{cat.ToString()} is not supported");
                }
            }
            return feeds.OrderByDescending(x => x.PublishDate.Date).ThenByDescending(x => x.PublishDate.Hour).ToList();
        }

        private XDocument getDocument(string url)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                return XDocument.Load(response.Content.ReadAsStreamAsync().Result);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public dynamic Parce(string value)
        {
            FeedSites parser;
            if (Enum.TryParse(value, true, out parser))
            {
                switch (parser)
                {
                    case FeedSites.autoblog:
                        return ParceAutoblog();
                    case FeedSites.engadget:
                        return ParseEngadged();
                    default:
                        throw new NotSupportedException($"{parser.ToString()} is not supported");
                }
            }
            else
            {
                return "No such Resource";
            }
            
        }



        private IList<EngadgetModel> ParseEngadged()
        {
            var url = "https://www.engadget.com/rss.xml";
            XDocument doc = getDocument(url);
            var listOfEngadget = new List<EngadgetModel>();
            foreach (var feed in doc.Elements("rss").Elements("channel").Elements("item"))
            {
                List<string> category = new List<string>();
                foreach (var cat in feed.Elements("category"))
                {
                    category.Add(cat.Value);
                }
                
                listOfEngadget.Add(new EngadgetModel
                {
                    Title = feed.Element("title").Value,
                    Link = feed.Element("link").Value,
                    PublishDate = DateTime.ParseExact(feed.Element("pubDate").Value, "ddd, dd MMM yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture).ToUniversalTime(),
                    Categories = category,
                    Comments = feed.Element("comments").Value
                });
            }

            return listOfEngadget;
        }

        
        private IList<AutoblogModel> ParceAutoblog()
        {
            var url = "http://www.autoblog.com/rss.xml";
            XDocument doc =getDocument(url);
            
            var listOfAutoblog = new List<AutoblogModel>();
            
            foreach (var feed in doc.Elements("rss").Elements("channel").Elements("item"))
            {
                
                List<string> category = new List<string>();
                foreach (var cat in feed.Elements("category"))
                {
                    category.Add(cat.Value);
                }
                listOfAutoblog.Add(new AutoblogModel
                {
                    Title = feed.Element("title").Value,
                    Link = feed.Element("link").Value,
                    PublishDate = DateTime.ParseExact(feed.Element("pubDate").Value, "ddd, dd MMM yyyy HH:mm:ss EDT", CultureInfo.InvariantCulture).ToUniversalTime(),
                    Categories = category,
                    Creator = feed.Element("{http://purl.org/dc/elements/1.1/}creator").Value,
                    Comments = feed.Element("comments").Value,
                    Image = feed.Element("enclosure").Attribute("url").Value
                });
            }

            return listOfAutoblog;
        }

       
    }
}
