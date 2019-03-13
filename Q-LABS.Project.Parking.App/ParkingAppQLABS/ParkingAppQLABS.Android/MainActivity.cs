using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using ParkingAppQLABS.Droid.Services;
using ParkingAppQLABS.Messages;
using Xamarin.Forms;

namespace ParkingAppQLABS.Droid
{
    [Activity(Label = "Project P", Icon = "@drawable/ParkingP", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            WireUpBackgroundTask();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        void WireUpBackgroundTask()
        {
            MessagingCenter.Subscribe<StartTaskMessage>(this, "StartTaskMessage", message => {
                var intent = new Intent(this, typeof(DroidBackgroundService));
                StartService(intent);
            });

            MessagingCenter.Subscribe<StopTaskMessage>(this, "StopTaskMessage", message => {
                var intent = new Intent(this, typeof(DroidBackgroundService));
                StopService(intent);
            });
        }
    }
}

