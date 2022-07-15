using System;
using System.Collections;
using System.Collections.Generic;
using APP;
using UnityEngine;


[Serializable]
public abstract class SimpleObject
{
    private IConfigurator m_Configurator;
    private IInitializator m_Initializator;

    
    public bool Debug { get; private set; } = true;


    /*
    public virtual void Configure() =>
        Configure<TConfig>(T instance, TConfig config, params object[] param)
    
    public virtual void Configure(T instance) =>
        Configure(instance: instance, config: null);

    public virtual void Configure(T instance, IConfig config) =>
        Configure(instance: instance, config: config, param: null);
    */
    
    public virtual void Configure<TConfig>(TConfig config, params object[] param)
    {
        m_Configurator = new Configurator();
        //m_Configurator.Configure(() => Set(instance, config, param));

        m_Initializator = new Initializator(m_Configurator);
    
    }
    
    public virtual void Init() {}
    
    public virtual void Dispose() {}

    public virtual void Subscribe() 
    { 
        m_Configurator.Message += OnGetMessage;
        m_Initializator.Message += OnGetMessage;

    }
    
    public virtual void Unsubscribe()
    { 
        m_Configurator.Message -= OnGetMessage;
        m_Initializator.Message -= OnGetMessage;
        
    }


    public abstract void Set<TConfig>(TConfig config, params object[] param);

    
    private void OnEnable() =>
        m_Initializator.Init(() => Init());

    private void OnDisable() =>
        m_Initializator.Dispose(() => Dispose());
    
    private void OnGetMessage(Message message) =>
        Send(new Message(this, message.Text, message.LogFormat));

    protected Message Send(Message message) =>
        Messager.Send(Debug, message);

}



