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
    public class RatingDTO : IDTOConverter<RatingEntity, RideRating>
    {
        public RatingEntity AppModelToSQL(RideRating appModel)
        {
            return new RatingEntity()
            {
                Id = appModel.Id,
                RideId = appModel.RideId,
                Value = appModel.Value,
            };
        }

        public RideRating SQLToAppModel(RatingEntity sqlModel)
        {
            return new RideRating()
            {
                Value = sqlModel.Value,
                RideId = sqlModel.RideId,
                Id = sqlModel.Id,
            };
        }
    }
}
