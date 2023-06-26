using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace App.Game
{
    public class EnemyUI : MonoBehaviour
    {

        private List<Popup> m_Popups;

        public virtual void Configure(params object[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (arg is IConfig)
                    {
                        //m_Config = (Config) args[0];

                    }

                }
            }

            m_Popups = new List<Popup>();

        }

        public virtual void Init()
        {

        }

        public virtual void Dispose()
        {

        }


        public void PopupShowDamage(int damage)
        {
            var popupConfig = new PopupConfig(transform, damage.ToString());
            var popup = new Popup();
            popup.Disappeared += OnPopupDisappeared;
            popup.Configure(popupConfig);
            popup.Init();

            m_Popups.Add(popup);
        }

        public void OnPopupDisappeared(Popup popup)
        {
            //if(m_Popups.Contains(popup))
            //m_Popups.Remove(popup);

            //popup.Dispose();
            popup.Disappeared -= OnPopupDisappeared;
        }

        private void Update()
        {
            if (m_Popups.Count > 0)
                foreach (var popup in m_Popups)
                    popup.Update();

        }
    }
}