using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController: IController
{
    
    private static IScreen m_ActiveScreen;
    
    private ScreenProvider m_ScreenProvider;

    public ScreenController()
    {
        Init();
    }

    public void Init()
    {
        m_ScreenProvider = new ScreenProvider();
    }

    public void Dispose()
    {

    }

    private T GetScreen<T>() 
        where T: class, IScreen
    {
        if(m_ScreenProvider.Get<T>(out T screen))
            return (T)screen;

        return null;
    }
    
    public void AcrivateScreen<T>(bool active) 
        where T: MonoBehaviour, IScreen
    {
        T screen = GetScreen<T>();
        screen.gameObject.SetActive(active);
        m_ActiveScreen = screen;
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