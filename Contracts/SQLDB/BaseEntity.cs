using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SQLDB
{
    public interface BaseEntity
    {
        public Guid Id { get; set; }
    }
}
