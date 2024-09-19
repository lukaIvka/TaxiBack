using Models.Auth;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic.Implementations
{
    internal class AuthLogic : Contracts.Logic.IAuthLogic
    {
        private Contracts.Database.IData dbService;

        public AuthLogic(Contracts.Database.IData dbService) 
        {
            this.dbService = dbService;
        }

        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            return await dbService.GetUserProfile(id);
        }

        public async Task<LoginResponse> Login(LoginData loginData)
        {
            UserProfile existingUser = null;
            foreach (UserType type in Enum.GetValues(typeof(UserType)))
            {
                if (loginData.authType == AuthType.TRADITIONAL)
                {
                    existingUser = await dbService.ExistsWithPwd(loginData.Email, loginData.Password);
                }
                // Google Auth
                else
                {
                    existingUser = await dbService.ExistsOnlyEmail(loginData.Email);
                }

                if (existingUser != null)
                {
                    var res = new LoginResponse()
                    {
                        userId = (Guid)existingUser.Id,
                        userType = existingUser.Type
                    };
                    if (existingUser.Type == UserType.ADMIN)
                    {
                        res.roleId = ((Admin)existingUser).AdminId;  
                    }
                    else if (existingUser.Type == UserType.CLIENT)
                    {
                        res.roleId = ((Client)existingUser).ClientId;
                    }
                    else if (existingUser.Type == UserType.DRIVER)
                    {
                        res.roleId = ((Driver)existingUser).DriverId;
                    }
                    return res; 
                }
            }

            return null;
        }

        public async Task<bool> Register(UserProfile userProfile)
        {
            var userExists = false;
            foreach (UserType type in Enum.GetValues(typeof(UserType)))
            {
                userExists |= await dbService.ExistsOnlyEmail(userProfile.Email) != null;
            }

            if (userExists)
            {
                return false;
            }

            if (userProfile.Type == UserType.DRIVER)
            {
                var newDriver = new Models.UserTypes.Driver(userProfile, Models.UserTypes.DriverStatus.NOT_VERIFIED);
                newDriver.DriverId = Guid.NewGuid();
                return await dbService.CreateDriver(newDriver);
            }
            else if (userProfile.Type == UserType.CLIENT) 
            {
                var newClient = new Models.UserTypes.Client(userProfile);
                newClient.ClientId = Guid.NewGuid();
                return await dbService.CreateClient(newClient);
            }

            return await dbService.CreateUser(userProfile);
        }

        public async Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, Guid id)
        {
            return await dbService.UpdateUserProfile(updateUserProfileRequest, id);
        }
    }
}
