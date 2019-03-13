using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using ParkingAppQLABS.iOS.Services;
using ParkingAppQLABS.Messages;
using UIKit;
using Xamarin.Forms;

namespace ParkingAppQLABS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static Action BackgroundSessionCompletionHandler;

        public override void HandleEventsForBackgroundUrl(UIApplication application, string sessionIdentifier, Action completionHandler)
        {
            Console.WriteLine("HandleEventsForBackgroundUrl(): " + sessionIdentifier);
            // We get a completion handler which we are supposed to call if our transfer is done.
            BackgroundSessionCompletionHandler = completionHandler;
        }
        #region Methods
        iOSBackgroundService longRunningTaskExample;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            WireUpLongRunningTask();

            return base.FinishedLaunching(app, options);
        }

        void WireUpLongRunningTask()
        {
            MessagingCenter.Subscribe<StartTaskMessage>(this, "StartTaskMessage", async message => {
                longRunningTaskExample = new iOSBackgroundService();
                await longRunningTaskExample.Start();
            });

            MessagingCenter.Subscribe<StopTaskMessage>(this, "StopTaskMessage", message => {
                longRunningTaskExample.Stop();
            });
        }
        #endregion
    }
}
