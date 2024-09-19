using Microsoft.ServiceFabric.Services.Remoting;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Logic
{
    [ServiceContract]
    public interface IDriverLogic : IService
    {
        [OperationContract]
        Task<DriverStatus> GetDriverStatus(Guid id);

        [OperationContract]
        Task<bool> UpdateDriverStatus(Guid id, DriverStatus status);

        [OperationContract]
        Task<IEnumerable<Driver>> ListAllDrivers();
    }
}
