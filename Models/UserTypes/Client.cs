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
    public class Client : UserProfile
    {

        [DataMember]
        public Guid ClientId { get; set; }

        public Client() { }

        public Client(UserProfile user)
        {
            this.Id = user.Id;
            this.Address = user.Address;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Username = user.Username;
            this.DateOfBirth = user.DateOfBirth;
            this.Fullname = user.Fullname;
            this.Type = UserType.CLIENT;
            this.ImagePath = user.ImagePath;
        }

        public override Guid GetDictKey()
        {
            return ClientId;
        }
    }
}
