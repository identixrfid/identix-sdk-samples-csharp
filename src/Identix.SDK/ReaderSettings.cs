using Identix.SDK.API;
using Identix.SDK.API.BLE;
using Identix.SDK.API.RFID;
using Newtonsoft.Json;

namespace Identix.SDK
{
    public class ReaderSettings
    {
        [JsonProperty("heap_size")]
        public int HeapSize { get; set; }

        [JsonProperty("app_uptime")]
        public int AppUptime { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }
        
        [JsonProperty("mac_address")]
        public string MacAddress { get; set; }
        
        [JsonProperty("chip_id")]
        public string ChipId { get; set; }
        
        [JsonProperty("usb_power")]
        public float UsbPower { get; set; }

        [JsonProperty("login")]
        public Login Login { get; set; }

        [JsonProperty("wifi")]
        public Wifi Wifi { get; set; }
        
        [JsonProperty("firmware_update")]
        public FirmwareUpdate FirmwareUpdate { get; set; }
        
        [JsonProperty("date_time")]
        public Datetime DateTime { get; set; }
        
        public BLE BLE { get; set; }
        
        public RFID RFID { get; set; }
        
        [JsonProperty("data_output")]
        public DataOutput DataOutput { get; set; }
        
        public Usb USB { get; set; }
    }
}