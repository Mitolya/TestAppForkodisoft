using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testAppForKodisoft.Models
{
    public class MyCollection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Feeds { get; set; }
    }

    //for DB 
    public class MyEntityDb
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerializedListOfStrings { get; set; }
    }
}
