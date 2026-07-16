using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class LoadScene : BaseScene
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

        SceneType = Define.EScene.LoadScene;

        return true;
    }

    public override void Clear()
    {

    }
}
