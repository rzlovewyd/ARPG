using UnityEngine;
using System.Collections;
using Game.BaseClass;
using Game.Helper;

public class WarriorCtrl : BaseHeroCtrl
{
    protected Warrior m_Warrior;

    public override void Init(int id, string targetName)
    {
        base.Init(id, targetName);

        m_Warrior = (Warrior)m_OwnerScript;
    }

    public override void OnUseActiveSkill(SkillID skillId)
    {
        base.OnUseActiveSkill(skillId);
        if(skillId == SkillID.PowerSlash)
        {
            SendObjMsgHelper.SendMsg_PowerSlash(m_Warrior.Name, m_Warrior.Name, GetAttackDir(), m_Warrior.transform.position);
        }
        else if(skillId == SkillID.ShockWave)
        {
            SendObjMsgHelper.SendMsg_ShockWave(m_Warrior.Name, m_Warrior.Name, GetAttackDir(), m_Warrior.transform.position);
        }
        else if(skillId == SkillID.FatalCircle)
        {
            SendObjMsgHelper.SendMsg_FatalCircle(m_Warrior.Name, m_Warrior.Name, GetAttackDir(), m_Warrior.transform.position, 3);
        }
        else if(skillId == SkillID.ShadowDance)
        {
            SendObjMsgHelper.SendMsg_ShadowDance(m_Warrior.Name, m_Warrior.Name, GetAttackDir(), m_Warrior.transform.position, 3);
        }
    }
}