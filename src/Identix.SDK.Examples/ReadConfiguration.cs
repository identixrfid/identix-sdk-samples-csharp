////////////////////////////////////////////////////////////////////////////////
//
//    Read Configuration
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Identix.SDK.Examples.Full
{
    class ReadConfiguration
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

                // Connect to the reader
                if (reader.Connect())
                {
                    // Get all settings
                    reader.GetAllSettings();

                    // Reader Settings                
                    Console.WriteLine("...RFID > Radio > Model: " + reader.ReaderSettings.RFID.Radio.Model);
                    Console.WriteLine("...RFID > Radio > Version: " + reader.ReaderSettings.RFID.Radio.Version);
                    Console.WriteLine("...RFID > Radio > Serie Number: " + reader.ReaderSettings.RFID.Radio.SerialNumber);
                    Console.WriteLine("...RFID > Radio > Region: " + reader.ReaderSettings.RFID.Radio.Region);

                    Console.WriteLine("...RFID > ModeAndSession > Profile: " + reader.ReaderSettings.RFID.ModeAndSession.Profile);
                    Console.WriteLine("...RFID > ModeAndSession > SearchMode: " + reader.ReaderSettings.RFID.ModeAndSession.SearchMode);
                    Console.WriteLine("...RFID > ModeAndSession > Session: " + reader.ReaderSettings.RFID.ModeAndSession.Session);
                    Console.WriteLine("...RFID > ModeAndSession > PopulationEstimate: " + reader.ReaderSettings.RFID.ModeAndSession.PopulationEstimate);

                    foreach (var antenna in reader.ReaderSettings.RFID.Antennas)
                    {
                        Console.WriteLine("...RFID > Antennas > Enable: " + antenna.Enable);
                        Console.WriteLine("...RFID > Antennas > TxPowerCdbm: " + antenna.TxPowerCdbm);
                    }
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