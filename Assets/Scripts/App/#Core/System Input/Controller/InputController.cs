using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

using Core;


public class InputController : ModelConfigurable, IInputController
{

    private bool m_isDebug = true;

    private InputControllerConfig m_Config;

    public string Label => "UIController";


    public override void Init(params object[] args)
    {
        var config = (int)Params.Config;

        if (args.Length > 0)
            try { m_Config = (InputControllerConfig)args[config]; }
            catch { Debug.LogWarning($"{this}: config was not found. Configuration failed!"); return; }

    }



    public void HandleKeyInput(KeyCode key)
    {

    }
}

public interface IInputController
{
    void HandleKeyInput(KeyCode key);
}

public struct InputControllerConfig : IConfig
{

}