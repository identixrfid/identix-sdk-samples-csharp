////////////////////////////////////////////////////////////////////////////////
//
//    Raw Mode Read Tags
//    Dependencies:
//      - System.IO.Ports >= 6.0.0
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace Identix.SDK.Examples.Full
{
    class RawModeReadTags
    {
        static void Main(string[] args)
        {
            Console.Write("Inform the virtual COM port: ");
            string comPort = Console.ReadLine();
            Console.WriteLine("COM port {0}", comPort);

            try
            {
                // Create a new reader instance
                Reader reader = new Reader(comPort);

                // Subscribe callback events
                reader.TagsReportedHandler += new TagsReportedHandler(OnTagsReported);

                // Connect to the reader
                if (reader.Connect())
                {
                    // Get all settings
                    reader.GetAllSettings();

                    // Configure new settings

                    // Mode and session
                    reader.ReaderSettings.RFID.ModeAndSession.SearchMode = SearchMode.DualTarget;
                    reader.ReaderSettings.RFID.ModeAndSession.Session = 0;
                    reader.ReaderSettings.RFID.ModeAndSession.PopulationEstimate = 4;

                    // Antennas
                    reader.ReaderSettings.RFID.Antennas[0].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[0].TxPowerCdbm = 2300;
                    reader.ReaderSettings.RFID.Antennas[1].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[1].TxPowerCdbm = 1300;

                    // Enable or disable view data
                    reader.ReaderSettings.RFID.ReportFields.ReaderName = false;
                    reader.ReaderSettings.RFID.ReportFields.Timestamp = false;
                    reader.ReaderSettings.RFID.ReportFields.Rssi = true;
                    reader.ReaderSettings.RFID.ReportFields.Phase = false;
                    reader.ReaderSettings.RFID.ReportFields.Channel = false;
                    reader.ReaderSettings.RFID.ReportFields.FastId = false;
                    reader.ReaderSettings.RFID.ReportFields.Antenna = true;

                    // Apply new settings
                    reader.ApplySettings();

                    // Start inventory
                    reader.Start();

                    Thread.Sleep(10000);

                    // Stop inventory
                    reader.Stop();

                    // Disconnect from the reader
                    reader.Disconnect();

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("COM port not connected. Press any key to continue...");
                    Console.ReadKey();
                }
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
            Console.WriteLine(data);
        }
    }
}