using UnityEngine;
using static Define;

public class UI_Boss : UI_Base
{
    enum Objects
    {
        BossArmor,
        //BossDamage,
    }

    Boss _boss;
    UI_Armor _bossArmor;
    //UI_Damage _bossDamage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(Objects));

        _bossArmor = GetObject((int)Objects.BossArmor).GetOrAddComponent<UI_Armor>();
        _bossArmor.Init();

        //_bossDamage = GetObject((int)Objects.BossDamage).GetOrAddComponent<UI_Damage>();
        //_bossDamage.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = SortingLayers.COMBAT_UI;
        return true;
    }

    private void LateUpdate()
    {
        transform.position = transform.position = new Vector3(_boss.CenterPosition.x, _boss.CenterPosition.y + 20f);
    }

    public void SetBoss(Boss boss)
    {
        _boss = boss;

        boss.OnArmorChanged += (armor) =>
        {
            _bossArmor.SetArmor(armor);
        };
        /*boss.OnDamageChanged += (armor) =>
        {
            _bossDamage.SetDamage(armor);
        };*/
    }
}
