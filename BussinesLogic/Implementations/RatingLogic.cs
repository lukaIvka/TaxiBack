using Models.Auth;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic.Implementations
{
    internal class RatingLogic : Contracts.Logic.IRatingLogic
    {
        private Contracts.Database.IData dbService;
        private Contracts.Logic.IRideLogic rideLogic;
        public RatingLogic(Contracts.Database.IData dbService, Contracts.Logic.IRideLogic rideLogic) 
        {
            this.dbService = dbService;
            this.rideLogic = rideLogic;
        }    
        public async Task<float> GetAverageRatingForDriver(Guid id)
        {
            return await dbService.GetAverageRatingForDriver(id);
        }

        public async Task<RideRating> RateDriver(RideRating driverRating)
        {
            return await dbService.RateDriver(driverRating);
        }
    }
}
