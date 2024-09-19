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
    [Table("User")]
    public class UserEntity : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [MinLength(3)]
        public string Username { get; set; }
        [MinLength(3)]
        public string Email { get; set; }
        public string Password { get; set; }
        [MinLength(3)]
        public string Fullname { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MinLength(3)]
        public string Address { get; set; }
        public int Type { get; set; }
        [MinLength(1)]
        public string ImagePath { get; set; }
    }
}
