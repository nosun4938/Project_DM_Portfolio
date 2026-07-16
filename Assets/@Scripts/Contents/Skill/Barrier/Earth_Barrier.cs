using UnityEngine;
using static Define;

public class Earth_Barrier : SkillBase
{
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
        Owner.StartCreatureCoroutine(SkillExcuting(ESkillMoveType.Dash));
    }
}
