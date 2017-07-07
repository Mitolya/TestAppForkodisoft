using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testAppForKodisoft.Models;

namespace testAppForKodisoft.Services
{
    public interface IParseRSS
    {
        dynamic Parce(string resources);
        IList<Feeds> ParseCategories(IEnumerable<string> category);
    }
}
