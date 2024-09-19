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
    public class DriverDTO : IDTOConverter<DriverEntity, Driver>
    {
        public DriverEntity AppModelToSQL(Driver appModel)
        {
            return new DriverEntity()
            {
                Id = appModel.DriverId,
                UserId = (Guid)appModel.Id,
                Status = (int)appModel.Status,
            };
        }

        public Driver SQLToAppModel(DriverEntity sqlModel)
        {
            return new Driver()
            {
                Username = sqlModel.User.Username,
                Type = (Models.Auth.UserType)sqlModel.User.Type,
                Password = sqlModel.User.Password,
                ImagePath = sqlModel.User.ImagePath,
                Id = sqlModel.UserId,
                DriverId = sqlModel.Id,
                Fullname = sqlModel.User.Fullname,
                Email = sqlModel.User.Email,
                DateOfBirth = sqlModel.User.DateOfBirth,
                Address = sqlModel.User.Address,
                Status = (DriverStatus)sqlModel.Status,
            };
        }
    }
}
