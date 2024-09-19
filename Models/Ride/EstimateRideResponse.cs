using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Ride
{

    public class EstimateRideResponse
    {
        public int EstimatedDriverArrivalSeconds { get; set; }
        public float PriceEstimate { get; set; }
    }
}
