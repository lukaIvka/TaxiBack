using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SQLDB
{
    public interface ISQLCRUD<TDB, TM> where TDB : class, BaseEntity
    {
        Task AddOrUpdateMultipleEntities(IEnumerable<TDB> entities);
        IEnumerable<TM> GetAllEntities();
    }
}
