using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Data;
using ParkingAppQLABS.Helpers;
using ParkingAppQLABS.Messages;
using ParkingAppQLABS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OverviewPage : ContentPage
	{
	    private LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO _carparks;
	    private string _currentMessage;
	    private bool _refreshData;

        public OverviewPage ()
	   {
	       InitializeComponent ();

	       //send start message to background task
	       var message = new StartTaskMessage();
	       MessagingCenter.Send(message, "StartTaskMessage");

	       //subscribe to the message center (required for background processing)
	       HandleReceivedMessages();
        }

	   protected override async void OnAppearing()
	   {
	       base.OnAppearing();

            //Display loading screen
            var loading = !Settings.DebugMode; //if debug mode is enabled the loading screen will be skipped
            while (loading)
            {
                LoadingScreen.IsVisible = true;
                await LoadingAnimation();
                if (_currentMessage != null)
                {
                    if (!_currentMessage.Contains("Failed"))
                    {
                        loading = false;
                    }
                }
            }

            //refresh data and hide loading screen
            await RefreshData();
            LoadingScreen.IsVisible = false;

	       //refresh the page every 10 seconds.
	       _refreshData = true;
	       Device.StartTimer(new TimeSpan(0, 0, 10), () =>
	       {
	           Task.Run(async () => await RefreshData());
	           return _refreshData;
	       });
        }

	    protected override void OnDisappearing()
	    {
             //stops the page refresh
	        _refreshData = false;
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage",
                message => { Device.BeginInvokeOnMainThread(() => { _currentMessage = message.Message; }); });

            MessagingCenter.Subscribe<CancelMessage>(this, "CancelledMessage",
                message => { Device.BeginInvokeOnMainThread(() => { _currentMessage = "Cancelled"; }); });
        }

        public void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
	        if (e.SelectedItem != null)
	        {
                 //deselect selection
	            CarparksListView.SelectedItem = null;
	        }
	    }

         //Displays a loading animation
	    private async Task LoadingAnimation()
	    {
	        LoadingIndicator.Source = "Range0.png";
	        await Task.Delay(1000);
	        LoadingIndicator.Source = "Range1.png";
	        await Task.Delay(1000);
	        LoadingIndicator.Source = "Range2.png";
	        await Task.Delay(1000);
	        LoadingIndicator.Source = "Range3.png";
	        await Task.Delay(1000);
        }

         //Refreshes the carpark data
	    private async Task RefreshData()
	    {
            Device.BeginInvokeOnMainThread(async () =>
            {
                RestService restservice = new RestService();
	            _carparks = await restservice.GetCarParksAsync();
	            if (_carparks.value != null)
	            {
	                //Determine background color (+ name)
	                foreach (var carpark in _carparks.value)
	                {
	                    switch (carpark.value.availableParkingSpots)
	                    {
	                        case int n when (n == 0):
	                            carpark.value.color = Color.FromHex("#CCCCCC");
	                            break;
	                        case int n when (n == 1):
	                            carpark.value.color = Color.FromHex("#F26D44");
	                            break;
	                        case int n when (n >= 2 && n <= 4):
	                            carpark.value.color = Color.FromHex("#EFBB33");
	                            break;
	                        case int n when (n >= 5):
	                            carpark.value.color = Color.FromHex("#69D180");
	                            break;
	                        default:
	                            carpark.value.color = Color.FromHex("#CCCCCC");
	                            break;
	                    }

                         //currently hardcoded, because the names are too long.
	                    switch (carpark.value.name)
	                    {
                            case string n when (n.Contains("QNH")):
                                carpark.value.name = "QNH";
                                break;
	                        case string n when (n.Contains("carconnect")):
	                            carpark.value.name = "Carconnect";
	                            break;
	                        case string n when (n.Contains("Race Art")):
	                            carpark.value.name = "Race Art";
	                            break;
	                        case string n when (n.Contains("ENGIE")):
	                            carpark.value.name = "Engie";
	                            break;
                        }
	                }

	                CarparksListView.ItemsSource = null;
	                CarparksListView.ItemsSource = _carparks.value;
	                ErrorLabel.Text = _carparks.value.Count == 0 ? "Failed to obtain parking information." : null;
	            }
	            else
	            {
	                ErrorLabel.Text = "Failed to obtain parking information.";
	            }
            });
	    }
    }
}