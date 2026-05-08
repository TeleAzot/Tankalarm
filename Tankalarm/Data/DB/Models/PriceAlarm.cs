using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Data.DB.Models
{
    public class PriceAlarm
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string FuelType { get; set; }

        public double TargetPrice { get; set; }
    }
}