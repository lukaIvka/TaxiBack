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
    public interface IRatingDataService : IService
    {
        [OperationContract]
        Task<RideRating> RateDriver(RideRating driverRating);

    }
}
