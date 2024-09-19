using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Models.Auth;
using TaxiData.DataImplementations;
using Models.UserTypes;
using Models.Ride;
using TaxiData.DataServices;
using DatabaseAccess.CRUD;
using DatabaseAccess.Entities;
using DatabaseAccess.DTO;

namespace TaxiData
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class TaxiData : StatefulService, IData
    {
        private readonly DataServiceFactory dataServiceFactory;

        public TaxiData(
            StatefulServiceContext context
        )
            : base(context)
        {
            var userDto = new UserDTO();

            dataServiceFactory = new DataServiceFactory(
                StateManager,
                new CRUD<UserEntity, UserProfile>(
                    new UserDTO()
                ),
                new CRUD<ClientEntity, Client>(
                    new ClientDTO()
                    ),
                new CRUD<AdminEntity, Admin>(new AdminDTO()),
                new CRUD<DriverEntity, Driver>(new DriverDTO()),
                new CRUD<RideEntity, Ride>(new RideDTO()),
                new CRUD<RatingEntity, RideRating>(new RatingDTO())
            );
        }

        #region AuthMethods
        public async Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest request, Guid id)
        {
            return await dataServiceFactory.AuthDataService.UpdateUserProfile(request, id);
        }

        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            return await dataServiceFactory.AuthDataService.GetUserProfile(id);
        }

        public async Task<UserProfile> ExistsWithPwd(string email, string password)
        {
            return await dataServiceFactory.AuthDataService.ExistsWithPwd(email, password);
        }

        public async Task<UserProfile> ExistsOnlyEmail(string email)
        {
            return await dataServiceFactory.AuthDataService.ExistsOnlyEmail(email);
        }
        public async Task<bool> CreateUser(UserProfile appModel)
        {
            return await dataServiceFactory.AuthDataService.Create(appModel);
        }
        public async Task<bool> CreateDriver(Models.UserTypes.Driver appModel)
        {
            var userCreated = await dataServiceFactory.AuthDataService.Create(appModel);
            if (userCreated)
            {
                userCreated = await dataServiceFactory.DriverDataService.Create(appModel);
            }

            return userCreated;
        }

        public async Task<bool> CreateClient(Client client)
        {
            var userCreated = await dataServiceFactory.AuthDataService.Create(client);
            if (userCreated) 
            {
                userCreated = await dataServiceFactory.ClientDataService.CreateClient(client);
            }
            return userCreated;
        }

        #endregion

        #region SyncMethods

        private async Task SyncSQLTablesWithDict()
        {
            await dataServiceFactory.SyncSQLTablesWithDict();
        }

        private async Task RunPeriodicalUpdate(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                await SyncSQLTablesWithDict();

                await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
            }
        }

        private async Task SyncDictWithSQLTable()
        {
            await dataServiceFactory.SyncDictWithSQLTable();
        }

        #endregion

        #region ServiceFabricMethods
        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            await SyncDictWithSQLTable();

            int syncCnt = 0;

            while (true)
            {
               
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                if(syncCnt++ % 20 == 0)
                {
                    await SyncSQLTablesWithDict();
                    syncCnt = 0;
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

            }
        }

        #endregion

        #region DriverMethods
        public async Task<DriverStatus> GetDriverStatus(Guid id)
        {
            return await dataServiceFactory.DriverDataService.GetDriverStatus(id);
        }

        public async Task<bool> UpdateDriverStatus(Guid id, DriverStatus status)
        {
            return await dataServiceFactory.DriverDataService.UpdateDriverStatus(id, status);
        }

        public async Task<IEnumerable<Models.UserTypes.Driver>> ListAllDrivers()
        {
            return await dataServiceFactory.DriverDataService.ListAllDrivers();
        }

        #endregion

        #region RideMethods

        public async Task<Models.Ride.Ride> CreateRide(Models.Ride.Ride ride)
        {
            return await dataServiceFactory.RideDataService.CreateRide(ride);
        }

        public async Task<Models.Ride.Ride> UpdateRide(UpdateRideRequest updateRide, Guid driverId)
        {
            return await dataServiceFactory.RideDataService.UpdateRide(updateRide, driverId);
        }

        public async Task<IEnumerable<Models.Ride.Ride>> GetRides(QueryRideParams? queryParams)
        {
            return await dataServiceFactory.RideDataService.GetRides(queryParams);    
        }

        public async Task<Ride> GetRide(Guid id)
        {
            return await dataServiceFactory.RideDataService.GetRide(id);
        }
        #endregion

        #region DriverRatingMethods
        public async Task<Models.UserTypes.RideRating> RateDriver(Models.UserTypes.RideRating driverRating)
        {
            return await dataServiceFactory.DriverRatingDataService.RateDriver(driverRating);
        }

        public async Task<float> GetAverageRatingForDriver(Guid driverId)
        {
            return await dataServiceFactory.DriverDataService.GetAverageRatingForDriver(driverId);
        }


        #endregion
    }
}
