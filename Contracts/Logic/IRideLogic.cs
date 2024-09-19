using Microsoft.ServiceFabric.Services.Remoting;
using Models.Auth;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Logic
{
    [ServiceContract]
    public interface IRideLogic : IService
    {
        [OperationContract]
        Task<EstimateRideResponse> EstimateRide(EstimateRideRequest request);

        [OperationContract]
        Task<Ride> CreateRide(CreateRideRequest request, Guid clientId);

        [OperationContract]
        Task<Ride> UpdateRide(UpdateRideRequest request, Guid driverId);

        [OperationContract]
        Task<IEnumerable<Ride>> GetNewRides();

        [OperationContract]
        Task<IEnumerable<Ride>> GetUsersRides(Guid userId, UserType userType);

        [OperationContract]
        Task<IEnumerable<Ride>> GetAllRides();

        [OperationContract]
        Task<Ride> GetRideStatus(Guid id);
    }
}
