using UnityEngine;
using static Define;

public class Sword_Z_Idle_Third : SkillBase
{
    //int _dashDir;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public override void SetInfo(Creature owner, int skillTemplateID)
    {
        base.SetInfo(owner, skillTemplateID);
    }

    public override void DoSkill()
    {
        base.DoSkill();
        Owner.StartCreatureCoroutine(SkillExcuting(ESkillMoveType.Step));
    }
}
