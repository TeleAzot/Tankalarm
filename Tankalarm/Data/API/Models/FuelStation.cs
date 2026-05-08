using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Data.API.Models
{
    public class FuelStation
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        public double Distance { get; set; }
        public double Price { get; set; }
        public bool IsOpen { get; set; }
    }
}