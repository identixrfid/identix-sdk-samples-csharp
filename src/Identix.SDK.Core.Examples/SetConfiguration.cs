////////////////////////////////////////////////////////////////////////////////
//
//    Set Configuration
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Identix.SDK.Examples.Core
{
    class SetConfiguration
    {
        static void Main(string[] args)
        {
            Console.Write("Inform the ip or host: ");
            string ipHost = Console.ReadLine();

            try
            {
                // Create a new reader instance
                Reader reader = new Reader(ipHost);

                // Connect to the reader
                if (reader.Connect())
                {
                    // Get all settings
                    reader.GetAllSettings();

                    // Configure new settings

                    // Mode and session
                    reader.ReaderSettings.RFID.ModeAndSession.Profile = Profile.AUTO;
                    reader.ReaderSettings.RFID.ModeAndSession.SearchMode = SearchMode.SingleTarget_A_TO_B;
                    reader.ReaderSettings.RFID.ModeAndSession.Session = 1;
                    reader.ReaderSettings.RFID.ModeAndSession.PopulationEstimate = 4;

                    // Antennas
                    reader.ReaderSettings.RFID.Antennas[0].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[0].TxPowerCdbm = 1700;
                    reader.ReaderSettings.RFID.Antennas[1].Enable = true;
                    reader.ReaderSettings.RFID.Antennas[1].TxPowerCdbm = 1700;

                    // Filters
                    reader.ReaderSettings.RFID.Filter.RssiFilterEnable = true;
                    reader.ReaderSettings.RFID.Filter.RssiThreshold = -6000;

                    // Heartbeat
                    reader.ReaderSettings.DataOutput.Heartbeat.EnableOnSocket = true;
                    reader.ReaderSettings.DataOutput.Heartbeat.PeriodSec = 5;

                    // Socket
                    reader.ReaderSettings.DataOutput.Socket.Enable = true;
                    reader.ReaderSettings.DataOutput.Socket.Port = 14150;

                    // Apply new settings
                    reader.ApplySettings();
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
    }
}