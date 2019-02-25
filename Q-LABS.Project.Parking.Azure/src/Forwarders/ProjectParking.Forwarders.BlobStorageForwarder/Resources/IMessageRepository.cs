using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectParking.Contracts;

namespace ProjectParking.Forwarders.BlobStorageForwarder.Resources
{
    public interface IMessageRepository
    {
        Task<string> Store(IParkingSpotStatusUpdate update);
    }
}
