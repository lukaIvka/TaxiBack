using Contracts.SQLDB;
using DatabaseAccess.Entities;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.DTO
{
    public class ClientDTO : IDTOConverter<ClientEntity, Client>
    {
        public ClientEntity AppModelToSQL(Client appModel)
        {
            return new ClientEntity()
            {
                Id = appModel.ClientId,
                UserId = (Guid)appModel.Id,
            };
        }

        public Client SQLToAppModel(ClientEntity sqlModel)
        {
            return new Client()
            {
                Username = sqlModel.User.Username,
                Type = (Models.Auth.UserType)sqlModel.User.Type,
                Password = sqlModel.User.Password,
                ImagePath = sqlModel.User.ImagePath,
                Id = sqlModel.UserId,
                ClientId = sqlModel.Id,
                Fullname = sqlModel.User.Fullname,
                Email = sqlModel.User.Email,
                DateOfBirth = sqlModel.User.DateOfBirth,
                Address = sqlModel.User.Address,
            };
        }
    }
}
