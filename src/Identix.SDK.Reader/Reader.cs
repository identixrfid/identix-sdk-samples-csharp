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
    public delegate void GpiReportedHandler(Reader sender, string data);

    public class Reader
    {
        public event TagsReportedHandler TagsReportedHandler;
        public event BeaconsReportedHandler BeaconsReportedHandler;
        public event HeartbeatReportedHandler HeartbeatReportedHandler;
        public event GpiReportedHandler GpiReportedHandler;

        private string ConnectionMode = "socket";
        public string Name { get; private set; }
        public string Address { get; private set; }
        public uint Port { get; private set; }
        public bool IsConnected { get; set; }
        public bool IsRunningInventory { get; private set; }
        public ReaderSettings ReaderSettings { get; set; }
        public ReaderSettings _ReaderSettings { get; set; }
        private SocketClient SocketClient { get; set; }

        RawMode rawMode = null;

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

            if (address.ToLower().StartsWith("com"))
            {
                this.ConnectionMode = "raw";
            }

        }

        /// <summary>
        /// Connect a reader
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            // O leitor precisa ter um método de autenticação
            // A autenticação deverá ser tratada aqui

            if (this.ConnectionMode == "socket")
            {
                this.IsConnected = true;
            }
            else if (this.ConnectionMode == "raw")
            {
                if (rawMode == null)
                    rawMode = new RawMode(this);
                rawMode.SetSerialPort(this.Address);

                this.IsConnected = rawMode.OpenSerialPort();
            }

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

            if (this.ConnectionMode == "socket")
            {
                if (this.SocketClient != null)
                {
                    this.SocketClient.Disconnect();
                }
            }
            else if (this.ConnectionMode == "raw")
            {
                rawMode.CloseSerialPort();
            }

            return this.IsConnected;
        }

        /// <summary>
        /// Start inventory tags
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                if (this.ConnectionMode == "socket")
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
                }
                else if (this.ConnectionMode == "raw")
                {
                    rawMode.SendCommand(RawMode.Command.Start());
                    this.IsRunningInventory = true;
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
                if (this.ConnectionMode == "socket")
                {
                    InventoryCommand inventoryCommand = new InventoryCommand
                    {
                        Inventory = false
                    };

                    string json = JsonConvert.SerializeObject(inventoryCommand).ToLower();
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = HttpPut("start/stop", json);

                    if (response.IsSuccessStatusCode) IsRunningInventory = false;
                }
                else if (this.ConnectionMode == "raw")
                {
                    rawMode.SendCommand(RawMode.Command.Stop());
                    this.IsRunningInventory = false;
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
                if (this.ConnectionMode == "socket")
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
                else if (this.ConnectionMode == "raw")
                {
                    this.ReaderSettings = new ReaderSettings();
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
            if (this.ConnectionMode == "socket")
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

                if (this.ReaderSettings.DataOutput.Socket.Enable)
                {
                    if (this.SocketClient == null)
                        this.SocketClient = new SocketClient(this);
                    this.SocketClient.Connect();
                }

                #endregion
            }
            else if (this.ConnectionMode == "raw")
            {
                #region RFID

                if (this.ReaderSettings.RFID != null)
                {
                    rawMode.SendCommand(RawMode.Command.SetMode(this.ReaderSettings.RFID.ModeAndSession.SearchMode));
                    // Set RF Profile
                    rawMode.SendCommand(RawMode.Command.SetProfile(this.ReaderSettings.RFID.ModeAndSession.Profile));
                    // Set Session
                    rawMode.SendCommand(RawMode.Command.SetSession(this.ReaderSettings.RFID.ModeAndSession.Session));
                    // Set Tag Population
                    rawMode.SendCommand(RawMode.Command.SetTagPopulation(this.ReaderSettings.RFID.ModeAndSession.PopulationEstimate));

                    ushort idxAntenna = 1;
                    foreach (var antenna in this.ReaderSettings.RFID.Antennas)
                    {
                        if (!antenna.Enable) antenna.TxPowerCdbm = 0;
                        rawMode.SendCommand(RawMode.Command.SetTxtPower(idxAntenna, antenna.TxPowerCdbm));
                        idxAntenna++;
                    }

                    rawMode.SendCommand(RawMode.Command.EnableReaderName(this.ReaderSettings.RFID.ReportFields.ReaderName));
                    rawMode.SendCommand(RawMode.Command.EnableTimestamp(this.ReaderSettings.RFID.ReportFields.Timestamp));
                    rawMode.SendCommand(RawMode.Command.EnableRssi(this.ReaderSettings.RFID.ReportFields.Rssi));
                    rawMode.SendCommand(RawMode.Command.EnablePhase(this.ReaderSettings.RFID.ReportFields.Phase));
                    rawMode.SendCommand(RawMode.Command.EnableChannel(this.ReaderSettings.RFID.ReportFields.Channel));
                    rawMode.SendCommand(RawMode.Command.EnableFastId(this.ReaderSettings.RFID.ReportFields.FastId));
                }

                #endregion
            }
        }

        /// <summary>
        /// Set GPO state
        /// </summary>
        /// <param name="port"></param>
        /// <param name="state"></param>
        public void SetGpo(int port, bool state)
        {
            SocketClient.SendData("{\"gpio\":" + port + ",\"mode\":1,\"status\":" + (state == true ? 1 : 0) + ",\"time\":0}");
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
            if (!IsRunningInventory) return;

            if (TagsReportedHandler != null)
                TagsReportedHandler(this, report);
        }

        /// <summary>
        /// Event beacon reported
        /// </summary>
        /// <param name="report"></param>
        public virtual void OnBeaconsReported(string report)
        {
            if (!IsRunningInventory) return;

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

        /// <summary>
        /// Event gpi reported
        /// </summary>
        /// <param name="report"></param>
        public virtual void OnGpiReported(string report)
        {
            if (!IsRunningInventory) return;

            if (GpiReportedHandler != null)
                GpiReportedHandler(this, report);
        }
    }

    /// <summary>
    /// Reader Profiles
    /// </summary>
    public class Profile
    {
        public static uint AUTO
        {
            get { return 0; }
        }

        public static uint DRM_FCC
        {
            get { return 1; }
        }

        public static uint DRM_ETSI
        {
            get { return 2; }
        }

        public static uint FAST_MODE
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
        public static uint DualTarget
        {
            get { return 0; }
        }

        public static uint SingleTarget_A_TO_B
        {
            get { return 1; }
        }

        public static uint SingleTarget_B_TO_A
        {
            get { return 2; }
        }

        public static uint TagFocus
        {
            get { return 3; }
        }
    }

    /// <summary>
    /// Reader Filters
    /// </summary>
    public class Filter
    {
        public static uint TARGET_SESSION_S0
        {
            get { return 0; }
        }

        public static uint TARGET_SESSION_S1
        {
            get { return 1; }
        }

        public static uint TARGET_SESSION_S2
        {
            get { return 2; }
        }

        public static uint TARGET_SESSION_S3
        {
            get { return 3; }
        }

        public static uint TARGET_SL_FLAG
        {
            get { return 4; }
        }

        public static uint TARGET_RFU_1
        {
            get { return 5; }
        }

        public static uint TARGET_RFU_2
        {
            get { return 6; }
        }

        public static uint TARGET_RFU_3
        {
            get { return 7; }
        }
    }

    /// <summary>
    /// BLE Scan
    /// </summary>
    public class BleScan
    {
        public static uint Fast
        {
            get { return 0; }
        }

        public static uint Normal
        {
            get { return 1; }
        }

        public static uint Slow
        {
            get { return 2; }
        }
    }
}
