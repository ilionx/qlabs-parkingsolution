using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ParkingAppQLABS.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ParkingAppQLABS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPageParkingMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetailPageParkingMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetailPageParkingMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetailPageParkingMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPageMenuItem> MenuItems { get; set; }
            
            public MasterDetailPageParkingMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailPageMenuItem>(new[]
                {
                    new MasterDetailPageMenuItem { Id = 0, Title = "Home", TargetType = typeof(StartPage)} ,
                    new MasterDetailPageMenuItem { Id = 1, Title = "Settings", TargetType = typeof(SettingsPage)},
                    new MasterDetailPageMenuItem { Id = 2, Title = "About", TargetType = typeof(AboutPage)}
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}