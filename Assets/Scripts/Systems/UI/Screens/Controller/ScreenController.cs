
public class ScreenController : Controller, IConfigurable
{

    private IScreen m_ScreenActive;

    public ScreenController(IConfig config) =>
        Configure(config);


    public void Configure(IConfig config)
    {

    }

    protected override void Init()
    {
        Activate<ScreenLoading>();
        base.Init();
    }

    protected override void Dispose()
    {
        base.Dispose();
    }



    public void Activate<TScreen>(bool animate)
        where TScreen: IScreen
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