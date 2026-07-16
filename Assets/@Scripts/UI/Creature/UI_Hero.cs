using UnityEngine;
using static Define;

public class UI_Hero : UI_Base
{
    enum Objects
    {
        //HeroArmor,
        HeroDamage,
    }

    Hero _hero;
    //UI_Armor _heroArmor;
    UI_Damage _heroDamage;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(Objects));

        //_heroArmor = GetObject((int)Objects.HeroArmor).GetOrAddComponent<UI_Armor>();
        //_heroArmor.Init();

        _heroDamage = GetObject((int)Objects.HeroDamage).GetOrAddComponent<UI_Damage>();
        _heroDamage.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = SortingLayers.COMBAT_UI;
        return true;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_hero.CenterPosition.x, _hero.CenterPosition.y + 20f);
    }

    public void SetHero(Hero hero)
    {
        _hero = hero;

        /*hero.OnArmorChanged += (armor) =>
        {
            _heroArmor.SetArmor(armor);
        };*/
        hero.OnDamageChanged += (damage) =>
        {
            _heroDamage.SetDamage(damage);
        };
    }
}
