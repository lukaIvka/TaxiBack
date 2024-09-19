using Microsoft.ServiceFabric.Services.Remoting;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Database
{
    [ServiceContract]
    public interface IRideDataService : IService
    {
        [OperationContract]
        Task<Models.Ride.Ride> CreateRide(Models.Ride.Ride ride);

        [OperationContract]
        Task<Models.Ride.Ride> UpdateRide(Models.Ride.UpdateRideRequest updateRide, Guid driverId);

        [OperationContract]
        Task<IEnumerable<Models.Ride.Ride>> GetRides(Models.Ride.QueryRideParams? queryParams);

        [OperationContract]
        Task<Ride> GetRide(Guid id);
    }
}
