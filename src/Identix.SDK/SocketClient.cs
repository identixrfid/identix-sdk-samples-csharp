using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Identix.SDK
{
    public class SocketClient
    {
        private Thread thReceive;
        private Reader Reader { get; set; }
        private TcpClient ClientSocket { get; set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        private bool IsRunning { get; set; }

        /// <summary>
        /// SocketClient builder that always need reader parameter
        /// </summary>
        /// <param name="reader"></param>
        public SocketClient(Reader reader)
        {
            Reader = reader;
            Ip = reader.Address.Replace("https://", "").Replace("http://", "");
            Port = reader.ReaderSettings.DataOutput.Socket.Port;
            IsRunning = false;
        }

        /// <summary>
        /// Connect reader to the socket server
        /// </summary>
        public void Connect()
        {
            try
            {
                if (ClientSocket == null)
                    ClientSocket = new TcpClient();

                ClientSocket.Connect(this.Ip, this.Port);

                IsRunning = true;
                if (thReceive == null)
                {
                    thReceive = new Thread(ReceiveData);
                    thReceive.IsBackground = true;
                    thReceive.Start();
                }
            }
            catch (Exception)
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Disconnect reader to the socket server
        /// </summary>
        public void Disconnect()
        {
            try
            {
                IsRunning = false;

                if (ClientSocket != null)
                {
                    if (ClientSocket.Connected)
                        ClientSocket.Close();

                    if (thReceive != null)
                    {
                        if (thReceive.IsAlive)
                        {
                            thReceive.Abort();
                        }
                        thReceive = null;
                    }
                }
            }
            catch (Exception)
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Receive data from socket server
        /// </summary>
        private void ReceiveData()
        {
            try
            {
                NetworkStream nwStream = ClientSocket.GetStream();
                byte[] bytesToRead = new byte[ClientSocket.ReceiveBufferSize];

                while (IsRunning)
                {
                    int bytesRead = nwStream.Read(bytesToRead, 0, ClientSocket.ReceiveBufferSize);
                    string data = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);

                    data = data.Replace("\\r", "|").Replace("\\n", "|").Replace("\n", "|").Replace("\r", "|").Replace("||", "|");

                    string[] arrData = data.Split('|');

                    foreach (var tagData in arrData)
                    {
                        if (!string.IsNullOrEmpty(tagData))
                        {
                            if (tagData.Contains("epc"))
                                Reader.OnTagsReported(tagData);
                            else if (tagData.Contains("status"))
                                Reader.OnHeartbeatReported(tagData);
                            else if (tagData.Contains("mac"))
                                Reader.OnBeaconsReported(tagData);
                        }
                    }
                }
            }
            catch
            {
            }
        }
    }
}