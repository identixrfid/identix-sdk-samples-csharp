using Newtonsoft.Json;

namespace Identix.SDK.API
{
    public class Datetime
    {
        [JsonProperty("ntp_enable")]
        public bool NtpEnable { get; set; }

        [JsonProperty("ntp_servers")]
        public string NtpServers { get; set; }

        [JsonProperty("enable_svt")]
        public bool EnableSvt { get; set; }

        public int Timezone { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
