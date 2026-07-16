using UnityEngine;
using static Define;

public class Sword_C_Airborne : SkillBase
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
        Owner.Rigidbody.linearVelocityY = CalculateAirborneVelocityY(Delay);
        base.DoSkill();
        Owner.StartCreatureCoroutine(SkillExcuting(ESkillMoveType.Step));
    }

    public float CalculateAirborneVelocityY(float targetAirTime)
    {
        Rigidbody2D rb = Owner.Rigidbody;

        float currentY = Owner.transform.position.y;
        float groundY = Owner.LastPosition.y;

        float g = Physics2D.gravity.y * rb.gravityScale;
        float t = targetAirTime;
        float v0 = (groundY - currentY - 0.5f * g * t * t) / t;

        return v0;
    }
}
