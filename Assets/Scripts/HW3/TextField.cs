using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HW3
{
    public class TextField : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textObject;
        [SerializeField] private Scrollbar scrollbar;
        
        private List<string> messages = new List<string>();
        private void Start()
        {
            scrollbar.onValueChanged.AddListener((float value) => UpdateText());
        }
        public void ReceiveMessage(object message)
        {
            messages.Add(message.ToString());
            var value = (messages.Count - 1) * scrollbar.value;
            scrollbar.value = Mathf.Clamp(value, 0, 1);
            UpdateText();
        }
        private void UpdateText()
        {
            var text = "";
            var index = (int)(messages.Count * scrollbar.value);
            
            for (var i = index; i < messages.Count; i++)
            {
                text += messages[i] + "\n";
            }
            textObject.text = text;
        }

        private void OnDestroy()
        {
            scrollbar.onValueChanged.RemoveAllListeners();
        }
    }
}