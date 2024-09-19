using Models.Auth;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic.Implementations
{
    internal class RideLogic : Contracts.Logic.IRideLogic
    {
        private Contracts.Database.IData dbService;
        public RideLogic(Contracts.Database.IData dbService) 
        {
            this.dbService = dbService;
        }

        public async Task<Ride> CreateRide(CreateRideRequest request, Guid clientId)
        {
            var now = DateTime.UtcNow;
            now = DateTime.SpecifyKind(now, DateTimeKind.Utc);
            var unixTimestamp = new DateTimeOffset(now).ToUnixTimeMilliseconds();

            var newRide = new Models.Ride.Ride()
            {
                Id = request.Id,
                ClientId = clientId,
                CreatedAtTimestamp = unixTimestamp,
                DriverId = null,
                EndAddress = request.EndAddress,
                StartAddress = request.StartAddress,
                Price = request.Price,
                Status = RideStatus.CREATED,
                EstimatedDriverArrival = now.AddSeconds(request.EstimatedDriverArrivalSeconds),
                EstimatedRideEnd = null
            };

            return await dbService.CreateRide(newRide);
        }

        public Task<EstimateRideResponse> EstimateRide(EstimateRideRequest request)
        {
            var randomGen = new Random();

            return Task.FromResult(
                new EstimateRideResponse()
                {
                    PriceEstimate = randomGen.NextSingle() * 1000,
                    EstimatedDriverArrivalSeconds = randomGen.Next(60) + 60 // Max 1 hour
                });
        }

        public async Task<IEnumerable<Ride>> GetAllRides()
        {
            return await dbService.GetRides(default);
        }

        public async Task<IEnumerable<Ride>> GetNewRides()
        {
            return await dbService.GetRides(new QueryRideParams()
            {
                Status = RideStatus.CREATED
            });
        }

        public async Task<Ride> GetRideStatus(Guid id)
        {
            return await dbService.GetRide(id);
        }

        public async Task<IEnumerable<Ride>> GetUsersRides(Guid userId, UserType userType)
        {
            switch (userType)
            {
                case UserType.CLIENT:
                    {
                        var clientRides = new List<Ride>();
                        foreach (RideStatus status in Enum.GetValues(typeof(RideStatus)))
                        {
                            var rides = await dbService.GetRides(new QueryRideParams()
                            {
                                ClientId = userId,
                                Status = status
                            });
                            clientRides.AddRange(rides);
                        }
                        return clientRides;
                    }
                case UserType.DRIVER:
                    return await dbService.GetRides(new QueryRideParams()
                    {
                        DriverId = userId,
                        Status = RideStatus.COMPLETED
                    });
                case UserType.ADMIN:
                default:
                    return await GetAllRides();
            }
        }

        public async Task<Ride> UpdateRide(UpdateRideRequest request, Guid driverId)
        {
            // Driver accepted the ride
            if (request.Status == RideStatus.ACCEPTED)
            {
                var randomGen = new Random();

                var rideWithTimeEstimate = new Models.Ride.UpdateRideWithTimeEstimate()
                {
                    RideId = request.RideId,
                    Status = request.Status,
                    RideEstimateSeconds = randomGen.Next(60)
                };

                return await dbService.UpdateRide(rideWithTimeEstimate, driverId);
            }

            return await dbService.UpdateRide(request, driverId);
        }
    }
}
