using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.ServiceFabric.Services.Remoting;
using Models.Auth;
using Models.UserTypes;
using Models.Ride;
using Models.Email;
using Contracts.Email;

namespace Contracts.Logic
{
    [ServiceContract]
    public interface IBussinesLogic : IAuthLogic, IDriverLogic, IRideLogic, IEmailService, IRatingLogic
    {
    }
}
