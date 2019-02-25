using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectParking.Contracts;
using ProjectParking.Processors.HeartbeatProcessor.Resources;

namespace ProjectParking.Processors.HeartbeatProcessor
{
    internal class HeartbeatProcessor
    {
        private readonly IHeartbeatRepository _repository;
        private readonly INotificationAccess _notificationAccess;
        private int _msBetweenChecks = 15000;

        public HeartbeatProcessor(IHeartbeatRepository repository, INotificationAccess notificationAccess)
        {
            _repository = repository;
            _notificationAccess = notificationAccess;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var spots = await _repository.RetrieveUnresponsiveParkingSpots();

                if (spots.Length > 0)
                {
                    await _repository.MarkUnresponsiveSpots(spots);
                    await _notificationAccess.SendNotificationFor(spots);
                }

                await Task.Delay(_msBetweenChecks, cancellationToken);
            }
        }

        public async Task Process(IParkingSpotStatusUpdate update)
        {
            await _repository.Store(update);
        }
    }
}