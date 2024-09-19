using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Models.Auth;
using Models.Ride;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class DriverDataService : BaseDataService<Models.UserTypes.Driver,DriverEntity>, Contracts.Database.IDriverDataService
    {
        public DriverDataService(
            Synchronizer<DriverEntity, Models.UserTypes.Driver> synchronizer,
            IReliableStateManager stateManager
        ) : base(synchronizer, stateManager)
        {}

        public async Task<DriverStatus> GetDriverStatus(Guid id)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var existingDriver = await dict.TryGetValueAsync(txWrapper.transaction, $"{id}");

            if (!existingDriver.HasValue)
            {
                return default;
            }

            return existingDriver.Value.Status;
        }

        public async Task<bool> UpdateDriverStatus(Guid id, DriverStatus status)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var key = $"{id}";
            var existingDriver = await dict.TryGetValueAsync(txWrapper.transaction, key);
            if (!existingDriver.HasValue)
            {
                return false;
            }
            existingDriver.Value.Status = status;
            var result = await dict.TryUpdateAsync(txWrapper.transaction, key, existingDriver.Value, existingDriver.Value);
            return result;
        }

        public async Task<IEnumerable<Models.UserTypes.Driver>> ListAllDrivers()
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var collectionEnum = await dict.CreateEnumerableAsync(txWrapper.transaction);
            var asyncEnum = collectionEnum.GetAsyncEnumerator();

            var drivers = new List<Models.UserTypes.Driver>();

            while (await asyncEnum.MoveNextAsync(default))
            {
                var driverEntity = asyncEnum.Current.Value;
                if (driverEntity != null)
                {
                    drivers.Add(driverEntity);
                }
            }

            return drivers;
        }


        public async Task<bool> Create(Models.UserTypes.Driver driver)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var dictKey = $"{driver.DriverId}";
            var created = await dict.AddOrUpdateAsync(txWrapper.transaction, dictKey, driver, (key, value) => value);
            return created != null;
        }

        public async Task<bool> CreateDriver(Driver appModel)
        {
            return await Create(appModel);
        }

        public async Task<float> GetAverageRatingForDriver(Guid driverId)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var dictKey = $"{driverId}";

            var driver = await dict.TryGetValueAsync(txWrapper.transaction, dictKey);

            if (!driver.HasValue) 
            {
                return 0.0f;
            }

            var driverRides = await GetDriverRides(driverId, txWrapper.transaction);
            if (!driverRides.Any()) 
            {
                return 0.0f;
            }
            
            var totalAvg = 0.0f;
            var totalCnt = 0;
            foreach (var ride in driverRides)
            {
                var ratings = await GetRatingsForRide(ride.Id, txWrapper.transaction);
                if (!ratings.Any()) continue;
                totalCnt++;
                totalAvg += (float)ratings.Average(rating => rating.Value);
            }

            if (totalCnt == 0) return 0;

            return totalAvg / totalCnt;
        }

        private async Task<IEnumerable<Ride>> GetDriverRides(Guid driverId, ITransaction tx)
        {
            var ridesDict = await stateManager.GetOrAddAsync<IReliableDictionary<string, Ride>>(typeof(Ride).Name);

            var ridesEnumAsync = await ridesDict.CreateEnumerableAsync(tx);
            var ridesEnum = ridesEnumAsync.GetAsyncEnumerator();

            var rides = new List<Ride>();

            while (await ridesEnum.MoveNextAsync(default))
            {
                var ride = ridesEnum.Current;
                if (ride.Value != null)
                {
                    if (ride.Value.DriverId ==driverId)
                    {
                        rides.Add(ride.Value);
                    }
                }
            }

            return rides;
        }

        private async Task<IEnumerable<RideRating>> GetRatingsForRide(Guid rideId, ITransaction tx)
        {
            var ratingDict = await stateManager.GetOrAddAsync<IReliableDictionary<string, RideRating>>(typeof(RideRating).Name);
            var ratingEnumAsync = await ratingDict.CreateEnumerableAsync(tx);
            var ratingEnum = ratingEnumAsync.GetAsyncEnumerator();

            var ratings = new List<RideRating>();

            while (await ratingEnum.MoveNextAsync(default))
            {
                var rating = ratingEnum.Current;
                if (rating.Value != null)
                {
                    if (rating.Value.RideId == rideId)
                    {
                        ratings.Add(rating.Value);
                    }
                }
            }

            return ratings;
        }
    }
}
