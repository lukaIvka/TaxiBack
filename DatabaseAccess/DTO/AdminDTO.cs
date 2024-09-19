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
    public class AdminDTO : IDTOConverter<AdminEntity, Admin>
    {
        public AdminEntity AppModelToSQL(Admin appModel)
        {
            return new AdminEntity()
            {
                Id = appModel.AdminId,
                UserId = (Guid)appModel.Id,
            };
        }

        public Admin SQLToAppModel(AdminEntity sqlModel)
        {
            return new Admin()
            {
                Id=sqlModel.UserId,
                Address=sqlModel.User.Address,
                AdminId = sqlModel.Id,
                DateOfBirth=sqlModel.User.DateOfBirth,
                Email=sqlModel.User.Email,
                Fullname=sqlModel.User.Fullname,
                ImagePath=sqlModel.User.ImagePath,
                Password=sqlModel.User.Password,
                Type = (Models.Auth.UserType)sqlModel.User.Type,
                Username=sqlModel.User.Username,
            };
        }
    }
}
