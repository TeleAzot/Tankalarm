using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Database.Models
{
    public class PriceAlarm
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public FuelType FuelType { get; set; }

        public double TargetPrice { get; set; }
    }

    public enum FuelType
    {
        Diesel = 0,
        Super = 1,
        SuperE10 = 2
    }
}