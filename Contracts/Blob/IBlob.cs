using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Blob
{
    public interface IBlob
    {
        Task<string> UploadBlob(string blobName, Stream blobContent);

        Task<string> GenerateSasUri(string blobName);
    }

}
