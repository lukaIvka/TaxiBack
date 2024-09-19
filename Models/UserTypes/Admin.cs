using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserTypes
{
    [DataContract]
    public class Admin : UserProfile
    {
        [DataMember]
        public Guid AdminId { get; set; }

        public override Guid GetDictKey()
        {
            return AdminId;
        }
    }
}
