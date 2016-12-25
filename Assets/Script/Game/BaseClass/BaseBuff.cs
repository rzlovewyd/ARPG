using UnityEngine;
using System.Collections;

namespace Game.BaseClass
{
    public class BaseBuff
    {
        protected float m_Time;
        protected float m_Value;
        protected BuffType m_Type;
        protected BaseCharacter m_Owner;

        public float Time{ get; set; }

        public float Value{ get; private set; }

        public BuffType BufferType{ get; private set; }

        public static BaseBuff GetInstance(BuffType type)
        {
            return new BaseBuff();
        }

        public virtual void OnInit(BuffType type, float time, float value, BaseCharacter owner)
        {
            m_Type = type;
            m_Time = time;
            m_Value = value;
            m_Owner = owner;
        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnUpdate(float delta)
        {

        }
    }

    public enum BuffType
    {
        None = -1,
        MIN = 0,
        ATT_UP = 0,
        DEF_UP = 1,
        AS_UP = 2,
        HEAL = 3,
        VAMPIRE = 4,
        HPPORTION = 5,
        MPPORTION = 6,
        MAX = 7,
    }
}