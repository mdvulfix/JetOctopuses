using System;
/*
public class SignalMenuPlay: 
    SignalModel<SignalMenuPlay, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuPlay()
    {
        var signalInfo = new SignalInfo(this, PlayerAction.MenuPlay);
        Configure(new SignalConfig<SignalInfo>(signalInfo));
        base.Init();
    }
}

public class SignalMenuMain: 
    SignalModel<SignalMenuMain, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuMain()
    {
        var signalInfo = new SignalInfo(this, PlayerAction.MenuMain);
        Configure(new SignalConfig<SignalInfo>(signalInfo));
        base.Init();
    }

}


public class SignalMenuOptions: 
    SignalModel<SignalMenuOptions, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuOptions(SignalConfig<SignalInfo> info)
    {
        Configure(info);
        base.Init();
    }

}

public class SignalMenuExit: 
    SignalModel<SignalMenuExit, SignalInfo>, 
    ISignal<SignalInfo>
{
    public SignalMenuExit(SignalConfig<SignalInfo> info)
    {
        Configure(info);
        base.Init();
    }

}


public class SignalMenuLevel:
    SignalModel<SignalMenuLevel, SignalLevelInfo>,
    ISignal<SignalLevelInfo>
{

    public SignalMenuLevel(SignalConfig<SignalLevelInfo> info)
    {
        Configure(info);
        base.Init();
    }
}

*/

namespace APP
{
    public interface ISignal
    {
        void Call ();

    }
}
