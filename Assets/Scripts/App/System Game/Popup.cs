using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace App.Game
{
   public class Popup : IUpdatable
   {
      private PopupConfig m_Config;

      [SerializeField] private GameObject m_GameObject;
      [SerializeField] private Text m_Text;

      private Transform m_Parent;
      private Vector3 m_Position;
      private string m_Caption;
      private float m_LifeTime = 5;
      private float m_DisappearSpeed = 1;
      private float m_MoveSpeed = 2;
      private Color m_Color = Color.green;

      public event Action<Popup> Disappeared;

      public virtual void Configure(params object[] args)
      {
         if (args.Length > 0)
         {
            foreach (var arg in args)
            {
               if (arg is PopupConfig)
               {
                  m_Config = (PopupConfig)args[0];
                  m_Parent = m_Config.Parent;
                  m_Position = m_Config.Position;
                  m_Caption = m_Config.Caption;
               }
            }
         }

      }

      public virtual void Init()
      {
         m_GameObject = new GameObject("Popup");
         m_GameObject.transform.position = m_Position;
         m_GameObject.transform.parent = m_Parent;

         m_Text = m_GameObject.AddComponent<Text>();
         m_Text.text = m_Caption;
         m_Text.color = m_Color;
      }

      public virtual void Dispose()
      {
         GameObject.Destroy(m_GameObject);
      }

      public virtual void Update()
      {
         //m_GameObject.transform.position += new Vector3(0, m_PopupMoveSpeed) * Time.deltaTime;
         m_LifeTime -= Time.deltaTime;

         if (m_LifeTime <= 0)
         {
            m_LifeTime = 0;
            //m_Color.a -= m_DisappearSpeed * Time.deltaTime;
            //m_Text.color = m_Color;

            if (m_Color.a <= 0)
               Disappeared?.Invoke(this);
         }



      }

   }



   public class PopupConfig
   {
      public PopupConfig(Transform parent, string caption)
      {
         Position = parent.position;
         Caption = caption;
         Parent = parent;
      }

      public Vector3 Position { get; private set; }
      public string Caption { get; private set; }
      public Transform Parent { get; private set; }
   }
}

