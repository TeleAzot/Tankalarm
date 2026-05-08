using System;
using System.Collections.Generic;
using System.Text;

namespace Tankalarm.Data.API.DTO
{
    public class GeoResponse
    {
        public List<GeoResult> results { get; set; }
    }

    public class GeoResult
    {
        public double lon { get; set; }

        public double lat { get; set; }
    }
}
