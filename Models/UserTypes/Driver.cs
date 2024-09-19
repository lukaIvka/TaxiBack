using Models.Auth;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserTypes
{
    public enum DriverStatus
    {
        NOT_VERIFIED = 0,
        VERIFIED = 1,
        BANNED = 2
    }

    [DataContract]
    public class Driver : UserProfile
    {
        [DataMember]
        public DriverStatus Status { get; set; }

        [DataMember]
        public Guid DriverId { get; set; }

        public Driver() { }

        public Driver(UserProfile user, DriverStatus status)
        {
            this.Id = user.Id;
            this.Address = user.Address;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Status = status;
            this.Username = user.Username;
            this.DateOfBirth = user.DateOfBirth;
            this.Fullname = user.Fullname;
            this.Type = UserType.DRIVER;
            this.ImagePath = user.ImagePath;
        }

        public override Guid GetDictKey()
        {
            return DriverId;
        }
    }
}
