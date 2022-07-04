using System;
using UnityEngine;

namespace APP
{
    public interface IConfig
    {
        Instance Instance { get; }
    }

    public class Instance
    {
        public object Obj { get; private set; }
        public Type ObjType { get; private set; }

        public Instance(object obj)
        {
            Obj = obj;
            ObjType = obj.GetType();
        }
    }

    public class Instance<TScene> : Instance
    where TScene : UComponent, IScene
    {
        public String Name { get; private set; }
        public SceneIndex SceneIndex { get; private set; }
        public GameObject GObj { get; private set; }
        public GameObject GObjParent { get; private set; }

        public Instance(
            object instance,
            string name = null) : base(instance)
        {
            Name = name;
            SceneIndex = SceneIndex<TScene>.Index;
            GObj = ((UComponent) instance).gameObject;
            
            if(name == null) 
                GObj.name = "Unnamed"; 
            else 
                GObj.name = name;
            
            GObjParent = ((UComponent) instance).transform.parent.gameObject;
        }
    }

}