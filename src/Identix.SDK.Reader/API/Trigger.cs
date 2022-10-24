using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Trigger
    {
        [JsonProperty("led")]
        public bool Led { get; set; }

        [JsonProperty("buzzer")]
        public bool Buzzer { get; set; }

        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("threshold")]
        public int Threshold { get; set; }

        [JsonProperty("timming")]
        public int Timming { get; set; }
    }
}