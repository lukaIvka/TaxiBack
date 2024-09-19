
using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class RideDataService : BaseDataService<Models.Ride.Ride, RideEntity>, Contracts.Database.IRideDataService
    {
        public RideDataService(
            Synchronizer<RideEntity, Models.Ride.Ride> synchronizer, 
            IReliableStateManager stateManager) : 
            base(synchronizer, stateManager)
        {}

        public async Task<Models.Ride.Ride> CreateRide(Models.Ride.Ride ride)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var dictKey = $"{ride.Id}";
            var createdRide = await dict.AddOrUpdateAsync(txWrapper.transaction, dictKey, ride, (key, value) => value);
            return createdRide;
        }

        public async Task<Models.Ride.Ride> UpdateRide(UpdateRideRequest updateRide, Guid driverId)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var rideKey = $"{updateRide.RideId}";

            var existing = await dict.TryGetValueAsync(txWrapper.transaction, rideKey);

            if (!existing.HasValue)
            {
                return null;
            }

            existing.Value.Status = updateRide.Status;

            if (updateRide.Status == RideStatus.ACCEPTED && updateRide is UpdateRideWithTimeEstimate updateRideWithTime)
            {
                existing.Value.DriverId = driverId;
                existing.Value.EstimatedRideEnd = existing.Value.EstimatedDriverArrival.AddSeconds(updateRideWithTime.RideEstimateSeconds);
            }

            var res = await dict.AddOrUpdateAsync(txWrapper.transaction, rideKey, existing.Value, (key, value) => value);

            return res;
        }
        public async Task<IEnumerable<Models.Ride.Ride>> GetRides(QueryRideParams? queryParams)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var collectionEnum = await dict.CreateEnumerableAsync(txWrapper.transaction);
            var asyncEnum = collectionEnum.GetAsyncEnumerator();
            var rides = new List<Models.Ride.Ride>();

            while (await asyncEnum.MoveNextAsync(default))
            {
                var rideEntity = asyncEnum.Current.Value;
                if (rideEntity != null)
                {
                    if (queryParams != null)
                    {
                        bool shouldAdd = (queryParams.Status == null) || (queryParams.Status == rideEntity.Status);
                        shouldAdd &= (queryParams.ClientId == null) || (queryParams.ClientId == rideEntity.ClientId);
                        shouldAdd &= (queryParams.DriverId == null) || (queryParams.DriverId == rideEntity.DriverId);
                        if (shouldAdd)
                        {
                            rides.Add(rideEntity);
                        }
                    }
                    else
                    {
                        rides.Add(rideEntity);
                    }
                }
            }

            return rides;
        }

        public async Task<Ride> GetRide(Guid id)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());

            var existingRide = await dict.TryGetValueAsync(txWrapper.transaction, $"{id}");

            if (!existingRide.HasValue)
            {
                return null;
            }

            return existingRide.Value;
        }
    }
}
