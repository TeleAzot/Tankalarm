using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Data.API.DTO
{
    public class TankerkoenigResponse
    {
        public bool ok { get; set; }
        public List<TankerkoenigStation> stations { get; set; }
    }

    public class TankerkoenigStation
    {
        public string name { get; set; }
        public string brand { get; set; }
        public string street { get; set; }
        public string place { get; set; }
        public int postCode { get; set; }
        public double dist { get; set; }
        public double price { get; set; }
        public bool isOpen { get; set; }
    }
}