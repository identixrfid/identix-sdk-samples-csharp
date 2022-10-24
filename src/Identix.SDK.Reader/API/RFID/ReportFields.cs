using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class ReportFields
    {
        [JsonProperty("fast_id")]
        public bool FastId { get; set; }

        [JsonProperty("rssi")]
        public bool Rssi { get; set; }

        [JsonProperty("phase")]
        public bool Phase { get; set; }

        [JsonProperty("channel")]
        public bool Channel { get; set; }

        [JsonProperty("antenna")]
        public bool Antenna { get; set; }

        [JsonProperty("reader_name")]
        public bool ReaderName { get; set; }

        [JsonProperty("timestamp")]
        public bool Timestamp { get; set; }
    }
}
