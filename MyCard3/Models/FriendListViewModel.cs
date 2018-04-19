using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCard3.Models
{
    public class FriendListViewModel
    {
        public IEnumerable<Person> Friends { get; set; }
        public IDictionary<int, string> LastMessage { get; set; }
    }
}