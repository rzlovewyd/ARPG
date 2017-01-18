using UnityEngine;
using System.Collections;
using Game.BaseClass;
using System;
using Game.Data.MsgData;

public class Warrior : BaseHero 
{
    /// <summary>
    /// 标记猛砍技能第几段
    /// </summary>
    protected int m_PowerSlashPhase;

    void Start()
    {
        Init(0);
        m_Name = name;
        m_Ctrl = transform.FindChild("FBX").GetComponent<BaseHeroCtrl>();
        m_Ctrl.Init(0, "Hero_Warrior");
        m_Ctrl.IsAwake = true;
        ChangeState(eState.Idle);
    }
    public override void Init(int uid)
    {
        base.Init(uid);

        m_ShadowDancePhaseMax = 5;
        RegisterState(eState.PowerSlash, PowerSlash_Enter, PowerSlash_Update, null);
        RegisterState(eState.ShockWave, ShockWave_Enter, ShockWave_Update, null);
        RegisterState(eState.FatalCircle, FatalCircle_Enter, FatalCircle_Update, null);
        RegisterState(eState.ShadowDance, ShadowDance_Enter, ShadowDance_Update, null);

        RegisterMsgHandler(eCmd.PowerSlash, OnMsg_PowerSlash);
        RegisterMsgHandler(eCmd.ShockWave, OnMsg_ShockWave);
        RegisterMsgHandler(eCmd.FatalCircle, OnMsg_FatalCircle);
        RegisterMsgHandler(eCmd.ShadowDance, OnMsg_ShadowDance);
    }

    private void PowerSlash_Enter(StateParam param)
    {
        PlayAnim("DoSkill1");
    }

    private void PowerSlash_Update(float delta)
    {
        m_StateTimer += delta;
    }

    private void ShockWave_Enter(StateParam param)
    {
        PlayAnim("DoSkill2");
    }

    private void ShockWave_Update(float delta)
    {
        m_StateTimer += delta;
    }

    private void FatalCircle_Enter(StateParam param)
    {
        PlayAnim("DoSkill4");
    }

    private void FatalCircle_Update(float delta)
    {
        m_StateTimer += delta;
    }

    private void ShadowDance_Enter(StateParam param)
    {
        PlayAnim("DoSkill3");
    }

    private void ShadowDance_Update(float delta)
    {
        m_StateTimer += delta;
    }

    private void OnMsg_PowerSlash(IBaseMsgData bmd)
    {
        PowerSlash_MsgData data = (PowerSlash_MsgData)bmd;
        transform.position = data.pos;
        m_Rigidbody.velocity = Vector3.zero;
        Look(data.dir);
        m_CurSkillId =  SkillID.PowerSlash;
        m_PowerSlashPhase = 0;
        ChangeState(eState.PowerSlash);
    }

    private void OnMsg_ShockWave(IBaseMsgData bmd)
    {
        ShockWave_MsgData data = (ShockWave_MsgData)bmd;
        transform.position = data.pos;
        m_Rigidbody.velocity = Vector3.zero;
        Look(data.dir);
        m_CurSkillId = SkillID.ShockWave;
        ChangeState(eState.ShockWave);
    }

    float m_CurSkillRange;
    private void OnMsg_FatalCircle(IBaseMsgData bmd)
    {
        FatalCircle_MsgData data = (FatalCircle_MsgData)bmd;
        transform.position = data.pos;
        m_Rigidbody.velocity = Vector3.zero;
        Look(data.dir);
        m_CurSkillId = SkillID.FatalCircle;
        m_CurSkillRange = data.range;
        ChangeState(eState.FatalCircle);
    }

    int m_ShadowDancePhase;
    int m_ShadowDancePhaseMax;
    int m_ShadowDanceHitCount;
    private void OnMsg_ShadowDance(IBaseMsgData bmd)
    {
        ShadowDance_MsgData data = (ShadowDance_MsgData)bmd;
        transform.position = data.pos;
        m_Rigidbody.velocity = Vector3.zero;
        Look(data.dir);
        m_CurSkillId = SkillID.ShadowDance;
        m_ShadowDancePhase = 0;
        m_ShadowDanceHitCount = data.hitCount;
        ChangeState(eState.ShadowDance);
    }

    public override void OnSkillMoveStart(float moveSpeed)
    {
        base.OnSkillMoveStart(moveSpeed);
        m_Rigidbody.velocity = GetDirection() * moveSpeed;
    }
    public override void OnSkillSlowStart(float animSpeed)
    {
        base.OnSkillSlowStart(animSpeed);
        animator.speed = animSpeed;
    }

    public override void OnSkillMoveEnd()
    {
        base.OnSkillMoveEnd();
        m_Rigidbody.velocity = Vector3.zero;
    }

    public override void OnSkillSlowEnd()
    {
        base.OnSkillSlowEnd();
        animator.speed = 1.0f;
    }

    public override void OnSkillEnd()
    {
        if(m_CurState == eState.PowerSlash && m_PowerSlashPhase == 0)
        {
            m_WeaponCol.enabled = false;
            m_PowerSlashPhase++;
            return;
        }
        if (m_CurState == eState.ShadowDance)
        {
            m_ShadowDancePhase++;
            m_WeaponCol.radius = 0f;
            m_WeaponCol.enabled = false;
            if(m_ShadowDancePhase > m_ShadowDancePhaseMax)
                base.OnSkillEnd();
            return;
        }
        base.OnSkillEnd();
    }
}
