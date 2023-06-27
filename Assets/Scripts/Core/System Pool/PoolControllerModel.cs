using Core;

namespace Core.Pool
{
   public abstract class PoolControllerModel : ModelController
   {

      private IPool m_Pool;

      public override void Configure(params object[] args)
      {

         base.Configure();
      }

      public override void Init()
      {

         base.Init();
      }

      public override void Dispose()
      {
         base.Dispose();
      }


      public void Push(IPoolable poolable)
      {
         poolable.Dispose();
         m_Pool.Push(poolable);
      }

      public bool Pop(out IPoolable poolable)
      {
         if (m_Pool.Pop(out poolable))
         {
            poolable.Init();
            return true;
         }

         return false;
      }

      public bool Peek(out IPoolable poolable)
      {
         if (m_Pool.Peek(out poolable))
         {
            poolable.Init();
            return true;
         }

         return false;
      }

   }

   public class PoolControllerConfig : IConfig
   {
      public IPool Pool { get; private set; }

      public PoolControllerConfig(IPool pool)
      {
         Pool = pool;
      }
   }


   public interface IPoolController : IController
   {
      bool Peek(out IPoolable poolable);
      bool Pop(out IPoolable poolable);
      void Push(IPoolable poolable);
   }
}

