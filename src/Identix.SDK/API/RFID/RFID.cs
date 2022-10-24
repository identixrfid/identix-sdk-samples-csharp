using Newtonsoft.Json;
using System.Collections.Generic;

namespace Identix.SDK.API.RFID
{
    public class RFID
    {
        public Radio Radio { get; set; }
        
        public Inventory Inventory { get; set; }
        
        public Trigger Trigger { get; set; }
        
        public List<Antennas> Antennas { get; set; }

        [JsonProperty("mode_and_session")]
        public ModeAndSession ModeAndSession { get; set; }

        [JsonProperty("report_fields")]
        public ReportFields ReportFields { get; set; }

        [JsonProperty("data_transformation")]
        public DataTransformation DataTransformation { get; set; }

        public RfidFilter Filter { get; set; }

        public int CountAntennas { get; }

        public RFID()
        {
            if (this.Antennas != null)
                this.CountAntennas = this.Antennas.Count;
        }
    }
}