using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.SQLDB
{
    public interface IDTOConverter<TDB, TM> where TDB : class, BaseEntity
    {
        TM SQLToAppModel(TDB sqlModel);
        TDB AppModelToSQL(TM appModel);
    }
}
