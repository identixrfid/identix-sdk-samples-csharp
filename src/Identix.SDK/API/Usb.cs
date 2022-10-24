using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Usb
    {
        public int Mode { get; set; }

        [JsonProperty("format")]
        public UsbFormat Format { get; set; }
    }
}