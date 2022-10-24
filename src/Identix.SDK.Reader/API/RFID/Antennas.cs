using Newtonsoft.Json;

namespace Identix.SDK.API.RFID
{
    public class Antennas
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("reverse_power_cdbm")]
        public float ReversePowerCdbm { get; set; }

        [JsonProperty("tx_power_cdbm")]
        public int TxPowerCdbm { get; set; }
    }
}