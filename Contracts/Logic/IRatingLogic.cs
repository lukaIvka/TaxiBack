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
    public interface IRatingLogic : IService
    {
        [OperationContract]
        Task<RideRating> RateDriver(RideRating driverRating);

        [OperationContract]
        Task<float> GetAverageRatingForDriver(Guid id);
    }
}
