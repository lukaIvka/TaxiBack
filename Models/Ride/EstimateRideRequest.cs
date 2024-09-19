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
    public class EstimateRideRequest
    {
        [DataMember]
        [Required]
        [MinLength(3)]
        public string StartAddress { get; set; }
        
        [DataMember]
        [Required]
        [MinLength(3)]
        public string EndAddress { get; set; }
    }
}
