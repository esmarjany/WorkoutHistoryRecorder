using Samsung.Sap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen;

namespace WorkoutHistoryRecorder.WatchApp
{
    internal class SAPService
    {
        private Agent Agent;
        private Connection Connection;
        private Peer Peer;
        private Channel ChannelId;

        public event EventHandler<DataReceivedEventArgs> DataReceived = delegate { };
        public async Task<bool> Connect()
        {
            //return false;
            try
            {
                Agent = await Agent.GetAgent("/example/companion");
                var peers = await Agent.FindPeers();
                ChannelId = Agent.Channels.First().Value;
                if (peers.Count() > 0)
                {
                    Console.WriteLine("Peer found");
                    Peer = peers.First();
                    Connection = Peer.Connection;
                    Connection.DataReceived -= DataReceived;
                    Connection.DataReceived += DataReceived;
                    await Connection.Open();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("MyApp", "Error: " + ex.Message);
                return false;
            }
        }

        public void SendText(string message)
        {
            if (Peer != null)
            {
                Connection.Send(ChannelId, Encoding.Unicode.GetBytes(message));
            }
            else
            {
                throw new Exception("Connect to phone first");
            }
        }
        public void SendBytes(byte[] message)
        {
            if (Peer != null)
            {
                Connection.Send(ChannelId, message);
            }
            else
            {
                throw new Exception("Connect to phone first");
            }
        }
    }
}
