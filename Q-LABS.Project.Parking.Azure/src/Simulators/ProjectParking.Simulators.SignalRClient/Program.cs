using System;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using ProjectParking.Contracts;
using System.Collections.Generic;
using ProjectParking.Contracts.enumerables;
using System.Linq;

namespace ProjectParking.Simulators.SignalRClient
{
    class Program
    {
        private const string ServerUrl = "https://tstqlabsparkingtw3tdeahyqiss.azurewebsites.net";
        private Random random = new Random();

        static async Task Main(string[] args)
        {
            var app = new Program();
            await app.RunConnection();
        }

        private async Task RunConnection()
        {
            try
            {
                var userId = "C#" + random.Next(0, 100);

                var hubConnection = new HubConnectionBuilder()
                    .WithUrl(ServerUrl + "/broadcast")
                    .Build();

                var closedTcs = new TaskCompletionSource<object>();
                hubConnection.Closed += HubConnection_Closed;
                hubConnection.On<string, string>("Message", (sender, message) => Console.WriteLine($"[{userId}] {sender}: {message}"));

                hubConnection.On<int, List<ParkingSpotImpl>>("NotifyParkingStatusForCarpark", (sender, message) =>
                {

                    if (message.Count > 0)
                    {
                        int available = message.Count(x => x.Status == SpotStatus.Available);
                        int unavailable = message.Count(x => x.Status == SpotStatus.Available) +
                                          message.Count(x => x.Status == SpotStatus.Unavailable);

                        var spots = message.Select(x => $"{x.SpotId} = {x.Status}");

                        Console.Clear();
                        Console.SetCursorPosition(0,1);
                        Console.WriteLine($"\r\n\r\nUpdate ({available}/{unavailable}):  \r\n" + string.Join(Environment.NewLine, spots));
                    }
                });

                hubConnection.On<string, List<Object>>("GetAllCarparks", (sender, message) =>
                {
                    Console.WriteLine($"We have {message.Count}");
                });

                await hubConnection.StartAsync();
                Console.WriteLine($"[{userId}] Connection Started");



                var ticks = 0;
                var nextMsgAt = 3;

                try
                {
                    while (!closedTcs.Task.IsCompleted)
                    {
                        await Task.Delay(200);

                        await hubConnection.SendAsync("GetAllCarparks");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[{userId}] Connection terminated with error: {ex}");
                }
            }
            catch (Exception e)
            {
                throw;
            }


        }

        private Task HubConnection_Closed(Exception arg)
        {
            throw arg;
        }
    }

    class ParkingSpotImpl : IParkingSpot
    {
        public int CarparkId { get; set; }
        public int SpotId { get; set; }
        public DateTime LastUpdated { get; set; }
        public SpotStatus Status { get; set; }
    }

    class CarparkImpl : ICarpark
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICarparkInfo Info { get; set; }
        public ILocation Location { get; set; }
    }

}
