using Contracts.SQLDB;
using DatabaseAccess.Entities;
using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.DTO
{
    public class UserDTO : IDTOConverter<UserEntity, UserProfile>
    {
        public UserEntity AppModelToSQL(UserProfile appModel)
        {
            return new UserEntity()
            {
                Address = appModel.Address,
                DateOfBirth = appModel.DateOfBirth,
                Email = appModel.Email,
                Fullname = appModel.Fullname,
                Id = (Guid)appModel.Id,
                ImagePath = appModel.ImagePath,
                Password = appModel.Password,
                Type = (int)appModel.Type,
                Username = appModel.Username,
            };
        }

        public UserProfile SQLToAppModel(UserEntity sqlModel)
        {
            return new UserProfile()
            {
                Username = sqlModel.Username,
                Type = (UserType)sqlModel.Type,
                Password = sqlModel.Password,
                ImagePath = sqlModel.ImagePath,
                Id = sqlModel.Id,
                Fullname = sqlModel.Fullname,
                Address = sqlModel.Address,
                DateOfBirth = sqlModel.DateOfBirth,
                Email = sqlModel.Email,
            };
        }
    }
}
