using Identix.SDK.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;

namespace Identix.SDK
{
    public delegate void TagsReportedHandler(Reader sender, string data);
    public delegate void BeaconsReportedHandler(Reader sender, string data);
    public delegate void HeartbeatReportedHandler(Reader sender, string data);

    public class Reader
    {
        public event TagsReportedHandler TagsReportedHandler;
        public event BeaconsReportedHandler BeaconsReportedHandler;
        public event HeartbeatReportedHandler HeartbeatReportedHandler;

        public string Name { get; private set; }
        public string Address { get; private set; }
        public uint Port { get; private set; }
        public bool IsConnected { get; set; }
        public bool IsRunningInventory { get; private set; }
        public ReaderSettings ReaderSettings { get; set; }
        public ReaderSettings _ReaderSettings { get; set; }
        private SocketClient SocketClient { get; set; }

        /// <summary>
        /// New reader instance
        /// </summary>
        /// <param name="address">Host or IP (192.168.4.1 is a default)</param>
        /// <param name="port">Communication port (80 is a default)</param>
        public Reader(string address = "192.168.4.1", uint port = 80, string name = "")
        {
            this.Name = string.IsNullOrEmpty(name) ? address : name;
            this.Address = address;
            this.Port = port;
            this.ReaderSettings = new ReaderSettings();
            this.IsConnected = false;
            this.IsRunningInventory = false;
        }

        /// <summary>
        /// Connect a reader
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            // O leitor precisa ter um método de autenticação
            // A autenticação devérá ser tratada aqui           

            this.IsConnected = true;

            return this.IsConnected;
        }

        /// <summary>
        /// Disconnect reader
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            if (!this.IsConnected) return true;

            // Se existirem variáveis relacionadas a autenticação estas dever sem tratadas aqui          

            this.IsConnected = false;

            return this.IsConnected;
        }

        /// <summary>
        /// Start inventory tags
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (this.ReaderSettings.DataOutput.Socket.Enable)
            {
                this.SocketClient = new SocketClient(this);
            }

            try
            {
                InventoryCommand inventoryCommand = new InventoryCommand
                {
                    Inventory = true,
                    BLE = this.ReaderSettings.BLE.Enable
                };

                string json = JsonConvert.SerializeObject(inventoryCommand).ToLower();
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = HttpPut("start/stop", json);

                if (response.IsSuccessStatusCode) IsRunningInventory = true;

                if (this.SocketClient != null)
                {
                    this.SocketClient.Connect();
                }
            }
            catch (Exception ex)
            {
                this.IsRunningInventory = false;
                throw new Exception(ReaderErrors.StartInventory + "|" + ex.Message);
            }

            return this.IsRunningInventory;
        }

        /// <summary>
        /// Stop inventory tags
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
                InventoryCommand inventoryCommand = new InventoryCommand
                {
                    Inventory = false
                };

                string json = JsonConvert.SerializeObject(inventoryCommand).ToLower();
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = HttpPut("start/stop", json);

                if (response.IsSuccessStatusCode) IsRunningInventory = false;

                if (this.SocketClient != null)
                {
                    this.SocketClient.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ReaderErrors.StopInventory + "|" + ex.Message);
            }

            return IsRunningInventory;
        }

        /// <summary>
        /// Get all reader settings
        /// </summary>
        public void GetAllSettings()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response = HttpGet("config/all");

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStringAsync().Result;
                    this._ReaderSettings = JsonConvert.DeserializeObject<ReaderSettings>(contentStream);
                    this.ReaderSettings = JsonConvert.DeserializeObject<ReaderSettings>(contentStream);
                }
                else
                {
                    throw new Exception(ReaderErrors.GetSettingsError + "|" + ReaderErrors.HttpError + " - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ReaderErrors.GetSettingsError + "|" + ex.Message);
            }
        }

        /// <summary>
        /// Apply settings
        /// </summary>
        public void ApplySettings()
        {
            #region RFID

            if (this.ReaderSettings.RFID != null)
            {
                if (this.ReaderSettings.RFID.ModeAndSession != null && !JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.RFID.ModeAndSession), JsonConvert.SerializeObject(this._ReaderSettings.RFID.ModeAndSession)))
                {
                    try
                    {
                        HttpResponseMessage response = HttpPut("config/rfid/mode_and_session", JsonConvert.SerializeObject(this.ReaderSettings.RFID.ModeAndSession).ToLower());

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(ReaderErrors.ApplySettings);
                        }

                        this._ReaderSettings.RFID.ModeAndSession = this.ReaderSettings.RFID.ModeAndSession;

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                    }
                }

                if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.RFID.Antennas), JsonConvert.SerializeObject(this._ReaderSettings.RFID.Antennas)))
                {
                    try
                    {
                        HttpResponseMessage response = HttpPut("config/rfid/antennas", JsonConvert.SerializeObject(this.ReaderSettings.RFID.Antennas).ToLower());

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(ReaderErrors.ApplySettings);
                        }

                        this._ReaderSettings.RFID.Antennas = this.ReaderSettings.RFID.Antennas;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                    }
                }

                if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.RFID.ReportFields), JsonConvert.SerializeObject(this._ReaderSettings.RFID.ReportFields)))
                {
                    try
                    {
                        HttpResponseMessage response = HttpPut("config/rfid/report_fields", JsonConvert.SerializeObject(this.ReaderSettings.RFID.ReportFields).ToLower());

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(ReaderErrors.ApplySettings);
                        }

                        this._ReaderSettings.RFID.ReportFields = this.ReaderSettings.RFID.ReportFields;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                    }
                }

                if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.RFID.Filter), JsonConvert.SerializeObject(this._ReaderSettings.RFID.Filter)))
                {
                    try
                    {
                        HttpResponseMessage response = HttpPut("config/rfid/filter", JsonConvert.SerializeObject(this.ReaderSettings.RFID.Filter).ToLower());

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(ReaderErrors.ApplySettings);
                        }

                        this._ReaderSettings.RFID.Filter = this.ReaderSettings.RFID.Filter;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                    }
                }
            }

            #endregion

            #region BLE

            if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.BLE), JsonConvert.SerializeObject(this._ReaderSettings.BLE)))
            {
                try
                {
                    HttpResponseMessage response = HttpPut("config/ble", JsonConvert.SerializeObject(this.ReaderSettings.BLE).ToLower());

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(ReaderErrors.ApplySettings);
                    }

                    this._ReaderSettings.BLE = this.ReaderSettings.BLE;
                }
                catch (Exception ex)
                {
                    throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                }
            }

            #endregion

            #region Output

            if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Heartbeat), JsonConvert.SerializeObject(this._ReaderSettings.DataOutput.Heartbeat)))
            {
                try
                {
                    HttpResponseMessage response = HttpPut("config/data_output/heartbeat", JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Heartbeat).ToLower());

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(ReaderErrors.ApplySettings);
                    }

                    this._ReaderSettings.DataOutput.Heartbeat = this.ReaderSettings.DataOutput.Heartbeat;
                }
                catch (Exception ex)
                {
                    throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                }
            }

            if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Socket), JsonConvert.SerializeObject(this._ReaderSettings.DataOutput.Socket)))
            {
                try
                {
                    HttpResponseMessage response = HttpPut("config/data_output/socket", JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Socket).ToLower());

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(ReaderErrors.ApplySettings);
                    }

                    this._ReaderSettings.DataOutput.Socket = this.ReaderSettings.DataOutput.Socket;
                }
                catch (Exception ex)
                {
                    throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                }
            }

            if (!JToken.DeepEquals(JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Mqtt), JsonConvert.SerializeObject(this._ReaderSettings.DataOutput.Mqtt)))
            {
                try
                {
                    HttpResponseMessage response = HttpPut("config/data_output/mqtt", JsonConvert.SerializeObject(this.ReaderSettings.DataOutput.Mqtt).ToLower());

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(ReaderErrors.ApplySettings);
                    }

                    this._ReaderSettings.DataOutput.Mqtt = this.ReaderSettings.DataOutput.Mqtt;
                }
                catch (Exception ex)
                {
                    throw new Exception(ReaderErrors.ApplySettings + "|" + ex.Message);
                }
            }

            #endregion
        }

        /// <summary>
        /// Retrieve data from REST API
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private HttpResponseMessage HttpPut(string endpoint, string body)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + this.Address + ":" + this.Port);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(body, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PutAsync(client.BaseAddress + endpoint, content).Result;

                return response;
            }
        }

        /// <summary>
        /// Send data to REST API
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        private HttpResponseMessage HttpGet(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://" + this.Address + ":" + this.Port);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(client.BaseAddress + endpoint).Result;
                string json = response.Content.ReadAsStringAsync().Result;

                return response;
            }
        }

        /// <summary>
        /// Event tag reported
        /// </summary>
        /// <param name="report"></param>
        public virtual void OnTagsReported(string report)
        {
            if (TagsReportedHandler != null)
                TagsReportedHandler(this, report);
        }

        /// <summary>
        /// Event beacon reported
        /// </summary>
        /// <param name="report"></param>
        public virtual void OnBeaconsReported(string report)
        {
            if (BeaconsReportedHandler != null)
                BeaconsReportedHandler(this, report);
        }

        /// <summary>
        /// Event heartbeat reported
        /// </summary>
        /// <param name="report"></param>
        public virtual void OnHeartbeatReported(string report)
        {
            if (HeartbeatReportedHandler != null)
                HeartbeatReportedHandler(this, report);
        }
    }

    /// <summary>
    /// Reader Profiles
    /// </summary>
    public class Profile
    {
        public static int AUTO
        {
            get { return 0; }
        }

        public static int DRM_FCC
        {
            get { return 1; }
        }

        public static int DRM_ETSI
        {
            get { return 2; }
        }

        public static int FAST_MODE
        {
            get { return 3; }
        }

        public static int SENSITIVE_MODE
        {
            get { return 4; }
        }
    }

    /// <summary>
    /// Reader Search Modes
    /// </summary>
    public class SearchMode
    {
        public static int DualTarget
        {
            get { return 0; }
        }

        public static int SingleTarget_A_TO_B
        {
            get { return 1; }
        }

        public static int SingleTarget_B_TO_A
        {
            get { return 2; }
        }

        public static int TagFocus
        {
            get { return 3; }
        }
    }

    /// <summary>
    /// Reader Filters
    /// </summary>
    public class Filter
    {
        public static int TARGET_SESSION_S0
        {
            get { return 0; }
        }

        public static int TARGET_SESSION_S1
        {
            get { return 1; }
        }

        public static int TARGET_SESSION_S2
        {
            get { return 2; }
        }

        public static int TARGET_SESSION_S3
        {
            get { return 3; }
        }

        public static int TARGET_SL_FLAG
        {
            get { return 4; }
        }

        public static int TARGET_RFU_1
        {
            get { return 5; }
        }

        public static int TARGET_RFU_2
        {
            get { return 6; }
        }

        public static int TARGET_RFU_3
        {
            get { return 7; }
        }
    }

    /// <summary>
    /// BLE Scan
    /// </summary>
    public class BleScan
    {
        public static int Fast
        {
            get { return 0; }
        }

        public static int Normal
        {
            get { return 1; }
        }

        public static int Slow
        {
            get { return 2; }
        }
    }
}
