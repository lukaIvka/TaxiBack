using Microsoft.ServiceFabric.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiData
{
    internal class StateManagerTransactionWrapper : IDisposable
    {
        public Microsoft.ServiceFabric.Data.ITransaction transaction { get; }

        public StateManagerTransactionWrapper(ITransaction transaction)
        {
            this.transaction = transaction;
        }

        public async void Dispose()
        {
            await transaction.CommitAsync();
            transaction.Dispose();
        }
    }
}
