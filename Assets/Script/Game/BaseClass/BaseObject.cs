using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.BaseClass
{
    /// <summary>
    /// 游戏对象的基类
    /// </summary>
    public class BaseObject : MonoBehaviour
    {
        protected int m_Uid;
        protected string m_Name;
        //标识移除后储存
        protected bool m_bRererveRemove;


        protected Dictionary<eCmd, OnMsgFunc> m_MsgFuncs = new Dictionary<eCmd, OnMsgFunc>();

        public int Uid
        {
            get
            {
                return m_Uid;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public virtual void Init(int uid)
        {
            m_Uid = uid;
            m_MsgFuncs.Clear();
        }

        public virtual void Enable(Vector3 pos)
        {
            transform.position = pos;
            this.m_bRererveRemove = false;
        }

        /// <summary>
        /// 注册消息指令
        /// </summary>
        protected void RegisterMsgHandler(eCmd cmd, OnMsgFunc func)
        {
            if (!m_MsgFuncs.ContainsKey(cmd))
            {
                m_MsgFuncs.Add(cmd, func);
            }
            else
            {
                Debug.LogError("Register error!" + m_Uid + " already had cmd " + cmd);
            }
        }

        /// <summary>
        /// 执行消息指令
        /// </summary>
        public void OnObjMsg(eCmd cmd, IBaseMsgData bmd)
        {
            if (m_MsgFuncs.ContainsKey(cmd))
            {
                m_MsgFuncs[cmd](bmd);
            }
            else
            {
                Debug.LogError("execute error!" + name + " Don't have cmd " + cmd);
            }
        }

        protected void OnEnable()
        {
            Enable(Vector3.zero);
        }

        protected void Update()
        {
            this.UpdateObj(Time.deltaTime);
        }

        protected virtual void UpdateObj(float delta) { }

        public delegate void OnMsgFunc(IBaseMsgData bmd);
    }
}

/// <summary>
/// 基础消息接口
/// </summary>
public interface IBaseMsgData
{
    eCmd GetCmd();

    string GetReceiver();
}
