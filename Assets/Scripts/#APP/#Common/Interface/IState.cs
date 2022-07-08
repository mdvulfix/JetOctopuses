using System;
using System.Threading.Tasks;

namespace APP
{
    public interface IState: IConfigurable, ICacheable
    {
        
        event Action<IScene> SceneRequied;
        event Action<IState> StateRequied;
        
        Task Enter();
        Task Fail();
        Task Run();
        Task Exit();
    }

}