using UnityEngine;
using System.Collections;

namespace Game.BaseClass
{
    public class BaseDeBuff
    {
        protected float m_Time;
        protected float m_Value;
        protected DeBuffType m_Type;
        protected BaseCharacter m_Owner;

        public float Time{ get; set; }

        public float Value{ get; private set; }

        public DeBuffType BufferType{ get; private set; }

        public static BaseDeBuff GetInstance(DeBuffType type)
        {
            switch (type)
            {
                case DeBuffType.Min:
                    break;
                case DeBuffType.FR:
                    break;
                case DeBuffType.IC:
                    break;
            }
            return new BaseDeBuff();
        }

        public virtual void OnInit(DeBuffType type, float time, float value, BaseCharacter owner)
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

    public enum DeBuffType
    {
        None = -1,
        Min = 0,
        PS = 0,
        IC = 1,
        FR = 2,
        CS = 3,
        Max = 4,
    }
}