using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_A_UIRoute.Models
{
    public class BookModel
    {
        public string BookCode { get; set; }
        public string BookName { get; set; }
        public string BookDesc{ get; set; }


        public string BookAuthor { get; set; }
        public string Mode { get; set; }
    }
}