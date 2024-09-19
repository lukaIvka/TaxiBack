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
    public class CreateRideRequest
    {
        [DataMember]
        public Guid Id { get; set; }  

        [DataMember]
        [Required]
        [MinLength(3)]
        public string StartAddress { get; set; }
        
        [DataMember]
        [Required]
        [MinLength(3)]
        public string EndAddress { get; set; }
        
        [DataMember]
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public float Price { get; set; }

        [DataMember]
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int EstimatedDriverArrivalSeconds { get; set; }
    }
}
