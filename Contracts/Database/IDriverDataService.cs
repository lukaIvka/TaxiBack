using Microsoft.ServiceFabric.Services.Remoting;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Database
{
    [ServiceContract]
    public interface IDriverDataService : IService
    {
        [OperationContract]
        Task<bool> CreateDriver(Driver appModel);

        [OperationContract]
        Task<DriverStatus> GetDriverStatus(Guid id);

        [OperationContract]
        Task<bool> UpdateDriverStatus(Guid id, DriverStatus status);

        [OperationContract]
        Task<IEnumerable<Driver>> ListAllDrivers();

        [OperationContract]
        Task<float> GetAverageRatingForDriver(Guid driverId);
    }
}
