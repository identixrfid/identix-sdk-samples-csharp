using System;
using System.Threading;

namespace Identix.SDK.Demo
{
    class Program
    {
        static Reader reader;

        static void Main(string[] args)
        {
            // Create a new reader instance
            reader = new Reader("10.0.0.101");

            // Subscribe callback events
            reader.TagsReportedHandler += new TagsReportedHandler(OnTagsReported);
            reader.HeartbeatReportedHandler += new HeartbeatReportedHandler(OnHeartbeatReported);

            // Connect on reader
            if (reader.Connect())
            {
                // Get all settings
                reader.GetAllSettings();

                ApplySettings();
                WriteSettings();

                // Start inventory
                reader.Start();
                Console.WriteLine("Inventory started!");

                Thread.Sleep(10000);

                // Stop inventory
                reader.Stop();

                // Disconnect reader
                reader.Disconnect();
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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
        /// Write on console the heartbeat that were reported
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        static void OnHeartbeatReported(object source, string data)
        {
            Console.WriteLine(string.Format("Heartbeat: {0}", data));
        }

        /// <summary>
        /// Write the settings
        /// </summary>
        static void WriteSettings()
        {
            Console.WriteLine("List Settings:");
            Console.WriteLine("...RFID > Radio > Model: " + reader.ReaderSettings.RFID.Radio.Model);
            Console.WriteLine("...RFID > Radio > Version: " + reader.ReaderSettings.RFID.Radio.Version);
            Console.WriteLine("...RFID > Radio > Serie Number: " + reader.ReaderSettings.RFID.Radio.SerialNumber);
            Console.WriteLine("...RFID > Radio > Region: " + reader.ReaderSettings.RFID.Radio.Region);

            foreach (var antenna in reader.ReaderSettings.RFID.Antennas)
            {
                Console.WriteLine("...RFID > Antennas > Enable: " + antenna.Enable);
                Console.WriteLine("...RFID > Antennas > TxPowerCdbm: " + antenna.TxPowerCdbm);
            }

            Console.WriteLine("...RFID > Inventory > Automatic: " + reader.ReaderSettings.RFID.Inventory.Automatic);
            Console.WriteLine("...RFID > Inventory > Cycle > Duration: " + reader.ReaderSettings.RFID.Inventory.Cycle.Duration);
            Console.WriteLine("...RFID > Inventory > Cycle > Enable: " + reader.ReaderSettings.RFID.Inventory.Cycle.Enable);
            Console.WriteLine("...RFID > Inventory > Cycle > IDLE: " + reader.ReaderSettings.RFID.Inventory.Cycle.Idle);
            Console.WriteLine("...RFID > Inventory > Running: " + reader.ReaderSettings.RFID.Inventory.Running);
            Console.WriteLine("...RFID > Inventory > SmartBuffer > Enable: " + reader.ReaderSettings.RFID.Inventory.SmartBuffer.Enable);
            Console.WriteLine("...RFID > Inventory > SmartBuffer > Leave: " + reader.ReaderSettings.RFID.Inventory.SmartBuffer.Leave);
            Console.WriteLine("...RFID > Inventory > SmartBuffer > Mode: " + reader.ReaderSettings.RFID.Inventory.SmartBuffer.Mode);
            Console.WriteLine("...RFID > Inventory > SmartBuffer > Period: " + reader.ReaderSettings.RFID.Inventory.SmartBuffer.Period);
        }

        /// <summary>
        /// Apply the settings
        /// </summary>
        static void ApplySettings()
        {
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

            // Report fields
            reader.ReaderSettings.RFID.ReportFields.FastId = false;
            reader.ReaderSettings.RFID.ReportFields.Rssi = true;
            reader.ReaderSettings.RFID.ReportFields.Phase = false;
            reader.ReaderSettings.RFID.ReportFields.Channel = false;
            reader.ReaderSettings.RFID.ReportFields.Antenna = false;
            reader.ReaderSettings.RFID.ReportFields.ReaderName = false;
            reader.ReaderSettings.RFID.ReportFields.Timestamp = true;

            // Filters
            reader.ReaderSettings.RFID.Filter.RssiFilterEnable = false;
            reader.ReaderSettings.RFID.Filter.RssiThreshold = -6000;
            //reader.ReaderSettings.RFID.Filter.Target = Filter.TARGET_SL_FLAG;

            // Heartbeat
            reader.ReaderSettings.DataOutput.Heartbeat.EnableOnSocket = false;
            reader.ReaderSettings.DataOutput.Heartbeat.PeriodSec = 5;

            // Socket
            reader.ReaderSettings.DataOutput.Socket.Enable = true;
            reader.ReaderSettings.DataOutput.Socket.Port = 14150;

            // MQTT
            //reader.ReaderSettings.DataOutput.Mqtt.BleTopic = "";
            //reader.ReaderSettings.DataOutput.Mqtt.ClientId = "";
            //reader.ReaderSettings.DataOutput.Mqtt.CommandTopic = "";
            //reader.ReaderSettings.DataOutput.Mqtt.Enable = true;
            //reader.ReaderSettings.DataOutput.Mqtt.EnableTls = true;
            //reader.ReaderSettings.DataOutput.Mqtt.HeartbeatTopic = "";
            //reader.ReaderSettings.DataOutput.Mqtt.InfoTopic = "";
            //reader.ReaderSettings.DataOutput.Mqtt.InventoryTopic = "";
            //reader.ReaderSettings.DataOutput.Mqtt.Password = "";
            //reader.ReaderSettings.DataOutput.Mqtt.Port = 1883;
            //reader.ReaderSettings.DataOutput.Mqtt.Url = "";
            //reader.ReaderSettings.DataOutput.Mqtt.Username = "";

            reader.ApplySettings();
        }
    }
}