using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;
using ParkingAppQLABS.Messages;
using ParkingAppQLABS.Services;

namespace ParkingAppQLABS.iOS.Services
{
    public class iOSBackgroundService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public async Task Start()
        {
            _cts = new CancellationTokenSource();

            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("LongRunningTask", OnExpiration);

            try
            {
                //INVOKE THE SHARED CODE
                var backgroundTask = new BackgroundService();
                await backgroundTask.RunBackgroundTask(_cts.Token);

            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    var message = new CancelMessage();
                    Device.BeginInvokeOnMainThread(
                        () => MessagingCenter.Send(message, "CancelledMessage")
                    );
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}