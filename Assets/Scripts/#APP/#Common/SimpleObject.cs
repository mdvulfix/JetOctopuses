using System;
using System.Collections;
using System.Collections.Generic;
using APP;
using UnityEngine;


[Serializable]
public abstract class SimpleObject
{

    private Configurator m_Configurator;
    private Initializator m_Initializator;

    public bool Debug { get; private set; } = true;


    // CONFIGURE //
    public virtual void Configure() =>
        Configure(config: null);

    public virtual void Configure(IConfig config) =>
        Configure(config: config, param: null);

    public virtual void Configure (IConfig config, params object[] param)
    {
        if(config != null)
        {
            m_Config = (SceneControllerConfig)config;
        }          
            
        if(param.Length > 0)
        {
            foreach (var obj in param)
            {   
                if(obj is object)
                Send("Param is not used", LogFormat.Worning);
            }
        }          
            
        OnConfigured();
    }


    
    public virtual void Init() 
    { 

        
        OnInitialized();
    }
    
    public virtual void Dispose()
    { 

        
        OnDisposed();
    }
    
    
    
    
    
    
    
    
    


    
    protected string Send(Message message) =>
        Messager.Send(this, Debug, message);


}




public class Configurator<TConfig> 
where TConfig: IConfig
{
    public Configurator()
    {

    }

    public bool IsConfigured {get; private set; }

    public TConfig Config {get; private set; }

    public event Action Configured;

    public virtual void Configure (IConfig config, params object[] param)
    {
        if(config != null)
        {
            Config = (TConfig)config;
        }          
            
        if(param.Length > 0)
        {
            foreach (var obj in param)
            {   
                if(obj is object)
                Send("Param is not used", LogFormat.Worning);
            }
        }          
            
        OnConfigured();
    }


    private bool Check()
    {
        return true;
    }

    // CALLBACK //
    private void OnConfigured()
    {
        Send($"Configuration successfully completed!");
        IsConfigured = true;
        Configured?.Invoke();
    }

}

public class Initializator
{
    private bool m_IsConfigurable;
    private bool m_IsConfigured;
    private bool m_IsInitialized;

    public Initializator() {}
    public Initializator(bool isConfigurable, ref bool isConfigured)
    {
        m_IsConfigurable = isConfigurable;
        m_IsConfigured = isConfigured;
    }

    public bool IsInitialized { get => m_IsInitialized; }

    public event Action Initialized;
    public event Action Disposed;
    public event Action ConfigureRequired;
    public event Action<Message> Message;

    
    public void Init(Action action) 
    { 
        if(Check() == false)
            return;
        
        action.Invoke();
       
        m_IsInitialized = true;
        Send("Initialization successfully completed!");
        Initialized?.Invoke();
    }
    
    public void Dispose(Action action)
    { 
        action.Invoke();
        
        m_IsInitialized = false;
        Send("Dispose process successfully completed!");
        Disposed?.Invoke();
    }
    
    
    private bool Check()
    {
        if(m_IsConfigurable == true && m_IsConfigured == false)
        {
            Send("Instance was not configured. Initialization was failed!", LogFormat.Worning);
            ConfigureRequired?.Invoke();
            return false;
        }

        if(m_IsInitialized == true)
        {
            Send("Instance was already initialized. Current initialization was aborted!", LogFormat.Worning);
            return false;
        }
                
        return true;
    }

    private void Send(string text, LogFormat logFormat = LogFormat.None) =>
        Message?.Invoke(new Message(text, logFormat));
}




