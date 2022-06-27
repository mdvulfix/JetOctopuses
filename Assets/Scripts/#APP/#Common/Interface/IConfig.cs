using System;

namespace APP
{
    public interface IConfig
    {
        InstanceInfo InstanceInfo {get; }
    }


    public struct InstanceInfo
    {
        public Type ObjType {get; private set;}
        public object Obj {get; private set;}
        
        public InstanceInfo(object obj)
        {
            ObjType = obj.GetType();
            Obj = obj;
        }
        
        public InstanceInfo(Type objType, object obj)
        {
            ObjType = objType;
            Obj = obj;
        }
    }

}