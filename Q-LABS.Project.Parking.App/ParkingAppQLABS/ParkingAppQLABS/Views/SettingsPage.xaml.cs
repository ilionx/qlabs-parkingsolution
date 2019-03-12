using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
         //public List<Car> Cars = new List<Car>() { new Car { Name = "Test Car 1", Rfid = "13 bn zz 19 rr p4" }, new Car { Name = "Test Car 2" } };

	    public string CarSetting
	    {
	        set
	        {
	            if (Settings.CarSettings == value)
	                return;
	            Settings.CarSettings = value;
	            OnPropertyChanged();
	        }
	        get => Settings.CarSettings;
	    }

	    public string RaspberrySetting
	    {
	        set
	        {
	            if (Settings.RaspberrySettings == value)
	                return;
	            Settings.RaspberrySettings = value;
	            OnPropertyChanged();
	        }
	        get => Settings.RaspberrySettings;
	    }

	    public bool DebugSetting
	    {
	        set
	        {
	            if (Settings.DebugMode == value)
	                return;
	            Settings.DebugMode = value;
	            OnPropertyChanged();
	        }
	        get => Settings.DebugMode;
	    }

	    public bool TestSetting
	    {
	        set
	        {
	            if (Settings.TestMode == value)
	                return;
	            Settings.TestMode = value;
	            OnPropertyChanged();
	        }
	        get => Settings.TestMode;
	    }

        public SettingsPage ()
		{
			InitializeComponent ();

		    IpEntry.Text = RaspberrySetting;
		    NfcEntry.Text = CarSetting;
		    DebugSwitch.IsToggled = DebugSetting;
		    TestSwitch.IsToggled = TestSetting;

		    //CarListView.ItemsSource = Cars;
		    //CarListView.SelectedItem = Cars.FirstOrDefault(x => x.Name == CarSetting);
		    //CarListView.ItemsSource.Where(x => x.Name == CarSetting).Selected = true;
		}

	    //private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	    //{
	    //    var item = e.SelectedItem as Car;
	    //    //LanguageListView.SelectedItem = null;
	    //    if (item == null)
	    //        return;

	        
	    //    //LanguageSetting = item.Title;
	    //    //await DisplayAlert("Succes", "Changed language to: " + LanguageSetting, "Ok");
	    //    //await Navigation.PopAsync();
	    //}

        async void OnClicked(object sender, EventArgs e)
	    {
	        var btn = sender as Button;
	        if (PwEntry.Text == Settings.SettingsPassword)
	        {
	            RaspberrySetting = IpEntry.Text;
	            CarSetting = NfcEntry.Text;
	            DebugSetting = DebugSwitch.IsToggled;
	            TestSetting = TestSwitch.IsToggled;
	            //CarSetting = ((Car)CarListView.SelectedItem)?.Name;

	            await DisplayAlert("Notification", "Changed user settings.", "OK");
	        }
	        else
	        {
	            await DisplayAlert("Notification", "Failed to change user settings.", "OK");
            }
	    }
    }

    //public class Car
    //{
    //    public string Name { get; set; }
    //    public string Rfid { get; set; }
    //}
}