using UnityEngine;
using static Define;

public class UI_GameScene : UI_Scene
{
    enum Objects
    {
        HeroArmor,
        HeroDamage,
        HeroHPBar,
        BossHPBar,

        ShiftSlot,
        ZSlot,
        XSlot,
        ASlot,
        SSlot,
        CSlot,
    }

    UI_Armor _heroArmor;
    UI_Damage _heroDamage;

    UI_HpBar _heroHpBar;

    public GameObject _bossBar;
    UI_HpBar _bossHpBar;
    RectTransform _bossHpBarRect;
    Vector2 _bossHpBarInitPosition;

    UI_SkillSlot _shiftSlot;
    UI_SkillSlot _zSlot;
    UI_SkillSlot _xSlot;
    UI_SkillSlot _aSlot;
    UI_SkillSlot _sSlot;
    UI_SkillSlot _cSlot;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetSceneUI(this);
        BindObjects(typeof(Objects));

        _heroArmor = GetObject((int)Objects.HeroArmor).GetOrAddComponent<UI_Armor>();
        _heroDamage = GetObject((int)Objects.HeroDamage).GetOrAddComponent<UI_Damage>();

        _heroHpBar = GetObject((int)Objects.HeroHPBar).GetOrAddComponent<UI_HpBar>();

        _bossBar = GetObject((int)Objects.BossHPBar);
        _bossHpBar = _bossBar.GetOrAddComponent<UI_HpBar>();
        _bossHpBarRect = GetObject((int)Objects.BossHPBar).GetOrAddComponent<RectTransform>();
        _bossHpBarInitPosition = _bossHpBarRect.anchoredPosition;
        GetObject((int)Objects.BossHPBar).SetActive(false);

        _shiftSlot = GetObject((int)Objects.ShiftSlot).GetOrAddComponent<UI_SkillSlot>();
        _zSlot = GetObject((int)Objects.ZSlot).GetOrAddComponent<UI_SkillSlot>();
        _xSlot = GetObject((int)Objects.XSlot).GetOrAddComponent<UI_SkillSlot>();
        _aSlot = GetObject((int)Objects.ASlot).GetOrAddComponent<UI_SkillSlot>();
        _sSlot = GetObject((int)Objects.SSlot).GetOrAddComponent<UI_SkillSlot>();
        _cSlot = GetObject((int)Objects.CSlot).GetOrAddComponent<UI_SkillSlot>();

        Refresh();

        return true;
    }

    void Refresh()
    {
        if (_init == false)
            return;
    }

    public void SetHero(Hero hero)
    {
        _heroHpBar.SetSize(hero.MaxHp);

        Managers.Object.Player.OnHpChanged += (hp, maxHp) =>
        {
            _heroHpBar.SetHp(hp, maxHp);
        };

        hero.OnArmorChanged += (armor) =>
        {
            _heroArmor.SetArmor(armor);
        };
        hero.OnDamageChanged += (armor) =>
        {
            _heroDamage.SetDamage(armor);
        };

        _shiftSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Idle, ESkillSlot.Shift, 0)]);
        _zSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Dash, ESkillSlot.Z, 0)]);
        _xSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Idle, ESkillSlot.X, 0)]);
        _aSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Idle, ESkillSlot.A, 0)]);
        _sSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Idle, ESkillSlot.S, 0)]);
        _cSlot.SetSkill(hero.Skills._skillMap[new SkillKey(hero.CreatureWeapon, ECreatureState.Airborne, ESkillSlot.C, 0)]);
        Refresh();
    }

    public void SetBoss(Boss boss)
    {
        _bossHpBar.SetSize(boss.MaxHp);

        boss.OnHpChanged += (hp, maxHp) =>
        {
            _bossHpBar.SetHp(hp, maxHp);
        };

        Vector2 newPos = _bossHpBarInitPosition;
        //newPos.x -= _bossHpBar.AdjustX(boss.MaxHp);
        _bossHpBarRect.anchoredPosition = newPos;
        Refresh();
    }
}
