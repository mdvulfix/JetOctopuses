using System;
using UnityEngine;
using Core;

namespace App.Test
{
    public class Test : MonoBehaviour, IMessager
    {
        private bool m_Debug = true;

        public event Action<IMessage> Message;

        // MESSAGE //
        public IMessage Send(string text, LogFormat logFormat = LogFormat.None) =>
            Send(new Message(this, text, logFormat));

        public IMessage Send(IMessage message)
        {
            Message?.Invoke(message);
            return Messager.Send(m_Debug, this, message.Text, message.LogFormat);
        }

        // CALLBACK //
        public void OnMessage(IMessage message) =>
            Send($"{message.Sender}: {message.Text}", message.LogFormat);


        public virtual void Awake() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void Start() { }
        public virtual void Update() { }

        public virtual void OnGUI() { }

    }

}

namespace App
{
    public interface ITest
    {


    }


}