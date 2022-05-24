using UnityEngine;
using UnityEngine.UI;

public class ScreenController : IConfigurable
{

    private CachHandler m_CachHandler;

    private IScreen m_ScreenActive;

    public ScreenController(IConfig config)
    {
        Configure(config);
    }

    public void Configure(IConfig config)
    {
        m_CachHandler = new CachHandler();
    }

    public void Init()
    {
        Activate<ScreenLoading>();
    }

    public void Dispose()
    {

    }



    public void Activate<TScreen>(bool animate = true)
        where TScreen: Cachable, IScreen
    {
        
        if(m_ScreenActive != null && m_ScreenActive.GetType() == typeof(TScreen))
            return;

        if(m_CachHandler.Get<TScreen>(out var screen))
        {

            screen.gameObject.SetActive(true);


            if(animate)
                Animate();
        }

    }

    protected void Animate(bool animate = true)
    {
        
          
    }







}

public class ScreenControllerConfig: IConfig
{
    public ScreenControllerConfig()
    {

    }
}

public struct ButtonClickedEventArgs: IEventArgs
{
    public Button Button { get; }
    public IScreen Screen { get; }
    

    public ButtonClickedEventArgs(IScreen screen, Button button)
    {
        Screen = screen;
        Button = button;
    }
}

public interface IEventArgs
{ 

}