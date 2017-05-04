using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    [Serializable]
    public class ABUnit
    {
        public string abName;
        public string[] abDepends;

        public ABUnit(string name, string[] depends)
        {
            this.abName = name;
            if (depends == null || depends.Length == 0)
            {

            }
            else
            {
                this.abDepends = depends;
            }
        }

        public override string ToString()
        {
            string result = string.Format("ABName:{0}", abName);

            if (abDepends == null)
            {
                return result;
            }

            for (int i = 0; i < abDepends.Length; ++i)
            {
                result += string.Format(" #:{0}", abDepends[i]);
            }

            return result;
        }
    }
}
