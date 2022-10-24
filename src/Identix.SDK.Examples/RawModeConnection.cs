////////////////////////////////////////////////////////////////////////////////
//
//    RawModeConnection
//    Dependencies:
//      - System.IO.Ports >= 6.0.0
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace Identix.SDK.Examples.Full
{
    class RawModeConnection
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

                // Connect to the reader
                if (reader.Connect())
                    Console.WriteLine(string.Format("Reader connected on {0}!", reader.Address));

                Thread.Sleep(3000);

                // Disconnect from the reader
                if (reader.Disconnect())
                    Console.WriteLine(string.Format("Reader disconnected"));

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