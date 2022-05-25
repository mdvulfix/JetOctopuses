using System;
using UnityEngine;

[Serializable]
public class ScreenLevel : ScreenModel<ScreenLevel>, IScreen
{
    [SerializeField] private ButtonLevelPause m_LevelPause;

    protected override void Init()
    {
        var buttons = new IButton[]
        {
            m_LevelPause
        };

        Configure(new ScreenConfig(this, buttons));
        base.Init();
    }

}