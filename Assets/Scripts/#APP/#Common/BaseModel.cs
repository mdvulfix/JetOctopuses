using System;
using System.Collections;
using System.Collections.Generic;
using APP;
using UnityEngine;



public abstract class BaseModel
{

}

public abstract class ConfigurableModel: BaseModel
{
    IConfigurator m_Configurator;
    IProgram m_ConfigureProgram;
    IProgram m_InitProgram;
    IProgram m_DisposeProgram;

    public virtual void Configure(IConfig config, params object[] param) => 
        m_Configurator.Configure(() => m_ConfigureProgram.Execute(), config, param);

    public virtual void Init() => 
        m_Configurator.Init(() => m_InitProgram.Execute());

    public virtual void Dispose() => 
        m_Configurator.Dispose(() => m_DisposeProgram.Execute());



}

public interface IProgram
{
    void Execute();
}