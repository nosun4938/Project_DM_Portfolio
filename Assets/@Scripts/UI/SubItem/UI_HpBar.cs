using UnityEngine;
using UnityEngine.UIElements;

public class UI_HpBar : UI_Base
{
    enum Images
    {
        Background,
        HitBar,
        HpFill,
        HpBar,
        HpBorder,
    }

    float _targetFill = 1f;
    float _backFill = 1f;
    float maxSize = 400;

    RectTransform BackgroundRect;
    Vector2 BackgroundSize;

    RectTransform HitBarRect;
    Vector2 HitBarSize;

    RectTransform HpFillRect;
    Vector2 HpFillSize;

    RectTransform HpBorderRect;
    Vector2 HpBorderSize;

    RectTransform HpBarRect;
    Vector2 HpBarSize;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));

        HpBarRect = GetImage((int)Images.HpBar).rectTransform;
        HpBarSize = HpBarRect.sizeDelta;

        BackgroundRect = GetImage((int)Images.Background).rectTransform;
        BackgroundSize = BackgroundRect.sizeDelta;

        HitBarRect = GetImage((int)Images.HitBar).rectTransform;
        HitBarSize = HitBarRect.sizeDelta;

        HpFillRect = GetImage((int)Images.HpFill).rectTransform;
        HpFillSize = HpFillRect.sizeDelta;

        HpBorderRect = GetImage((int)Images.HpBorder).rectTransform;
        HpBorderSize = HpBorderRect.sizeDelta;
        return true;
    }

    private void Update()
    {
        _backFill = Mathf.Lerp(_backFill, _targetFill, Time.deltaTime * 5f);
        GetImage((int)Images.HitBar).fillAmount = _backFill;
    }
    public void SetSize(float maxHp)
    {
        GetImage((int)Images.HpFill).fillAmount = 1;
        Vector2 newSize = HpBarSize;
        float extraProportion = (maxHp / 30) / 1;

        newSize.x *= extraProportion;
        newSize.x = Mathf.Clamp(newSize.x, HpBarSize.x, HpBarSize.x + maxSize);
        newSize.x = Mathf.FloorToInt(newSize.x);

        if (extraProportion > 1)
        {
            Transform barMask = HpBarRect.parent;
            Vector2 pos = HpBarRect.localPosition;

            HpBarRect.SetParent(null);
            HpBarRect.sizeDelta = newSize;

            float extraWidth = newSize.x - HpBarSize.x;

            BackgroundRect.SetWidth(BackgroundSize.x + extraWidth);
            HitBarRect.SetWidth(HitBarSize.x + extraWidth);
            HpFillRect.SetWidth(HpFillSize.x + extraWidth);
            HpBorderRect.SetWidth(HpBorderSize.x + extraWidth);

            HpBarRect.SetParent(barMask);
            HpBarRect.localPosition = pos;
        }
    }
    
    public void SetHp(float hp, float maxHp)
    {
        _targetFill = hp / maxHp;
        GetImage((int)Images.HpFill).fillAmount = _targetFill;
    }

    public float AdjustX(float maxHp)
    {
        Vector2 newSize = HpBarSize;
        float extraProportion = (maxHp / 30) / 1;
        newSize.x *= extraProportion;

        return (newSize.x / 2 - 23) * 3;
    }

}
