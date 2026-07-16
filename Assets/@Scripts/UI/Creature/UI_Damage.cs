using TMPro;
using UnityEngine;

public class UI_Damage : UI_Base
{
    enum Images
    {
        DamageIcon,
    }

    enum Texts
    {
        DamageLevel,
    }

    private TMP_Text _damageText;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        _damageText = GetText((int)Texts.DamageLevel);
        return true;
    }

    public void SetDamage(float damage)
    {
        _damageText.text = Mathf.RoundToInt(damage).ToString();
    }
}
