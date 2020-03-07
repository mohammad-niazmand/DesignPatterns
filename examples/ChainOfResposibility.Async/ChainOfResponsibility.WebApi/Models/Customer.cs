using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChainOfResponsibility.WebApi.Models
{
    public class Customer
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
}