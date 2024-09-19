using Contracts.SQLDB;
using DatabaseAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.CRUD
{
    public class CRUD<TDB, TM> : ISQLCRUD<TDB, TM> where TDB : class, BaseEntity
    {
        private readonly IDTOConverter<TDB, TM> converter;

        public CRUD(IDTOConverter<TDB, TM> converter)
        {
            this.converter = converter;
        }

        public async Task AddOrUpdateMultipleEntities(IEnumerable<TDB> entities)
        {
            using var dbCtx = DBContextFactory.Instance.GetDBContext();

            foreach (var entity in entities)
            {
                if (dbCtx.Set<TDB>().Any(e => e.Id == entity.Id))
                {
                    dbCtx.Set<TDB>().Update(entity);
                }
                else
                {
                    dbCtx.Set<TDB>().Add(entity);
                }
            }

            try
            {
                await dbCtx.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }

        }

        public IEnumerable<TM> GetAllEntities()
        {
            using var dbCtx = DBContextFactory.Instance.GetDBContext();
            var entities = dbCtx.Set<TDB>().ToList();
            var converted = entities.Select(e => converter.SQLToAppModel(e)).ToList();
            return converted;
        }
    }
}
