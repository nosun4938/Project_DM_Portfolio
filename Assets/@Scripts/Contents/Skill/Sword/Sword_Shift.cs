using Data;
using System.Collections;
using UnityEditor.Experimental;
using UnityEngine;
using static Define;

public class Sword_Shift : SkillBase
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

    private IEnumerator BossShift(Vector2 targetPos, float dashTime)
    {
        Vector2 endPos = targetPos;

        Vector2 startPos = Owner.transform.position;
        if (targetPos.x - startPos.x > 0)
            endPos.x = targetPos.x - 1.0f;
        else
            endPos.x = targetPos.x + 1.0f;
        endPos.y = startPos.y;

        float elapsed = 0f;

        while (elapsed < dashTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dashTime;

            float easedT = Mathf.SmoothStep(0f, 1f, t);

            Vector2 newPos = Vector2.Lerp(startPos, endPos, easedT);

            Owner.Rigidbody.MovePosition(newPos);

            yield return null;
        }

        Owner.Rigidbody.MovePosition(endPos);
        Owner.OnSkillEnd(this);
    }
}

