using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Database;
using Contracts.Email;
using Contracts.Logic;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models.Auth;
using Models.Email;
using Models.Ride;
using Models.UserTypes;

namespace BussinesLogic
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class BussinesLogic : StatelessService, IBussinesLogic
    {
        private IEmailService emailService;

        private IAuthLogic authLogic;
        private IDriverLogic driverLogic;
        private IRideLogic rideLogic;
        private IRatingLogic ratingLogic;
        public BussinesLogic(
            StatelessServiceContext context, 
            IEmailService emailService,
            IAuthLogic authLogic,
            IDriverLogic driverLogic,
            IRideLogic rideLogic,
            IRatingLogic ratingLogic
            )
            : base(context)
        {
            this.emailService = emailService;
            this.authLogic = authLogic;
            this.driverLogic = driverLogic;
            this.rideLogic = rideLogic;
            this.ratingLogic = ratingLogic;
        }

        #region DriverMethods

        public async Task<DriverStatus> GetDriverStatus(Guid id)
        {
            return await driverLogic.GetDriverStatus(id);
        }

        public async Task<bool> UpdateDriverStatus(Guid id, DriverStatus status)
        {
            return await driverLogic.UpdateDriverStatus(id, status);
        }

        public async Task<IEnumerable<Driver>> ListAllDrivers()
        {
            return await driverLogic.ListAllDrivers();
        }

        #endregion

        #region AuthMethods

        public async Task<LoginResponse> Login(LoginData loginData)
        {
            return await authLogic.Login(loginData);
        }

        public async Task<bool> Register(UserProfile userProfile)
        {
            return await authLogic.Register(userProfile);
        }

        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            return await authLogic.GetUserProfile(id);
        }

        public async Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, Guid id)
        {
            return await authLogic.UpdateUserProfile(updateUserProfileRequest, id);
        }

        #endregion

        #region ServiceFabricMethods
        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        #endregion

        #region RideMethods

        public async Task<EstimateRideResponse> EstimateRide(EstimateRideRequest request)
        {
            return await rideLogic.EstimateRide(request);
        }

        public async Task<Ride> CreateRide(CreateRideRequest request, Guid clientId)
        {
            return await rideLogic.CreateRide(request, clientId);
        }

        public async Task<Ride> UpdateRide(UpdateRideRequest request, Guid driverId)
        {
            return await rideLogic.UpdateRide(request, driverId);
        }

        public async Task<IEnumerable<Ride>> GetNewRides()
        {
            return await rideLogic.GetNewRides();
        }

        public async Task<IEnumerable<Ride>> GetUsersRides(Guid userId, UserType userType)
        {
            return await rideLogic.GetUsersRides(userId, userType);
        }

        public async Task<IEnumerable<Ride>> GetAllRides()
        {
            return await rideLogic.GetAllRides();
        }

        public async Task<Ride> GetRideStatus(Guid rideId)
        {
            return await rideLogic.GetRideStatus(rideId);
        }
        #endregion

        #region EmailMethods

        public async Task<bool> SendEmail(SendEmailRequest sendEmailRequest)
        {
            return await emailService.SendEmail(sendEmailRequest);
        }


        #endregion

        #region DriverRatingMethods
        public async Task<RideRating> RateDriver(RideRating driverRating)
        {
            return await ratingLogic.RateDriver(driverRating);
        }

        public async Task<float> GetAverageRatingForDriver(Guid driverId)
        {
            return await ratingLogic.GetAverageRatingForDriver(driverId);
        }

        #endregion
    }
}
