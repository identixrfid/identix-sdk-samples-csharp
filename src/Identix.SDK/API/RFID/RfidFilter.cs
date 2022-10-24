using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class RfidFilter
    {
        [JsonProperty("rssi")]
        public bool RssiFilterEnable { get; set; }

        [JsonProperty("rssi_threshold")]
        public int RssiThreshold { get; set; }

        [JsonProperty("epc")]
        public bool Epc { get; set; }

        [JsonProperty("epc_regex_string")]
        public string EpcRegexString { get; set; }
        
        public bool GEN2 { get; set; }

        [JsonProperty("memory_bank")]
        public int MemoryBank { get; set; }
        
        public int Target { get; set; }
        public int Action { get; set; }

        [JsonProperty("bit_pointer")]
        public int BitPointer { get; set; }

        [JsonProperty("mask_length")]
        public int MaskLength { get; set; }

        [JsonProperty("hexadecimal_mask")]
        public string HexadecimalMask { get; set; }
    }
}
