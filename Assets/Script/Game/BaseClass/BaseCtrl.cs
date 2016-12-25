using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Manager;

namespace Game.BaseClass
{
    /// <summary>
    /// 基础控制类
    /// </summary>
    public class BaseCtrl : MonoBehaviour
    {
        protected int uid;
        protected string m_Name;

        /// <summary>
        /// 是否唤醒
        /// </summary>
        protected bool m_bAwake;

        protected eState m_OwnerState;
        protected GameObject m_OwnerObj;
        protected BaseActor m_OwnerScript;

        protected float m_StateTimer;
        protected Dictionary<eState, StateFunc> m_StateFuncs = new Dictionary<eState, StateFunc>();

        public virtual void Init(int id, string targetName)
        {
            uid = id;
            m_bAwake = false;
            m_StateTimer = 0f;
            m_Name = targetName + "_Ctrl";
            m_StateFuncs.Clear();

            m_OwnerObj = ObjectManager.FindObject(targetName);
            m_OwnerScript = m_OwnerObj.GetComponent<BaseActor>();
            if (!m_OwnerScript)
            {
                Debug.LogError("Don't Found BaseActor On " + targetName);
            }
        }

        protected void RegisterStateHandle(eState state, StateFunc func)
        {
            if (!m_StateFuncs.ContainsKey(state))
            {
                m_StateFuncs.Add(state, func);
            }
            else
            {
                Debug.LogError(m_Name + " already have the state!");
            }
        }

        protected void Update()
        {
            if (m_bAwake)
            {
                UpdateCtrl(Time.deltaTime);
            }
        }

        protected virtual void UpdateCtrl(float delta)
        {
            m_OwnerState = m_OwnerScript.CurState;
            if (m_StateFuncs.ContainsKey(m_OwnerState))
            {
                m_StateFuncs[m_OwnerState](delta);
            }
        }

        public int Uid
        {
            get{ return uid; }
        }

        public string Name
        {
            get{ return m_Name; }
        }

        public bool IsAwake
        {
            get{ return m_bAwake; }
            set{ m_bAwake = value; }
        }

        public delegate void StateFunc(float delta);
    }
}
