using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.BaseClass
{
    /// <summary>
    /// 基础角色(英雄和怪物)
    /// 具有buff系统
    /// </summary>
    public class BaseCharacter : BaseActor
    {
        protected Vector3 m_MoveToPos;
        protected Vector3 m_MoveDirection;
        protected CapsuleCollider m_CapsuleCol;

        public override void Init(int uid)
        {
            base.Init(uid);
            m_CapsuleCol = GetComponent<CapsuleCollider>();
        }

        public override void Enable(Vector3 pos)
        {
            base.Enable(pos);
            m_Buffs.Clear();
            m_Debuffs.Clear();
            m_MoveToPos = Vector3.zero;
            m_MoveDirection = Vector3.zero;
        }

        protected override void UpdateObj(float delta)
        {
            base.UpdateObj(delta);
            UpdateBuffs(delta);
            UpdateDeBuffs(delta);
        }

        public float GetBodyRadius()
        {
            if (m_CapsuleCol != null)
                return m_CapsuleCol.radius;

            return 0f;
        }

        /// <summary>
        /// 是否可以被攻击
        /// </summary>
        public virtual bool IsPossibleAttacked()
        {
            return (m_CurState != eState.Sleep && m_CurState != eState.Die);
        }

        /// <summary>
        /// 根据当前状态判定是否在Move
        /// </summary>
        public virtual bool IsPossibleMoveTo()
        {
            switch (m_CurState)
            {
                case eState.Die:
                case eState.Down:
                case eState.Stun:
                case eState.Slide:
                case eState.CriticalStiff:
                    return false;
            }
            return true;
        }

        public bool IsDead()
        {
            return m_CurState == eState.Die;
        }

        #region Buffer

        protected List<BaseBuff> m_Buffs = new List<BaseBuff>();
        protected List<BaseDeBuff> m_Debuffs = new List<BaseDeBuff>();

        protected void UpdateBuffs(float delta)
        {
            for (int i = 0; i < m_Buffs.Count; i++)
            {
                m_Buffs[i].OnUpdate(delta);
                m_Buffs[i].Time -= delta;
                if (m_Buffs[i].Time < 0f)
                {
                    m_Buffs[i].OnDestroy();
                    m_Buffs.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void UpdateDeBuffs(float delta)
        {
            for (int i = 0; i < m_Debuffs.Count; i++)
            {
                m_Debuffs[i].OnUpdate(delta);
                m_Debuffs[i].Time -= delta;
                if (m_Debuffs[i].Time < 0f)
                {
                    m_Debuffs[i].OnDestroy();
                    m_Debuffs.RemoveAt(i);
                    i--;
                }
            }
        }

        protected void AddBuff(BuffType type, float time, float value)
        {
            BaseBuff buff = m_Buffs.Find(x => x.BufferType == type);
            if (buff == null)
            {
                buff = BaseBuff.GetInstance(type);
                m_Buffs.Add(buff);
                buff.OnInit(type, time, value, this);
            }
            buff.OnInit(type, time, value, this);
        }

        protected void AddDeBuff(DeBuffType type, float time, float value)
        {
            BaseDeBuff buff = m_Debuffs.Find(x => x.BufferType == type);
            if (buff == null)
            {
                buff = BaseDeBuff.GetInstance(type);
                m_Debuffs.Add(buff);
                buff.OnInit(type, time, value, this);
            }
            buff.OnInit(type, time, value, this);
        }

        protected float GetBuffValue(BuffType type)
        {
            BaseBuff buff = m_Buffs.Find(x => x.BufferType == type);
            return buff != null ? buff.Value : 0f;
        }

        protected float GetDeBuffValue(DeBuffType type)
        {
            BaseDeBuff buff = m_Debuffs.Find(x => x.BufferType == type);
            return buff != null ? buff.Value : 0f;
        }

        #endregion
    }

    public enum eState
    {
        None,
        Sleep,              //休眠
        //Advent,
        Idle,
        Move,
        MoveTo,
        Turn,
        CriticalStiff,      //至命最后一击
        Stiff,              //僵直
        Slide,              //滑动
        Down,               //打到
        StunSlide,          //击倒
        Stun,               //击晕
        Die,                //死亡
        Rush,               //冲刺
        Rebirth,            //重生
        Attack,             //攻击
        //战士技能
        PowerSlash,         //猛击
        ShockWave,          //冲击波
        FatalCircle,        //致命圈
        ShadowDance,        //影舞
    }
}
