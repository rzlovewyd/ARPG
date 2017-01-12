using UnityEngine;
using System.Collections;
using Game.BaseClass;

public class Warrior : BaseHero 
{
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
    }
}
