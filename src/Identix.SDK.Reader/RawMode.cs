using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO.Ports;
using System.Text;

namespace Identix.SDK
{
    public class RawMode
    {
        private Reader Reader { get; set; }
        private SerialPort serialPort = null;
        private StringBuilder message = new StringBuilder();

        public RawMode(Reader reader)
        {
            this.Reader = reader;
        }

        /// <summary>
        /// Configure serial port
        /// </summary>
        /// <param name="comPort"></param>
        public void SetSerialPort(string comPort)
        {
            if (serialPort == null)
            {
                serialPort = new SerialPort(comPort, 115200, Parity.None, 8, StopBits.One);
            }
        }

        /// <summary>
        /// Open serial port
        /// </summary>
        /// <returns></returns>
        public bool OpenSerialPort()
        {
            try
            {
                if (!serialPort.IsOpen) serialPort.Open();
                serialPort.DiscardInBuffer();

                serialPort.DataReceived += new SerialDataReceivedEventHandler(PortReceivedData);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Close serial port
        /// </summary>
        /// <returns></returns>
        public bool CloseSerialPort()
        {
            try
            {
                serialPort.DataReceived -= new SerialDataReceivedEventHandler(PortReceivedData);
                if (serialPort.IsOpen) serialPort.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reveived data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PortReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            if (!serialPort.IsOpen) return;

            try
            {
                string buffer = serialPort.ReadExisting();

                byte[] bytes = Encoding.UTF8.GetBytes(buffer.ToString());

                foreach (var s in bytes)
                {
                    if (s > 0)
                    {
                        if (((char)s == '\r' || (char)s == '\n') && message.Length > 0)
                        {
                            Reader.OnTagsReported(message.ToString());

                            message.Clear();
                        }
                        else
                        {
                            if (((char)s != '\r' && (char)s != '\n'))
                            {
                                message.Append((char)s);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Send a serial port command
        /// </summary>
        /// <param name="command"></param>
        public void SendCommand(string command)
        {
            if (!serialPort.IsOpen) return;

            serialPort.Write(command);

            System.Threading.Thread.Sleep(250);
        }

        /// <summary>
        /// Support Commands
        /// </summary>
        public class Command
        {
            public static string Start()
            {
                JObject command = new JObject(new JProperty("inventory", true));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string SetMode(uint readerMode = 0)
            {
                JObject command = new JObject(new JProperty("mode", readerMode));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string SetSession(uint readerSession = 0)
            {
                JObject command = new JObject(new JProperty("session", readerSession));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string SetProfile(uint readerProfile = 0)
            {
                JObject command = new JObject(new JProperty("profile", readerProfile));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string SetTagPopulation(uint poplation = 4)
            {
                JObject command = new JObject(new JProperty("population", poplation));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string SetTxtPower(ushort antenna, int txPw)
            {
                JObject command = new JObject(new JProperty("tx" + antenna, txPw));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableRssi(bool enable)
            {
                JObject command = new JObject(new JProperty("rssi", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnablePhase(bool enable)
            {
                JObject command = new JObject(new JProperty("phase", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableChannel(bool enable)
            {
                JObject command = new JObject(new JProperty("channel", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableFastId(bool enable)
            {
                JObject command = new JObject(new JProperty("fast_id", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableReaderName(bool enable)
            {
                JObject command = new JObject(new JProperty("reader_name", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableTimestamp(bool enable)
            {
                JObject command = new JObject(new JProperty("timestamp", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableBuzzer(bool enable)
            {
                JObject command = new JObject(new JProperty("buzzer", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string EnableLed(bool enable)
            {
                JObject command = new JObject(new JProperty("led", enable));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }

            public static string Stop()
            {
                JObject command = new JObject(new JProperty("inventory", false));

                var jsonCommand = JsonConvert.SerializeObject(command) + "\r\r";

                return jsonCommand;
            }
        }
    }
}
