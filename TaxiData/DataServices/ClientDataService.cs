using Contracts.Database;
using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class ClientDataService : BaseDataService<Models.UserTypes.Client, ClientEntity>, IClientDataService
    {
        public ClientDataService(Synchronizer<ClientEntity, Client> synchronizer, IReliableStateManager stateManager) : base(synchronizer, stateManager)
        {}

        public async Task<bool> CreateClient(Client client)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var dictKey = $"{client.ClientId}";
            var created = await dict.AddOrUpdateAsync(txWrapper.transaction, dictKey, client, (key, value) => value);
            return created != null;
        }
    }
}
