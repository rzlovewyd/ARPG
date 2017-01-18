public enum eCmd
{
    Idle,           //待机
    Move,
    MoveTo,
    RushMoveTo,
    Turn,
    Attacked,
    AttackedEff,
    Die,            //死亡
    CreateBullet,
    CreateBullets,
    CreateMonsters,
    MonSpawnStart,
    MonSpawnEnd,
    Buff,
    DeBuff,
    LevelUp,        //升级
    Rush,           //急促移动
    RushAttack,     //急冲攻击
    Rebirth,         //重生
    Attack,         //普通攻击
    DashAttack,     //冲撞攻击
    PowerSlash,     //猛砍
    FreezeSlash,    //
    NearAttack,     //近战攻击
    ShootAttack,    //远程攻击
    DropBulletAttack,
    PoisonBall,     //
    WhirlWind,      //旋风斩
    WhirlWindMove,
    ElectricWind,
    ElectricWindMove,
    ShockWave,      //冲击流
    ShadowDance,    //影舞
    FatalCircle,    //夺命圈
    //待补充

    //Archer Skill
    MultiShot,
    ArrowShower,
    ArrowShowerTurn,
    ExitShot,
    LeopardTech,

    //Assassin Skill
    RotationAttack,
    JumpBreaker,
    BlinkStab,
    //MillionStab,
    //HelmBreaker,
    //SwordStinger,
    //SwordDrive,

    //Wizard Skill
    Meteorites_Rain,
    Spirit_Bomb,
    Ice_Wall,
    Thunder_Bolt,


    //主城动作，移动，静止
    MTIdle,
    MTMove,

    //沙漠Boss技能
    SandStorm,
    MultiPunch,
}

public enum SkillID
{
    //战士
    PowerSlash,    //猛击
    ShockWave,     //电击波
    FatalCircle,   //致命圈
    ShadowDance,   //影舞
}
