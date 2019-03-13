using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Data;
using ParkingAppQLABS.Messages;
using ParkingAppQLABS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParkingHomePage : ContentPage
    {
        private LinkInfoWrapper_List_LinkInfoWrapper_CarparkListDTO _carparks;

        public ParkingHomePage()
        {
            InitializeComponent();

            Title = "Home";

            //list parking spots
            CarparksListView.RefreshCommand = new Command(async () =>
            {
                await RefreshData();

                CarparksListView.IsRefreshing = false;
            });

            start.Clicked += (s, e) =>
            {
                infoLabel.Text = "Determining location...";
                var message = new StartTaskMessage();
                MessagingCenter.Send(message, "StartTaskMessage");
            };

            stop.Clicked += (s, e) =>
            {
                var message = new StopTaskMessage();
                MessagingCenter.Send(message, "StopTaskMessage");
            };

            HandleReceivedMessages();
        }

        //list parking spots
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            LoadingIndicator.IsVisible = true;
            await RefreshData();
            LoadingIndicator.IsVisible = false;
        }

        void HandleReceivedMessages()
        {
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage",
                message => { Device.BeginInvokeOnMainThread(() => { infoLabel.Text = message.Message; }); });

            MessagingCenter.Subscribe<CancelMessage>(this, "CancelledMessage",
                message => { Device.BeginInvokeOnMainThread(() => { infoLabel.Text = "Cancelled"; }); });
        }

        //public async void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        public void OnListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                CarparksListView.SelectedItem = null;
                //var carpark = e.SelectedItem as Carpark;
                //if (carpark == null) return;
                //var parkPage = new ParkingDetailPage(carpark);
                //await Navigation.PushAsync(parkPage);
            }
        }

        private async Task RefreshData()
        {
            RestService restservice = new RestService();
            _carparks = await restservice.GetCarParksAsync();
            CarparksListView.ItemsSource = _carparks.value;
            ErrorLabel.Text = _carparks.value.Count == 0 ? "Failed to obtain parking information." : null;
        }
    }
}