using Contracts.SQLDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Entities
{
    [Table("Client")]
    public class ClientEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual ICollection<RideEntity> Rides { get; set; }
    }
}
