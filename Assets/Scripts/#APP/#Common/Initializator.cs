using System;
using APP;

public class Initializator : IInitializator
{
    private IConfigurator m_Configurator;
    private bool m_IsConfigurable;
    private bool m_IsConfigured;
    private bool m_IsInitialized;

    public Initializator() { }
    public Initializator(IConfigurator configurator)
    {
        if(configurator != null)
        {
            m_Configurator = configurator;
            m_IsConfigurable = true;
            m_IsConfigured = m_Configurator.IsConfigured;
        }   
        else
        {
            m_IsConfigurable = false;
        }
    }

    public bool IsInitialized { get => m_IsInitialized; }

    public event Action Initialized;
    public event Action Disposed;
    public event Action ConfigureRequired;
    public event Action<Message> Message;


    public void Init(Action action)
    {
        if (Check() == false)
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
        Send("Dispose successfully completed!");
        Disposed?.Invoke();
    }


    private bool Check()
    {
        if (m_IsConfigurable == true && m_IsConfigured == false)
        {
            Send("The instance is not configured. Initialization was aborted!", LogFormat.Worning);
            ConfigureRequired?.Invoke();
            return false;
        }

        if (m_IsInitialized == true)
        {
            Send("The instance was already initialized. Current initialization has been aborted!", LogFormat.Worning);
            return false;
        }

        return true;
    }

    private void Send(string text, LogFormat logFormat = LogFormat.None) =>
        Message?.Invoke(new Message(text, logFormat));
}

public interface IInitializator
{
    bool IsInitialized { get; }

    event Action Initialized;
    event Action Disposed;
    event Action ConfigureRequired;
    event Action<Message> Message;

    void Init(Action action);
    void Dispose(Action action);
    
}

