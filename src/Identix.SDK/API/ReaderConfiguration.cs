using Identix.SDK.Readers.Entities.RFID;
using System.Collections.Generic;

namespace Identix.SDK.Readers.Entities
{
    public class ReaderConfiguration
    {
        public double Heap_size { get; set; }
        public int App_uptime { get; set; }
        public string Version { get; set; }
        public string Device_name{ get; set; }
        public string Alias { get; set; }
        public string Mac_address { get; set; }
        public string Chip_id { get; set; }
        public double Usb_power { get; set; }
        public LoginEntity Login { get; set; }
        public WifiEntity Wifi { get; set; }
        public List<string> Firmware_update { get; set; }

        //public RFIDConfiguration RFID { get; set; }

        //public ReaderConfiguration()
        //{
        //    RFID = new RFIDConfiguration();
        //}
    }
}
