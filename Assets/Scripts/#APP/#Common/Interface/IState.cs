using System.Threading.Tasks;

namespace APP
{
    public interface IState
    {
        
        Task Enter();
        Task Fail();
        Task Run();
        Task Exit();
    }

}