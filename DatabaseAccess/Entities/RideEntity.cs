using Contracts.SQLDB;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Entities
{
    [Table("Ride")]
    public class RideEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long CreatedAtTimestamp { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public DateTime EstimatedDriverArrival { get; set; }
        public DateTime? EstimatedRideEnd { get; set; }
        public int Status { get; set; }
        public float Price { get; set; }

        public Guid ClientId { get; set; }
        public virtual ClientEntity Client { get; set; }

        public Guid? DriverId { get; set; }
        public virtual DriverEntity? Driver { get; set; }
        public virtual RatingEntity? Rating { get; set; }
    }
}
