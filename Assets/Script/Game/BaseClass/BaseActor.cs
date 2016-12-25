using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game.BaseClass
{
    /// <summary>
    /// 使物体具有状态机以及一些基础功能
    /// </summary>
    public class BaseActor : BaseObject
    {
        //AI控制

        //普通控制
        public GameObject m_FBX;

        public override void Init(int uid)
        {
            base.Init(uid);
            m_StateFuncs.Clear();
            RefreshMaterialInfo();
            if (m_FBX != null)
            {
                animator = m_FBX.GetComponent<Animator>();
            }
        }

        public override void Enable(Vector3 pos)
        {
            base.Enable(pos);
            m_CurState = 0;
            m_PreState = 0;
            m_StateTimer = 0f;
            m_ShineTimer = 0f;
            m_ShineValue = 0f;
            m_bRererveRemove = false;
            m_Direction = Vector3.forward;
        }

        protected override void UpdateObj(float delta)
        {
            base.UpdateObj(delta);

            if (m_bUpdateShineEff)
            {
                UpdateShineEff(delta);
            }

            if (m_StateFuncs.ContainsKey(m_CurState) && m_StateFuncs[m_CurState].updateFunc != null)
            {
                m_StateFuncs[m_CurState].updateFunc(delta);
            }
        }

        #region 动画

        protected Animator animator;

        public void PlayAnim(string condition, float speed = 0)
        {
            animator.speed = speed;
            animator.SetTrigger(condition);
        }

        public void PlayAnimInt(string condition, int value, float speed = 0)
        {
            animator.speed = speed;
            animator.SetInteger(condition, value);
        }

        public void PlayAnimFloat(string condition, float value, float speed = 0)
        {
            animator.speed = speed;
            animator.SetFloat(condition, value);
        }

        #endregion

        #region 状态机

        protected eState m_CurState;
        protected eState m_PreState;
        protected float m_StateTimer;
        protected StateParam m_StateParam = new StateParam();
        protected Dictionary<eState, StateFunc> m_StateFuncs = new Dictionary<eState, StateFunc>();

        /// <summary>
        /// Registers the state.
        /// </summary>
        protected void RegisterState(eState state, StateFunc.EnterFunc enter, StateFunc.UpdateFunc update, StateFunc.ExitFunc exit)
        {
            StateFunc func = new StateFunc
            {
                enterFunc = enter,
                updateFunc = update,
                exitFunc = exit
            };

            if (!m_StateFuncs.ContainsKey(state))
            {
                m_StateFuncs.Add(state, func);
            }
            else
            {
                Debug.LogError("RegisterState error! " + name + " already had state = " + state);
            }
        }

        /// <summary>
        /// Changes the state.
        /// </summary>
        protected void ChangeState(eState state, StateParam param = null)
        {
            m_PreState = m_CurState;
            m_CurState = state;

            if (m_StateFuncs.ContainsKey(m_PreState) && m_StateFuncs[m_PreState].exitFunc != null)
            {
                m_StateFuncs[m_PreState].exitFunc();
            }

            if (m_StateFuncs.ContainsKey(m_CurState) && m_StateFuncs[m_CurState].enterFunc != null)
            {
                m_StateFuncs[m_CurState].enterFunc(param);
            }

            m_StateTimer = 0f;
            m_StateParam = param;
        }

        public eState CurState
        {
            get{ return m_CurState; }
        }

        #endregion

        #region Transform

        protected Vector3 m_Direction;

        public Vector3 GetDirection()
        {
            return m_Direction;
        }

        /// <summary>
        /// 注视
        /// </summary>
        public void Look(Vector3 dir)
        {
            if (dir != Vector3.zero)
            {
                dir.Normalize();
                transform.localRotation = Quaternion.LookRotation(dir);
                m_Direction = dir;
            }
        }

        /// <summary>
        /// 转身
        /// </summary>
        public void Turn(float angle, float duration)
        {

        }

        /// <summary>
        /// 转身
        /// </summary>
        public void Turn(Vector3 targetPos, float duration)
        {

        }

        #endregion

        #region 闪光效果

        protected float m_ShineTimer;
        protected float m_ShineValue;
        //闪亮时间 (一般用在被攻击时)
        protected float m_ShineTimeMax;
        protected bool m_bUpdateShineEff;
        protected List<Material> m_Materials = new List<Material>();

        public void ShineEff(float shineTime = 0.15f)
        {
            m_ShineTimer = 0f;
            m_ShineTimeMax = shineTime;
            m_bUpdateShineEff = true;
        }

        protected void UpdateShineEff(float delta)
        {
            m_ShineTimer += delta;
            if (m_ShineTimer >= m_ShineTimeMax)
            {
                m_ShineTimer = 0f;
                m_ShineValue = 0f;
                m_bUpdateShineEff = false;
                RefreshShineEff(new Color(0, 0, 0, 1));
            }
            else
            {
                float num = m_ShineTimer / m_ShineTimeMax;
                m_ShineValue = (1 - num) * 5f;
                RefreshShineEff(new Color(1, 1, 1, 1));
            }
        }

        /// <summary>
        /// 刷新闪光效果
        /// </summary>
        protected void RefreshShineEff(float shineValue)
        {
            for (int i = 0; i < m_Materials.Count; i++)
            {
                m_Materials[i].SetFloat("_ShineValue", shineValue);
            }
        }

        /// <summary>
        /// 刷新闪光效果
        /// </summary>
        protected void RefreshShineEff(Color color)
        {
            for (int i = 0; i < this.m_Materials.Count; i++)
            {
                this.m_Materials[i].SetColor("_AddColor", color);
            }
        }

        /// <summary>
        /// 刷新材质球信息
        /// </summary>
        protected void RefreshMaterialInfo()
        {
            m_Materials.Clear();
            MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < mrs.Length; i++)
            {
                for (int j = 0; j < mrs[i].materials.Length; j++)
                {
                    m_Materials.Add(mrs[i].materials[j]);
                }
            }

            SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < smrs.Length; i++)
            {
                for (int j = 0; j < smrs[i].materials.Length; j++)
                {
                    m_Materials.Add(smrs[i].materials[j]);
                }
            }
        }

        #endregion

    }

    /// <summary>
    /// 状态参数
    /// </summary>
    public class StateParam
    {
        public bool bValue;
        public float fValue;
        public int iValue;
        public Vector3 vValue;
    }

    /// <summary>
    /// 状态回调函数
    /// </summary>
    public class StateFunc
    {
        public delegate void EnterFunc(StateParam param);

        public delegate void UpdateFunc(float delta);

        public delegate void ExitFunc();

        public EnterFunc enterFunc;
        public ExitFunc exitFunc;
        public UpdateFunc updateFunc;
    }
}