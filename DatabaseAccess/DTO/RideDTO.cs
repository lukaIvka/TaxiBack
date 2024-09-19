using Contracts.SQLDB;
using DatabaseAccess.Entities;
using Models.Ride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.DTO
{
    public class RideDTO : IDTOConverter<RideEntity, Ride>
    {
        public RideEntity AppModelToSQL(Ride appModel)
        {
            return new RideEntity()
            {
                ClientId = appModel.ClientId,
                CreatedAtTimestamp = appModel.CreatedAtTimestamp,
                DriverId = appModel.DriverId,
                StartAddress = appModel.StartAddress,
                EndAddress = appModel.EndAddress,
                EstimatedDriverArrival = appModel.EstimatedDriverArrival,
                EstimatedRideEnd = appModel.EstimatedRideEnd,
                Price = appModel.Price,
                Status = (int)appModel.Status,
                Id = appModel.Id,
            };
        }

        public Ride SQLToAppModel(RideEntity sqlModel)
        {
            return new Ride()
            {
                Id = sqlModel.Id,
                StartAddress = sqlModel.StartAddress,
                EndAddress = sqlModel.EndAddress,
                ClientId = sqlModel.ClientId,
                CreatedAtTimestamp = sqlModel.CreatedAtTimestamp,
                DriverId = sqlModel.DriverId,
                EstimatedDriverArrival = sqlModel.EstimatedDriverArrival,
                EstimatedRideEnd = sqlModel.EstimatedRideEnd,
                Price = sqlModel.Price,
                Status = (RideStatus)sqlModel.Status
            };
        }
    }
}
