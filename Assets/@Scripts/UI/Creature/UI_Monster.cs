using UnityEngine;
using static Define;

public class UI_Monster : UI_Base
{
    enum Objects
    {
        MonsterArmor,
    }

    Monster _monster;
    UI_Armor _monsterArmor;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(Objects));

        _monsterArmor = GetObject((int)Objects.MonsterArmor).GetOrAddComponent<UI_Armor>();
        _monsterArmor.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = SortingLayers.COMBAT_UI;
        return true;
    }

    private void LateUpdate()
    {
        transform.position = transform.position = new Vector3(_monster.CenterPosition.x, _monster.CenterPosition.y + 20f);
    }

    public void SetMonster(Monster monster)
    {
        _monster = monster;

        monster.OnArmorChanged += (armor) =>
        {
            _monsterArmor.SetArmor(armor);
        };
    }
}
