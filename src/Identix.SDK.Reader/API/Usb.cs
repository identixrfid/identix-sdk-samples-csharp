using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Usb
    {
        [JsonProperty("mode")]
        public int Mode { get; set; }

        [JsonProperty("format")]
        public UsbFormat Format { get; set; }
    }
}