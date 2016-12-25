using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game.Manager
{
    public class ObjectManager 
    {
        private static Dictionary<string, GameObject> m_Objects = new Dictionary<string, GameObject>();

        /// <summary>
        /// 查找对象 
        /// </summary>
        public static GameObject FindObject(string name)
        {
            GameObject gobj = null;
            if (!m_Objects.TryGetValue(name, out gobj))
            {
                gobj = GameObject.Find(name);
            }

            if(gobj == null)
            {
                Debug.LogError("Don't Find Object " + name);
            }

            return gobj;
        }

        public static void Init()
        {
            Resources.UnloadUnusedAssets();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        #region 加载资源

        #endregion

        #region 初始化资源

        #endregion

        #region 生产资源

        #endregion
    }
}