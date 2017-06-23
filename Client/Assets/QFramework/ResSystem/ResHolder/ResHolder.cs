using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace QFramework
{
    public class ResHolder : QSingleton<ResHolder>
    {
        protected string[] CommonRes =
        {

        };
			

        protected ResLoader m_Loader;

        public override void OnSingletonInit()
        {
            m_Loader = new ResLoader();
        }
			
    }
}
