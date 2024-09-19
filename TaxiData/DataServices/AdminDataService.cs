using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class AdminDataService : BaseDataService<Models.UserTypes.Admin, AdminEntity>
    {
        public AdminDataService(Synchronizer<AdminEntity, Admin> synchronizer, IReliableStateManager stateManager) : base(synchronizer, stateManager)
        {}
    }
}
