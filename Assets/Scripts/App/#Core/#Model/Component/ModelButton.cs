using System;
using UnityEngine;
using UnityEngine.UI;
using UComponent = UnityEngine.Component;



namespace Core
{
    public abstract class ModelButton : Button
    {
        public string Name => this.GetName();
        public Type Type => this.GetType();

        public event Action<ISignal> Clicked;

        public abstract void OnClick();


        public void Subscribe() =>
            onClick.AddListener(() => Click());

        public void Unsubscribe() =>
            onClick.RemoveListener(() => Click());


        public abstract void Click();


    }

}



