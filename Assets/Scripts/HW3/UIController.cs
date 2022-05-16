using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace HW3
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button _buttonStartServer;
        [SerializeField] private Button _buttonShutDownServer;
        [SerializeField] private Button _buttonConnectClient;
        [SerializeField] private Button _buttonDisconnectClient;
        [SerializeField] private Button _buttonSendMessage;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TextField _textField;
        
        [SerializeField] private Server _server;
        [SerializeField] private Client _client;
        
        [SerializeField] private TMP_InputField _nameField;
        
        
        private void Start()
        {
            _buttonStartServer.onClick.AddListener(() => StartServer());
            _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
            _buttonConnectClient.onClick.AddListener(() => Connect());
            _buttonDisconnectClient.onClick.AddListener(() => Disconnect());
            _buttonSendMessage.onClick.AddListener(() => SendMessage());
            _nameField.onSubmit.AddListener(SetUserName);
            _nameField.gameObject.SetActive(true);
            SetActiveToUI(false);
            
            _client.onMessageReceive += ReceiveMessage;
            
        }

        private void SetActiveToUI(bool state)
        {
            _buttonSendMessage.gameObject.SetActive(state);
            _inputField.gameObject.SetActive(state);
            _textField.gameObject.SetActive(state);
        }

        private void SetUserName(string uName)
        {
            _client.UserName = uName;
            _nameField.gameObject.SetActive(false);
            _nameField.onEndEdit.RemoveAllListeners();
            SetActiveToUI(true);
        }

        private void StartServer()
        {
            _server.StartServer();
            
        }

        private void ShutDownServer()
        {
            _server.ShutDownServer();
        }
        
        private void Connect()
        {
            if (_client.UserName == "")
            {
                Debug.Log("Enter name first");
                return;
            }

            _client.Connect();
            
        }
        private void Disconnect()
        {
            _client.Disconnect();
        }
        private void SendMessage()
        {
            if (_inputField.text == "") return;
            _client.SendMessage(_inputField.text);
            _inputField.text = "";
        }
        public void ReceiveMessage(object message)
        {
            _textField.ReceiveMessage(message);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                SendMessage();
        }

        private void OnDestroy()
        {
            _buttonStartServer.onClick.RemoveAllListeners();
            _buttonShutDownServer.onClick.RemoveAllListeners();
            _buttonConnectClient.onClick.RemoveAllListeners();
            _buttonDisconnectClient.onClick.RemoveAllListeners();
            _buttonSendMessage.onClick.RemoveAllListeners();
            _nameField.onEndEdit?.RemoveAllListeners();
            _client.onMessageReceive -= ReceiveMessage;
        }
    }
}