using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class DataTransformation
    {
        [JsonProperty("gtin_enable")]
        public bool GtinEnable { get; set; }

        [JsonProperty("epc_truncate")] 
        public bool EpcTruncate { get; set; }

        [JsonProperty("epc_truncate_start")] 
        public int EpcTruncateStart { get; set; }

        [JsonProperty("epc_truncate_length")]
        public int EpcTruncateLength { get; set; }

        [JsonProperty("epc_additional_prefix")]
        public string EpcAdditionalPrefix { get; set; }

        [JsonProperty("epc_additional_suffix")]
        public string EpcAdditionalSuffix { get; set; }
    }
}
