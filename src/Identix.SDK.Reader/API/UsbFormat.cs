using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class UsbFormat
    {
        [JsonProperty("lead_chars")]
        public string LeadChars { get; set; }

        [JsonProperty("trail_chars")]
        public string TrailChars { get; set; }

        [JsonProperty("separator")]
        public string Separator { get; set; }

        [JsonProperty("standard_json")]
        public bool StandardJson { get; set; }

        [JsonProperty("format_seq")]
        public string FormatSeq { get; set; }
    }
}