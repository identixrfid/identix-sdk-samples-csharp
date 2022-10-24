////////////////////////////////////////////////////////////////////////////////
//
//    Gpi Events
//
////////////////////////////////////////////////////////////////////////////////

using System;

namespace Identix.SDK.Examples.Full
{
    class GpiEvents
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
                reader.GpiReportedHandler += new GpiReportedHandler(OnGpiReported);

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
                }

                Console.WriteLine("Press enter to exit.");
                Console.ReadKey();

                // Disconnect from the reader
                reader.Disconnect();
            }
            catch (Exception ex)
            {
                // .NET errors
                Console.WriteLine("Exception : {0}", ex.Message);
            }
        }

        /// <summary>
        /// Write on console the gpi that were reported
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        static void OnGpiReported(object source, string data)
        {
            Console.WriteLine(string.Format("GPI: {0}", data));
        }
    }
}
