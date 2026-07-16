using UnityEngine;
using static Define;

public class Axe_D_Idle : SkillBase
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
        Managers.Sound.Play(ESound.Effect, "Straight", volume: 0.5f);
        base.DoSkill();
        Owner.StartCreatureCoroutine(SkillExcuting(ESkillMoveType.Dash));
    }
}
