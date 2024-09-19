using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class RatingDataService : BaseDataService<Models.UserTypes.RideRating, RatingEntity>, Contracts.Database.IRatingDataService
    {
        public RatingDataService(
            Synchronizer<RatingEntity, 
            Models.UserTypes.RideRating> synchronizer, 
            IReliableStateManager stateManager
        ) : base(synchronizer, stateManager)
        {}

        public async Task<Models.UserTypes.RideRating> RateDriver(Models.UserTypes.RideRating driverRating)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var key = $"{driverRating.Id}";
            var newRating = await dict.AddOrUpdateAsync(txWrapper.transaction, key, driverRating, (key, value) => value);

            return newRating;
        }
    }
}
