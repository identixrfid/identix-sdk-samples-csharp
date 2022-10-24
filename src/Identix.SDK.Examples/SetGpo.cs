////////////////////////////////////////////////////////////////////////////////
//
//    Set Gpo
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace Identix.SDK.Examples.Full
{
    class SetGpo
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
                    Console.WriteLine("Reader connected!");

                    // Get all settings
                    reader.GetAllSettings();

                    // Socket
                    reader.ReaderSettings.DataOutput.Socket.Enable = true;
                    reader.ReaderSettings.DataOutput.Socket.Port = 14150;

                    // Apply new settings
                    reader.ApplySettings();

                    // Set Gpo
                    reader.SetGpo(1, true);
                    Thread.Sleep(5000);
                    reader.SetGpo(1, false);
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
