using Contracts.SQLDB;
using DatabaseAccess.DTO;
using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Models;
using Models.Auth;
using Models.Ride;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiData.DataServices
{
    internal class DataServiceFactory
    {
        public AuthDataService AuthDataService { get; private set; }
        public DriverDataService DriverDataService { get; private set; }
        public AdminDataService AdminDataService { get; private set; }
        public ClientDataService ClientDataService { get; private set; }
        public RideDataService RideDataService { get; private set; }
        public RatingDataService DriverRatingDataService { get; private set; }

        public DataServiceFactory(
            IReliableStateManager stateManager,
            ISQLCRUD<UserEntity, UserProfile> userSql,
            ISQLCRUD<ClientEntity, Client> clientSql,
            ISQLCRUD<AdminEntity, Admin> adminSql,
            ISQLCRUD<DriverEntity, Driver> driverSql,
            ISQLCRUD<RideEntity, Ride> rideSql,
            ISQLCRUD<RatingEntity, RideRating> ratingSql
        ) 
        {
            AuthDataService = new AuthDataService(
                new DataImplementations.Synchronizer<UserEntity, Models.Auth.UserProfile>(
                    userSql, 
                    typeof(UserProfile).Name, 
                    new UserDTO(), 
                    stateManager
                ),
                stateManager
            );
            DriverDataService = new DriverDataService(
                new DataImplementations.Synchronizer<DriverEntity, Models.UserTypes.Driver>(
                    driverSql,
                    typeof(Models.UserTypes.Driver).Name,
                    new DriverDTO(),
                    stateManager
                ),
                stateManager
            );
            AdminDataService = new AdminDataService(
                new DataImplementations.Synchronizer<AdminEntity, Models.UserTypes.Admin>(
                    adminSql,
                    typeof(Models.UserTypes.Admin).Name,
                    new AdminDTO(),
                    stateManager
                    ),
                stateManager
            );
            ClientDataService = new ClientDataService(
                new DataImplementations.Synchronizer<ClientEntity, Models.UserTypes.Client>(
                    clientSql, typeof(Models.UserTypes.Client).Name, new ClientDTO(), stateManager
                    ),
                stateManager
            );

            RideDataService = new RideDataService(
                new DataImplementations.Synchronizer<RideEntity, Models.Ride.Ride>(
                    rideSql,
                    typeof(Models.Ride.Ride).Name,
                    new RideDTO(),
                    stateManager
                ),
                stateManager
            );

            DriverRatingDataService = new RatingDataService(
                new DataImplementations.Synchronizer<RatingEntity, Models.UserTypes.RideRating>(
                    ratingSql,
                    typeof(Models.UserTypes.RideRating).Name,
                    new RatingDTO(),
                    stateManager
                ),
                stateManager
            );
        }

        public async Task SyncSQLTablesWithDict()
        {
            await AuthDataService.SyncSQLTablesWithDict();
            await DriverDataService.SyncSQLTablesWithDict();
            await AdminDataService.SyncSQLTablesWithDict();
            await ClientDataService.SyncSQLTablesWithDict();
            await RideDataService.SyncSQLTablesWithDict();
            await DriverRatingDataService.SyncSQLTablesWithDict();
        }
        public async Task SyncDictWithSQLTable()
        {
            await AuthDataService.SyncDictWithSQLTable();
            await DriverDataService.SyncDictWithSQLTable();
            await AdminDataService.SyncDictWithSQLTable();
            await ClientDataService.SyncDictWithSQLTable();
            await RideDataService.SyncDictWithSQLTable();
            await DriverRatingDataService.SyncDictWithSQLTable();
        }
    }
}
