using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Counter : UI_Base
{
    enum Images
    {
        FirstCounter,
        SecondCounter,
        ThirdCounter,
    }

    private Image _first;
    private Image _second;
    private Image _third;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImages(typeof(Images));
        _first = GetImage((int)Images.FirstCounter);
        _second = GetImage((int)Images.SecondCounter);
        _third = GetImage((int)Images.ThirdCounter);

        return true;
    }

    public void SetCounter(int count)
    {
        count = Mathf.Clamp(count, 0, 3);

        _first.gameObject.SetActive(count >= 3);
        _second.gameObject.SetActive(count >= 2);
        _third.gameObject.SetActive(count >= 1);
    }
}
