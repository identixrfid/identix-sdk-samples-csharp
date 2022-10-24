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
            if (this.Radio == null)
                this.Radio = new Radio();
            if (this.Inventory == null)
                this.Inventory = new Inventory();
            if (this.Trigger == null)
                this.Trigger = new Trigger();
            if (this.ModeAndSession == null)
                this.ModeAndSession = new ModeAndSession();
            if (this.ReportFields == null)
                this.ReportFields = new ReportFields();
            if (this.DataTransformation == null)
                this.DataTransformation = new DataTransformation();
            if (this.Filter == null)
                this.Filter = new RfidFilter();

            if (this.Antennas == null)
            {
                this.Antennas = new List<Antennas>();
                this.Antennas.Add(new Antennas { Enable = false, TxPowerCdbm = 23 });
                this.Antennas.Add(new Antennas { Enable = false, TxPowerCdbm = 23 });
                this.Antennas.Add(new Antennas { Enable = false, TxPowerCdbm = 23 });
                this.Antennas.Add(new Antennas { Enable = false, TxPowerCdbm = 23 });
            }

            if (this.Antennas != null)
                this.CountAntennas = this.Antennas.Count;
        }
    }
}