using TMPro;
using UnityEngine;

public class UI_Armor : UI_Base
{
    enum Images
    {
        ArmorIcon,
    }

    enum Texts
    {
        ArmorLevel,
    }

    private TMP_Text _armorText;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        _armorText = GetText((int)Texts.ArmorLevel);
        return true;
    }

    public void SetArmor(float armor)
    {
        if (armor < 10)
            _armorText.text = Mathf.RoundToInt(armor).ToString();
        else
            _armorText.text = "X";
    }
}
