using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class ModeAndSession
    {
        [JsonProperty("search_mode")]
        public int SearchMode { get; set; }

        [JsonProperty("session")]
        public int Session { get; set; }

        [JsonProperty("profile")]
        public int Profile { get; set; }

        [JsonProperty("population_estimate")]
        public int PopulationEstimate { get; set; }
    }
}