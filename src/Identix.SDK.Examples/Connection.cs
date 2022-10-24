////////////////////////////////////////////////////////////////////////////////
//
//    Connection
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Threading;

namespace Identix.SDK.Examples.Full
{
    class Connection
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