using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Roaming
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("connection")] 
        public int Connection { get; set; }

        [JsonProperty("disconnection")] 
        public int Disconnection { get; set; }

        [JsonProperty("initiate_roaming")]
        public int InitiateRoaming { get; set; }

        [JsonProperty("roaming_tr")]
        public int RoamingTr { get; set; }

        [JsonProperty("roaming_rec")]
        public int RoamingRec { get; set; }
    }
}