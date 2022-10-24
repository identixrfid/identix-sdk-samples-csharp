////////////////////////////////////////////////////////////////////////////////
//
//    Read Tags and Beacons
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace Identix.SDK.Examples.Core
{
    class ReadTagsAndBeacons
    {
        static void Main(string[] args)
        {
            Console.Write("Inform the ip or host: ");
            string ipHost = Console.ReadLine();
            Console.WriteLine("IP or host {0}", ipHost);

            try
            {
                // Create a new reader instance
                Reader reader = new Reader(ipHost);

                // Subscribe callback events
                reader.TagsReportedHandler += new TagsReportedHandler(OnTagsReported);
                reader.BeaconsReportedHandler += new BeaconsReportedHandler(OnBeaconsReported);

                // Connect to the reader
                if (reader.Connect())
                {
                    // Get all settings
                    reader.GetAllSettings();

                    // Configure new settings

                    // Mode and session
                    reader.ReaderSettings.RFID.ModeAndSession.Profile = Profile.AUTO;
                    reader.ReaderSettings.RFID.ModeAndSession.SearchMode = SearchMode.DualTarget;
                    reader.ReaderSettings.RFID.ModeAndSession.Session = 0;
                    reader.ReaderSettings.RFID.ModeAndSession.PopulationEstimate = 4;

                    // Antennas
                    reader.ReaderSettings.RFID.Antennas[0].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[0].TxPowerCdbm = 2300;
                    reader.ReaderSettings.RFID.Antennas[1].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[1].TxPowerCdbm = 2300;

                    // BLE configuration
                    reader.ReaderSettings.BLE.Enable = false;
                    reader.ReaderSettings.BLE.Automatic = false;
                    reader.ReaderSettings.BLE.ScanSpeed = BleScan.Fast;
                    reader.ReaderSettings.BLE.Filter.RssiFilterEnable = false;
                    reader.ReaderSettings.BLE.Filter.RssiFilterValue = -90;
                    reader.ReaderSettings.BLE.Filter.Altbeacon = true;
                    reader.ReaderSettings.BLE.Filter.EddystoneTml = true;
                    reader.ReaderSettings.BLE.Filter.EddystoneUid = true;
                    reader.ReaderSettings.BLE.Filter.EddystoneUrl = true;
                    reader.ReaderSettings.BLE.Filter.Generic = true;
                    reader.ReaderSettings.BLE.Filter.Ibeacon = true;
                    reader.ReaderSettings.BLE.Filter.Identix = true;
                    reader.ReaderSettings.BLE.Filter.IdentixAccelerometer = true;
                    reader.ReaderSettings.BLE.Filter.IdentixTemperature = true;

                    // Socket
                    reader.ReaderSettings.DataOutput.Socket.Enable = true;
                    reader.ReaderSettings.DataOutput.Socket.Port = 14150;

                    // Apply new settings
                    reader.ApplySettings();

                    // Start inventory
                    reader.Start();

                    Thread.Sleep(10000);

                    // Stop inventory
                    reader.Stop();
                }

                // Disconnect from the reader
                reader.Disconnect();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                // .NET errors
                Console.WriteLine("Exception : {0}", ex.Message);
            }            
        }

        /// <summary>
        /// Write on console the tags that were reported
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        static void OnTagsReported(object source, string data)
        {
            Console.WriteLine(string.Format("Read: {0}", data));
        }

        /// <summary>
        /// Write on console the beacons that were reported
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        static void OnBeaconsReported(object source, string data)
        {
            Console.WriteLine(string.Format("Event: {0}", data));
        }
    }
}
