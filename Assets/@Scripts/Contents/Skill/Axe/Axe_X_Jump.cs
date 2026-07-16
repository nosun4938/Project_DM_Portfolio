using UnityEngine;
using static Define;

public class Axe_X_Jump : SkillBase
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
        Owner.StartCreatureCoroutine(BossAirSkillExcuting(12f));
    }
}
