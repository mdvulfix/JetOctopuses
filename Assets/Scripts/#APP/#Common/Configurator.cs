using System;
using APP;

public class Configurator : IConfigurator
{
    private bool m_IsConfigured;

    public Configurator() { }

    public bool IsConfigured { get => m_IsConfigured; }

    public IConfig Config { get; private set; }

    public event Action Configured;
    public event Action<Message> Message;



    // CONFIGURE //
    public virtual void Configure<T, TConfig>(T instance, TConfig config, params object[] param)
    where TConfig: IConfig
    {
        if (Check() == false)
            return;

        if (config != null)
        {
            Config = (IConfig)config;
            


        
        
        }

        if (param.Length > 0)
        {
            foreach (var obj in param)
            {
                if (obj is object)
                    Send(new Message("Param is not used", LogFormat.Worning));
            }
        }

        m_IsConfigured = true;
        Send(new Message("Configuration successfully completed!"));
        Configured?.Invoke();
    }



    private static bool Check(IConfigurable configurable)
    {
        if (configurable.IsConfigured == true)
        {
            Send(new Message(configurable, "The instance was already configured. The current setup has been aborted!", LogFormat.Worning));
            return false;
        }

        return true;
    }

    private bool Check()
    {
        if (m_IsConfigured == true)
        {
            Send(new Message("The instance was already configured. The current setup has been aborted!", LogFormat.Worning));
            return false;
        }

        return true;
    }


    /*
    private static void Send(object sender, string text, LogFormat logFormat = LogFormat.None)
    {
        Messager.Send(true, new Message(sender, text, logFormat));
        //Message?.Invoke(new Message(text, logFormat));
    }
    */

    private static string Send(Message message) =>
        Messager.Send(true, message);
}


public interface IConfigurator
{
    bool IsConfigured { get; }
    IConfig Config { get; }

    event Action Configured;
    event Action<Message> Message;

    void Configure<T, TConfig>(T instance, TConfig config, params object[] param)
    where TConfig: IConfig;

}