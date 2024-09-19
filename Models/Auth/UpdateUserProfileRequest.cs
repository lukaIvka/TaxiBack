using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Auth
{
    [DataContract]
    public class UpdateUserProfileRequest
    {
        [DataMember]
        public string? Username { get; set; }

        [DataMember]
        public string? Password { get; set; }

        [DataMember]
        public string? Fullname { get; set; }

        [DataMember]
        public string? Address { get; set; }

        [DataMember]
        public string? ImagePath { get; set; }
    }
}
