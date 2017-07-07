using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testAppForKodisoft.Models
{
    public class EngadgetModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
        public DateTime PublishDate { get; set; }
        public string Creator { get; set; }
        public IList<string> Categories { get; set; }
    }
}
