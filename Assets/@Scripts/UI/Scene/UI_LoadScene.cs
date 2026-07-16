using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_LoadScene : UI_Scene
{
    enum GameObjects
    {
        StartImage
    }

    enum Texts
    {
        DisplayText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));

        GetObject((int)GameObjects.StartImage).BindEvent(
        click: (evt) =>
        {
            //Debug.Log("LoadScene => TitleScene : Click");
            Managers.Scene.LoadScene(EScene.TitleScene);
        },
        anyKey: () =>
        {
            //Debug.Log("LoadScene => TitleScene : AnyKey");
            Managers.Scene.LoadScene(EScene.TitleScene);
        });

        GetObject((int)GameObjects.StartImage).gameObject.SetActive(false);
        GetText((int)Texts.DisplayText).text = $"";

        StartLoadAssets();

        return true;
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            //Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                Managers.Data.Init();

                GetObject((int)GameObjects.StartImage).gameObject.SetActive(true);
                GetText((int)Texts.DisplayText).text = "Press Anykey";
            }
        });
    }
}