using System;
using UnityEngine;

namespace APP.Map
{
    public class MapController : Controller
    {
        private GameObject m_MapField;

        public override void Init()
        {
            //CreateLayer("SpaceField", 0);
            //CreateLayer("Stars", 1);
        }


        public override void Dispose() { } 

    }
}