using Microsoft.ServiceFabric.Services.Remoting;
using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Models.UserTypes;
using Models.Ride;

namespace Contracts.Database
{
    [ServiceContract]
    public interface IData : IAuthDataService, IDriverDataService, IRideDataService, IRatingDataService, IClientDataService
    {}
}
