using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Email
{
    [ServiceContract]
    public interface IEmailService : IService
    {
        [OperationContract]
        Task<bool> SendEmail(Models.Email.SendEmailRequest sendEmailRequest);
    }
}
