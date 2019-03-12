using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            Title = "\tHome";
            BackgroundImage = "BG.png";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //stops the background process
            var message = new StopTaskMessage();
            MessagingCenter.Send(message, "StopTaskMessage");
    }

        private void StartClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OverviewPage());
            //    var page = (Page)Activator.CreateInstance(typeof(OverviewPage));
            //    page.Title = "test";
            //    var test = ()Parent;
            //        test.Detail = new NavigationPage(page);
        }
    }
}