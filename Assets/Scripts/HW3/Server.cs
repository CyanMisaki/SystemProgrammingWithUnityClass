using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace HW3
{
    public class Server : MonoBehaviour
    {
        private const int MAX_CONNECTIONS = 10;
        private int _port = 5805;
        private int _hostID;
        private int _reliableChannel;
        private bool _isStarted = false;
        private byte _error;
        private Dictionary<int, string> _connectionIDs = new Dictionary<int, string>();


        public void SendMessage(string message, int connectionID)
        {
            var buffer = Encoding.Unicode.GetBytes(message);
            NetworkTransport.Send(_hostID, connectionID, _reliableChannel, buffer, message.Length * sizeof(char),
                out _error);
            if ((NetworkError)_error != NetworkError.Ok) Debug.Log((NetworkError)_error);
        }

        public void SendMessageToAll(string message)
        {
            foreach (var id in _connectionIDs)
            {
                SendMessage(message, id);
            }
        }

        public void StartServer()
        {
            NetworkTransport.Init();
            var cc = new ConnectionConfig();
            _reliableChannel = cc.AddChannel(QosType.Reliable);
            var topology = new HostTopology(cc, MAX_CONNECTIONS);
            _hostID = NetworkTransport.AddHost(topology, _port);
            _isStarted = true;
        }

        public void ShutDownServer()
        {
            if (!_isStarted) return;
            NetworkTransport.RemoveHost(_hostID);
            NetworkTransport.Shutdown();
            _isStarted = false;
        }

        private void Update()
        {
            if (!_isStarted) return;
            int recHostId;
            int connectionId;
            int channelId;
            var recBuffer = new byte[1024];
            var bufferSize = 1024;
            int dataSize;
            var recData = NetworkTransport.Receive(out recHostId, out connectionId, out
                channelId, recBuffer, bufferSize, out dataSize, out _error);
           
            while (recData != NetworkEventType.Nothing)
            {
                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        break;
                    case NetworkEventType.ConnectEvent:
                        _connectionIDs.Add(connectionId,"");
                        //SendMessageToAll($"Player {connectionId} has connected.");
                        Debug.Log($"LOGServ-Player {connectionId} has connected.");
                        break;
                    case NetworkEventType.DataEvent:
                        var message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                        if (_connectionIDs[connectionId] == "")
                            _connectionIDs[channelId] = message;
                        
                        SendMessageToAll($"Player {_connectionIDs[channelId]}: {message}");
                        Debug.Log($"LOGServ-Player {_connectionIDs[channelId]}: {message}");
                        break;
                    case NetworkEventType.DisconnectEvent:
                        _connectionIDs.Remove(connectionId);
                        SendMessageToAll($"Player {_connectionIDs[channelId]} has disconnected.");
                        Debug.Log($"LOGServ-Player {_connectionIDs[channelId]} has disconnected.");
                        break;
                    case NetworkEventType.BroadcastEvent:
                        break;
                }
                
                recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                    bufferSize, out dataSize, out _error);
            }
        }

        private void OnDestroy()
        {
            _connectionIDs.Clear();
            ShutDownServer();
        }
    }
}