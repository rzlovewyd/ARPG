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

        /// <summary>
        /// 最大攻击连招索引
        /// </summary>
        protected int m_AttackIdxMax = 4;
        /// <summary>
        /// 当前攻击Id
        /// </summary>
        protected int m_CurAttackIdx;

        /// <summary>
        /// 标识是否持续攻击
        /// </summary>
        protected bool m_bHoldAttack;

        /// <summary>
        /// 储备下一次攻击
        /// </summary>
        protected bool m_bReserveNextAttack;

        public override void Init(int id, string targetName)
        {
            base.Init(id, targetName);
            m_Hero = (BaseHero)m_OwnerScript;

            RegisterStateHandle(eState.Idle, Idle_Update);
            RegisterStateHandle(eState.Move, Move_Update);
            RegisterStateHandle(eState.MoveTo, MoveTo_Update);
            RegisterStateHandle(eState.Attack, Attack_Update);
        }

        protected override void UpdateCtrl(float delta)
        {
            UpdateJoyState();
            UpdateAttackBtnState();
            base.UpdateCtrl(delta);
        }

        protected void UpdateJoyState()
        {
            #if UNITY_EDITOR
            m_JoyDir.y = 0f;
            m_JoyDir.z = Input.GetAxis ("Vertical");
            m_JoyDir.x = Input.GetAxis ("Horizontal");
            #endif
        }

        protected void UpdateAttackBtnState()
        {
            #if UNITY_EDITOR
            m_bHoldAttack = Input.GetAxis("Fire1") > 0f;
            if(Input.GetKeyDown(KeyCode.J))
            {
                OnUseActiveSkill(SkillID.PowerSlash);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                OnUseActiveSkill(SkillID.ShockWave);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                OnUseActiveSkill(SkillID.FatalCircle);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnUseActiveSkill(SkillID.ShadowDance);
            }
            #endif
        }

        public virtual void Idle_Update(float delta)
        {
            if (m_bReserveNextAttack)
            {
                m_bReserveNextAttack = false;
                m_CurAttackIdx = (m_CurAttackIdx + 1) % m_AttackIdxMax;
                SendObjMsgHelper.SendMsg_Attack(m_Hero.Name, m_Hero.Name, m_JoyDir, m_Hero.transform.position, m_CurAttackIdx);
            }
            else if (m_bHoldAttack)
            {
                m_CurAttackIdx = 0;
                m_bReserveNextAttack = false;
                SendObjMsgHelper.SendMsg_Attack(m_Hero.Name, m_Hero.Name, m_JoyDir, m_Hero.transform.position, m_CurAttackIdx);
            }
            else if (IsAvailJoyDir())
            {
                SendObjMsgHelper.SendMsg_Move(m_Hero.Name, m_Hero.Name, m_JoyDir, m_Hero.transform.position);
            }
        }
        public virtual void Move_Update(float delta)
        {
            if (m_bHoldAttack)
            {
                m_CurAttackIdx = 0;
                m_bReserveNextAttack = false;
                SendObjMsgHelper.SendMsg_Attack(m_Hero.Name, m_Hero.Name, m_JoyDir, m_Hero.transform.position, m_CurAttackIdx);
            }
            else if (IsAvailJoyDir())
            {
                SendObjMsgHelper.SendMsg_Move(m_Hero.Name, m_Hero.Name, m_JoyDir, transform.position);
            }
            else
            {
                SendObjMsgHelper.SendMsg_Idle(m_Hero.Name, m_Hero.Name, m_Hero.GetDirection(), transform.position);
            }
        }
        public virtual void MoveTo_Update(float delta)
        {

        }
        public virtual void Attack_Update(float delta)
        {
            //BaseHero中的OnAttackEnd动画回调会切换到idle状态
            m_bReserveNextAttack = m_bHoldAttack && m_CurAttackIdx < m_AttackIdxMax;
        }

        /// <summary>
        /// 使用技能
        /// </summary>
        public virtual void OnUseActiveSkill(SkillID skillId)
        {
            m_CurAttackIdx = 0;
            m_bHoldAttack = false;
            m_bReserveNextAttack = false;
        }

        /// <summary>
        /// 获取攻击朝向
        /// </summary>
        public virtual Vector3 GetAttackDir()
        {
            if (IsAvailJoyDir())
            {
                return m_JoyDir;
            }

            return m_Hero.GetDirection();
        }

        /// <summary>
        /// 是否可获得摇杆方位
        /// </summary>
        /// <returns></returns>
        public bool IsAvailJoyDir()
        {
            return m_JoyDir.magnitude > 0.5f;
        }

        public virtual void OnWakeEnd() { m_Hero.OnWakeEnd(); }
        public virtual void OnAttackStart() { m_Hero.OnAttackStart(); }
        public virtual void OnAttackedHit() { m_Hero.OnAttackedHit(); }
        public virtual void OnAttackEnd() { m_Hero.OnAttackEnd(); }
        public virtual void OnAttackMoveStart() { m_Hero.OnAttackMoveStart(); }
        public virtual void OnAttackOn(int uid) { m_Hero.OnAttackOn(uid); }
        public virtual void OnAttackMoveEnd() { m_Hero.OnAttackMoveEnd(); }
        public virtual void OnDashAttackOn() { m_Hero.OnDashAttackOn(); }
        public virtual void OnDashAttackEnd() { m_Hero.OnDashAttackEnd(); }
        public virtual void OnSkillMoveStart(float moveSpeed) { m_Hero.OnSkillMoveStart(moveSpeed); }
        public virtual void OnSkillSlowStart(float animSpeed) { m_Hero.OnSkillSlowStart(animSpeed); }
        public virtual void OnSkillMoveEnd() { m_Hero.OnSkillMoveEnd(); }
        public virtual void OnSkillSlowEnd() { m_Hero.OnSkillSlowEnd(); }
        public virtual void OnSkillEnd() { m_Hero.OnSkillEnd(); }
        public virtual void OnSkillPlayEff(int effId) { m_Hero.OnSkillPlayEff(effId); }
        public virtual void OnSkillOn(int uid) { m_Hero.OnSkillOn(uid); }
        public virtual void OnSkillOn() { m_Hero.OnSkillOn(); }
        public virtual void OnRunLeft() { m_Hero.OnRunLeft(); }
        public virtual void OnRunRight() { m_Hero.OnRunRight(); }
        public virtual void OnJump() { m_Hero.OnJump(); }
        public virtual void OnDown() { m_Hero.OnDown(); }
        public virtual void HeroQteTrigger(int uid) { m_Hero.HeroQteTrigger(uid); }
        public virtual void OnBlurOn() { m_Hero.OnBlurOn(); }
        public virtual void OnBlurEnd() { m_Hero.OnBlurEnd(); }
        public virtual void OnTimeScaleChange() { m_Hero.OnTimeScaleChange(); }
    }
}
