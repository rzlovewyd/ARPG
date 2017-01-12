using UnityEngine;
using System.Collections;
using Game.Data.MsgData;

namespace Game.BaseClass
{
    public class BaseHero : BaseCharacter
    {
        protected Rigidbody m_Rigidbody;

        public GameObject m_Weapon;
        public GameObject m_BulletWeapon;

        protected SphereCollider m_WeaponCol;
        protected BoxCollider m_BulletWeaponCol;

        /// <summary>
        /// 走起扬起的尘土
        /// </summary>
        public GameObject m_DustObj;
        protected ParticleSystem m_DustEff;

        /// <summary>
        /// 拖尾特效
        /// </summary>
        public GameObject[] m_TrailEffs = new GameObject[2];

        public float m_MoveSpeed = 4.0f;
        public float m_RushSpeed = 8.0f;
        public float m_RushTime = 0.5f;

        /// <summary>
        /// 当前攻击的动作索引
        /// </summary>
        protected int m_CurAttackIdx;

        public override void Init(int uid)
        {
            base.Init(uid);
            m_Rigidbody = GetComponent<Rigidbody>();
            m_WeaponCol = m_Weapon.GetComponent<SphereCollider>();
            m_BulletWeaponCol = m_BulletWeapon.GetComponent<BoxCollider>();

            //状态
            RegisterState(eState.Sleep, Sleep_Enter, null, null);
            RegisterState(eState.Idle, Idle_Enter, null, Idle_Exit);
            RegisterState(eState.Rush, Rush_Enter, Rush_Update, null);
            RegisterState(eState.Move, Move_Enter, Move_Update, Move_Exit);
            RegisterState(eState.MoveTo, MoveTo_Enter, MoveTo_Update, MoveTo_Exit);
            RegisterState(eState.Attack, Attack_Enter, Attack_Update, Attack_Exit);

            //消息
            RegisterMsgHandler(eCmd.Idle, OnMsg_Idle);
            RegisterMsgHandler(eCmd.Rush, OnMsg_Rush);
            RegisterMsgHandler(eCmd.Move, OnMsg_Move);
            RegisterMsgHandler(eCmd.MoveTo, OnMsg_MoveTo);
            RegisterMsgHandler(eCmd.Attack, OnMsg_Attack);
        }

        public void Sleep_Enter(StateParam param)
        {
            m_Rigidbody.velocity = Vector3.zero;
        }

        public virtual void Idle_Enter(StateParam param)
        {
            PlayAnimInt("State", 0);
            if(m_DustEff != null)
                this.m_DustEff.Stop();

            m_Rigidbody.velocity = Vector3.zero;

            m_WeaponCol.enabled = false;
            m_WeaponCol.center = new Vector3(0, 0.5f, 0.4f);
            m_BulletWeaponCol.enabled = false;
            for (int i = 0; i < m_TrailEffs.Length; i++)
            {
                if (this.m_TrailEffs[i] != null)
                {
                    this.m_TrailEffs[i].SetActive(false);
                }
            }       
        }

        public void Idle_Exit()
        {
            for (int i = 0; i < m_TrailEffs.Length; i++)
            {
                if (this.m_TrailEffs[i] != null)
                {
                    this.m_TrailEffs[i].SetActive(false);
                }
            }
        }

        public void Move_Enter(StateParam param)
        {
            PlayAnimInt("State", 2);
            if (m_DustEff != null && !m_DustEff.isPlaying)
            {
                m_DustEff.Play();
            }
        }

        public void Move_Update(float delta)
        {
            m_Rigidbody.velocity = m_MoveDirection * m_MoveSpeed;
        }

        public void Move_Exit()
        {
            PlayAnimInt("State", 0);
        }

        public void MoveTo_Enter(StateParam param)
        {
            Move_Enter(param);
        }

        public void MoveTo_Update(float delta)
        {
            if (!IsDead())
            {
                if (Vector3.Distance(transform.position, m_MoveToPos) <= 0.1f)
                {
                    ChangeState(eState.Idle, null);
                }
                else
                {
                    Vector3 vector = (m_MoveToPos - transform.position).normalized;
                    m_Rigidbody.velocity = (vector * m_MoveSpeed);
                }
            }
        }

        public void MoveTo_Exit()
        {
            PlayAnimInt("State", 0);
        }

        /// <summary>
        /// 进入攻击状态
        /// </summary>
        public virtual void Attack_Enter(StateParam param)
        {
            PlayAnim("DoAttack" + (m_CurAttackIdx + 1), 1.6f);
        }

        public virtual void Attack_Update(float delta) 
        {
            
        }

        /// <summary>
        /// 攻击状态退出
        /// </summary>
        public virtual void Attack_Exit()
        {
            if(m_DustEff != null)
                m_DustEff.Stop();
            
            m_WeaponCol.enabled = false;
            m_BulletWeaponCol.enabled = false;
        }

        public void Rush_Enter(StateParam param)
        {
            PlayAnim("Rush");

            if (m_DustEff != null && !m_DustEff.isPlaying)
                m_DustEff.Play();

            m_Rigidbody.velocity = m_MoveDirection * m_MoveSpeed * m_RushSpeed;
        }

        public void Rush_Update(float delta)
        {
            if (!IsDead())
            {
                m_StateTimer += delta;

                if (m_StateTimer >= m_RushTime)
                {
                    ChangeState(eState.Idle, null);
                }
            }
        }

        /// <summary>
        /// 待机消息
        /// </summary>
        public void OnMsg_Idle(IBaseMsgData baseData)
        {
            if (!IsDead())
            {
                Idle_MsgData data = (Idle_MsgData)baseData;
                transform.position = data.pos;
                Look(data.dir);
                ChangeState(eState.Idle, null);
            }
        }
        /// <summary>
        /// 冲刺消息
        /// </summary>
        public void OnMsg_Rush(IBaseMsgData baseData)
        {
            if (!IsDead())
            {
                Rush_MsgData data = (Rush_MsgData)baseData;
                transform.position = data.pos;
                Look(data.dir);
                m_MoveDirection = data.dir.normalized;
                ChangeState(eState.Rush, null);
            }
        }

        /// <summary>
        /// 移动消息
        /// </summary>
        public void OnMsg_Move(IBaseMsgData baseData)
        {
            if (!IsDead())
            {
                Move_MsgData data = (Move_MsgData)baseData;
                Vector3 targetPos = data.pos;
                Vector3 v_joy = data.dir;

                Vector3 dir = targetPos - transform.position;

                if (dir.magnitude < 0.01f)
                {
                    dir = v_joy;
                    Look(dir);
                }
                else
                {
                    dir.Normalize();
                    dir = (Vector3)(dir * v_joy.magnitude);
                    Turn(targetPos, 0.2f);
                }

                m_MoveDirection = dir;
                ChangeState(eState.Move, null);       //切换到移动状态
            }
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        public void OnMsg_MoveTo(IBaseMsgData baseData)
        {
            if (!base.IsDead())
            {
                MoveTo_MsgData data = (MoveTo_MsgData)baseData;
                transform.position = data.pos;
                Turn(data.targetPos, 0.2f);
                m_MoveToPos = data.targetPos;
                ChangeState(eState.MoveTo, null);
            }
        }

        /// <summary>
        /// 攻击消息
        /// </summary>
        public void OnMsg_Attack(IBaseMsgData baseData)
        {
            if (!IsDead())
            {
                m_Rigidbody.velocity = Vector3.zero;
                Attack_MsgData data = (Attack_MsgData)baseData;
                transform.position = data.pos;
                Look(data.dir);

                m_CurAttackIdx = data.attackIdx;

                ChangeState(eState.Attack, null);
            }
        }

        public virtual void OnWakeEnd()
        {
            ChangeState(eState.Idle);
        }
        public virtual void OnAttackedHit() { }
        public virtual void OnAttackStart() { }
        public virtual void OnAttackEnd()
        {
            ChangeState(eState.Idle);
        }
        public virtual void OnAttackMoveStart()
        {
            gameObject.GetComponent<Rigidbody>().velocity = m_Direction * 3;
        }
        public virtual void OnAttackMoveEnd()
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        public virtual void OnAttackOn(int uid) { }
        public virtual void OnDashAttackOn() { }
        public virtual void OnDashAttackEnd() { }
        public virtual void OnSkillMoveStart(int uid) { }
        public virtual void OnSkillSlowStart(int uid) { }
        public virtual void OnSkillMoveEnd() { }
        public virtual void OnSkillSlowEnd() { }
        public virtual void OnSkillEnd() { }
        public virtual void OnSkillPlayEff(int effId) { }
        public virtual void OnSkillOn(int uid) { }
        public virtual void OnSkillOn() { }
        public virtual void OnRunLeft() { }
        public virtual void OnRunRight() { }
        public virtual void OnJump() { }
        public virtual void OnDown() { }
        public virtual void HeroQteTrigger(int uid) { }
        public virtual void OnBlurOn() { }
        public virtual void OnBlurEnd() { }
        public virtual void OnTimeScaleChange() { }
    }
}
