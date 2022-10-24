using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class ModeAndSession
    {
        [JsonProperty("search_mode")]
        public uint SearchMode { get; set; }

        [JsonProperty("session")]
        public uint Session { get; set; }

        [JsonProperty("profile")]
        public uint Profile { get; set; }

        [JsonProperty("population_estimate")]
        public uint PopulationEstimate { get; set; }
    }
}