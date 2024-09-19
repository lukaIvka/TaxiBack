using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Ride
{
    [DataContract]
    public class QueryRideParams
    {
        [DataMember]
        public RideStatus? Status { get; set; }
        [DataMember]
        public Guid RideId { get; set; }

        [DataMember]
        public Guid? ClientId { get; set; }

        [DataMember]
        public Guid? DriverId { get; set; }
    }
}
