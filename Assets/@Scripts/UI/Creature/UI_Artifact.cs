using UnityEngine;
using static Define;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class UI_Artifact : UI_Base
{
    enum Objects
    {
        ArtifactArmor,
        HitCounter
    }

    Artifact _artifact;
    UI_Armor _artifactArmor;
    UI_Counter _artifactCounter;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(Objects));

        _artifactArmor = GetObject((int)Objects.ArtifactArmor).GetOrAddComponent<UI_Armor>();
        _artifactArmor.Init();

        _artifactCounter = GetObject((int)Objects.HitCounter).GetOrAddComponent<UI_Counter>();
        _artifactCounter.Init();

        Canvas canvas = GetComponent<Canvas>();
        canvas.sortingOrder = SortingLayers.COMBAT_UI;
        return true;
    }

    private void LateUpdate()
    {
        transform.position = transform.position = new Vector3(_artifact.CenterPosition.x, _artifact.CenterPosition.y + 28f);
    }

    public void SetArtifact(Artifact artifact)
    {
        _artifact = artifact;
        
        if (_artifact != null)
        {
            _artifact.OnArmorChanged += HandleArmorChanged;
            _artifact.OnCounterChanged += HandleCounterChanged;
        }
    }

    private void HandleArmorChanged (float armor)
    {
        _artifactArmor.SetArmor(armor);
    }

    private void HandleCounterChanged (int count)
    {
        _artifactCounter.SetCounter(count);
    }

    private void OnDestroy()
    {
        if (_artifact != null)
        {
            _artifact.OnArmorChanged -= HandleArmorChanged;
            _artifact.OnCounterChanged -= HandleCounterChanged;
        }
    }
}
