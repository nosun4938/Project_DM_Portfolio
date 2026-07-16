using UnityEngine;

public class UI_SkillSlot : UI_Base
{
    enum Images
    {
        Icon,
        CooldownOverlay,
    }

    enum Texts
    {
        CooldownText,
    }

    SkillBase _skill;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        return true;
    }

    public void SetSkill(SkillBase skill)
    {
        _skill = skill;
    }

    private void Update()
    {
        if (_skill == null)
            return;

        float max = _skill.SkillData.CoolTime;
        float remain = _skill.RemainCoolTime;

        float ratio = (max > 0) ? remain / max : 0f;
        GetImage((int)Images.CooldownOverlay).fillAmount = ratio;

        if (remain > 0)
        {
            GetText((int)Texts.CooldownText).text = Mathf.Ceil(remain).ToString();
        }
        else
        {
            GetText((int)Texts.CooldownText).text = "";
        }

        GetImage((int)Images.Icon).color = (remain > 0) ? Color.gray : Color.white;
    }
}
