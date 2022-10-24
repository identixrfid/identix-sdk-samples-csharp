using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Wifi
    {
        [JsonProperty("access_point")]
        public AccessPoint AccessPoint { get; set; }

        public Station Station { get; set; }
    }
}
