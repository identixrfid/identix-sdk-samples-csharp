////////////////////////////////////////////////////////////////////////////////
//
//    Multiple Readers
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Threading;

namespace Identix.SDK.Examples.Core
{
    class MultipleReaders
    {
        // Create a collection to hold all the ImpinjReader instances.
        static List<Reader> readers = new List<Reader>();

        static void Main(string[] args)
        {
            try
            {
                readers.Add(new Reader("10.0.0.101", 80, "Reader #1"));
                readers.Add(new Reader("10.0.0.102", 80, "Reader #2"));

                // Loop through the List of readers to configure and start them
                foreach (Reader reader in readers)
                {
                    // Subscribe callback events
                    reader.TagsReportedHandler += new TagsReportedHandler(OnTagsReported);

                    // Connect to the reader
                    reader.Connect();

                    // Get all settings
                    reader.GetAllSettings();

                    // Apply new settings
                    reader.ReaderSettings.RFID.Antennas[0].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[0].TxPowerCdbm = 2300;
                    reader.ReaderSettings.RFID.Antennas[1].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[1].TxPowerCdbm = 2300;
                    reader.ReaderSettings.DataOutput.Socket.Enable = true;
                    reader.ReaderSettings.DataOutput.Socket.Port = 14150;
                    reader.ReaderSettings.RFID.ReportFields.FastId = false;
                    reader.ReaderSettings.RFID.ReportFields.Rssi = true;
                    reader.ReaderSettings.RFID.ReportFields.Phase = false;
                    reader.ReaderSettings.RFID.ReportFields.Channel = false;
                    reader.ReaderSettings.RFID.ReportFields.Antenna = true;
                    reader.ReaderSettings.RFID.ReportFields.ReaderName = false;
                    reader.ReaderSettings.RFID.ReportFields.Timestamp = true;
                    reader.ApplySettings();

                    // Start reading
                    reader.Start();
                }

                Thread.Sleep(10000);

                // Loop through the List of readers to stop and disconnect them
                foreach (Reader reader in readers)
                {
                    // Stop inventory
                    reader.Stop();

                    // Disconnect from the reader
                    reader.Disconnect();
                }

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
        static void OnTagsReported(Reader sender, string data)
        {
            Console.WriteLine(string.Format("{0} Read: {1}", sender.Name, data));
        }
    }
}
