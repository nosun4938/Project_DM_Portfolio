using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class TitleScene : BaseScene
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Object obj = GameObject.FindAnyObjectByType(typeof(EventSystem));
        if (obj == null)
        {
            GameObject go = new GameObject() { name = "@EventSystem" };
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }

        SceneType = Define.EScene.TitleScene;

        // Camera
        CameraController camera = Camera.main.GetComponent<CameraController>();

        // Background
        GameObject background = Managers.Resource.Instantiate("Background");

        // Sound
        Managers.Sound.Init();
        Managers.Sound.Play(ESound.Bgm, "Hurt_and_heart", volume: 0.3f);

        return true;
    }

    void StartLoadAssets()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                //TODO
                //Managers.Data.Init();
            }
        });
    }

    public override void Clear()
    {

    }
}
