using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Game.BaseClass;
using Game.Helper;

namespace Game.BaseClass
{
    public class BaseHeroCtrl : BaseCtrl 
    {
        protected BaseHero m_Hero;
        protected Vector3 m_JoyDir;

        public override void Init(int id, string targetName)
        {
            base.Init(id, targetName);
            m_Hero = (BaseHero)m_OwnerScript;

            RegisterStateHandle(eState.Idle, Idle_Update);
            RegisterStateHandle(eState.Move, Move_Update);
            RegisterStateHandle(eState.MoveTo, MoveTo_Update);
            RegisterStateHandle(eState.Attack, Attack_Update);
        }

        protected void UpdateJoyState(float delta)
        {
            #if UNITY_EDITOR
            m_JoyDir.y = 0f;
            m_JoyDir.z = Input.GetAxis ("Vertical");
            m_JoyDir.x = Input.GetAxis ("Horizontal");
            #endif
        }

        public virtual void Idle_Update(float delta)
        {
            
        }
        public virtual void Move_Update(float delta)
        {
            if(IsAvailJoyDir())
            {
                SendObjMsgHelper.SendMsg_Move(m_Hero.GetName(), m_Hero.GetName(), m_JoyDir, transform.position);
            }
            else
            {
                SendObjMsgHelper.SendMsg_Idle(m_Hero.GetName(), m_Hero.GetName(), m_Hero.GetDirection(), transform.position);
            }
        }
        public virtual void MoveTo_Update(float delta)
        {

        }
        public virtual void Attack_Update(float delta)
        {

        }

        /// <summary>
        /// 是否可获得摇杆方位
        /// </summary>
        /// <returns></returns>
        public bool IsAvailJoyDir()
        {
            return m_JoyDir.magnitude > 0.5f;
        }
    }
}
