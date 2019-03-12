using System.Collections;
using ParkingAppQLABS.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ParkingAppQLABS.Data;
using ParkingAppQLABS.Helpers;
using Xamarin.Forms;
using Plugin.Notifications;
using Plugin.TextToSpeech;
using Plugin.TextToSpeech.Abstractions;

namespace ParkingAppQLABS.Services
{
    public class BackgroundService
    {
        private bool? _entered;

        public async Task RunBackgroundTask(CancellationToken token)
        {

            //var locales = await CrossTextToSpeech.Current.GetInstalledLanguages();
            string entry = "Unknown";

            await Task.Run(async () =>
            {
                for (long i = 0; i < long.MaxValue; i++)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(100); //100ms
                    
                    switch (_entered)
                    {
                        case true:
                            entry = "Entered";
                            break;
                        case false:
                            entry = "Exited";
                            break;
                    }

                    var message = new TickedMessage
                    {
                        Message = "Current status: " + entry
                    };
                    if (_entered == null)
                    {
                        message = new TickedMessage
                        {
                            Message = "Failed to obtain status."
                        };
                    }

                    //forces the background proces to update the current coordinates
                    if (i % 10 == 0)
                    {
                        //await CheckAvailability(); //used for debugging
                        await CheckEntry();
                    }

                    Device.BeginInvokeOnMainThread(() => { MessagingCenter.Send(message, "TickedMessage"); });
                }
            }, token);
        }

        //send push notification
        private async void NotifyUser(string msg)
        {
            await CrossNotifications.Current.Send(new Notification
            {
                Title = "Geofence Update",
                Message = msg,
                Vibrate = true
            });
        }

        //send text-to-speech notification
        private async void TtsNotification(string msg)
        {
            await CrossTextToSpeech.Current.Speak(msg);
        }

        //check whether the car has entered the geofence
        private async Task CheckEntry()
        {
            IRestService restService = new RestService();
            var cars = await restService.GetCarsAsync();
            if (cars == null)
            {
                _entered = null;
            }
            else
            {
                if (cars.Contains(Settings.CarSettings))
                {
                    if (_entered != true)
                    {
                        await CheckAvailability();
                        _entered = true;
                    }
                }
                else
                {
                    _entered = false;
                }
            }
        } 

        //API call
        private async Task CheckAvailability()
        {
            IRestService restService = new RestService();
            var carparkList = await restService.GetCarParksAsync();
            var carparks = carparkList.value;
            var priorities = new List<string>() {"QNH", "voetbalveld", "school"}; //temp list of priority carparks

            if (carparks.Count == 0)
            {
                //failed to obtain parking information
                return;
            }
            
            //check priorities
            foreach (var priority in priorities)
            {
                var carpark = carparks.FirstOrDefault(x => x.value.name.Contains(priority) && x.value.availableParkingSpots > 0);
                if (carpark != null)
                {
                    NotifyUser(
                        priority + " has " + carpark.value.availableParkingSpots + " parking spots available");
                    TtsNotification(priority + " has " + carpark.value.availableParkingSpots +
                                    " parking spots available");
                    return;
                }
            }

            //check remaining parking spots
            var remainingCarpark = carparks.FirstOrDefault(x => x.value.availableParkingSpots > 0);
            if (remainingCarpark != null)
            {
                NotifyUser(
                    remainingCarpark.value.name + " has " + remainingCarpark.value.availableParkingSpots + " parking spots available");
                TtsNotification(remainingCarpark.value.name + " has " + remainingCarpark.value.availableParkingSpots +
                                " parking spots available");
                return;
            }

            //no spots available
            NotifyUser("No parking spots available");
            TtsNotification("No parking spots available");
            return;
        }
    }
}
