using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using ProjectParking.Contracts;

namespace ProjectParking.Simulators.Json
{
    public class SpotConfiguration
    {
        public static SpotConfiguration Parse(string path)
        {
            var textFile = WaitForFile(path, FileMode.Open, FileAccess.Read, FileShare.Read);

            Console.WriteLine("CONFIG: \r\n\r\n");
            Console.WriteLine(textFile);

            Console.WriteLine("\r\n\r\n");


            var configuration = JsonConvert.DeserializeObject<SpotConfiguration>(textFile);

            for (int i = 0; i < configuration.Spots.Length; i++)
            {
                configuration.Spots[i].SpotId = i;
            }

            return configuration;
        }

        public static void Write(string path, SpotConfiguration config)
        {
            var json = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public Spot[] Spots { get; set; }

        static string WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fullPath, mode, access, share);
                    fs.Dispose();
                    return File.ReadAllText(fullPath);
                }
                catch (IOException)
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                    Thread.Sleep(50);
                }
            }

            return null;
        }
    }

    public class Spot : IParkingSpotStatusUpdate
    {
        public Spot()
        {
            MetaData = new Dictionary<string, string>();
        }

        public int SpotId { get; set; }
        public string Location { get; set; }
        public bool Available { get; set; }
        public DateTime Timestamp { get; set; }
        public IDictionary<string, string> MetaData { get; set; }
    }



}