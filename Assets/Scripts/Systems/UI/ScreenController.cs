using UnityEngine;
using UnityEngine.UI;

public class ScreenController
{
    private IScreen m_ScreenActive;
    
    public ScreenController()
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