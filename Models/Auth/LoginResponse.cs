using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Auth
{
    [DataContract]
    public class LoginResponse
    {
        [DataMember]
        public Guid userId { get; set; }

        [DataMember]
        public Guid roleId { get; set; }

        [DataMember]
        public UserType userType { get; set; }
    }
}
