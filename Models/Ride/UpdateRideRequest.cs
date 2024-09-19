using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Ride
{
    [DataContract]
    [KnownType(typeof(UpdateRideWithTimeEstimate))]
    public class UpdateRideRequest
    {
        [DataMember]
        public Guid RideId { get; set; }

        [DataMember]
        [Required]
        public RideStatus Status { get; set; }
    }

    [DataContract]
    public class UpdateRideWithTimeEstimate : UpdateRideRequest
    {
        [DataMember]
        public int RideEstimateSeconds { get; set; }
    }
}
