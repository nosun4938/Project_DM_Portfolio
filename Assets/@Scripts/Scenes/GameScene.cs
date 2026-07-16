using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Define;
using static Util;

public class GameScene : BaseScene
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

        SceneType = EScene.GameScene;

        // Hero, 가장 먼저 생성해야함 
        Hero hero = Managers.Object.Spawn<Hero>(new Vector3Int(0, 0, 0), 202001);

        // Map
        MapController mc = Managers.Map.SpawnMap("1000001");
        
        hero.transform.position = mc.Tilemap.GetCellCenterWorld(new Vector3Int(5, 0, 0)); //(5, 41, 0) BossMap 바로 앞
        hero.InitVector = mc.Tilemap.GetCellCenterWorld(new Vector3Int(5, 0, 0));

        // Camera, Hero랑 Map 후에 생성해야함
        CameraController camera = Camera.main.GetComponent<CameraController>();
        camera.Target = hero;
        camera.MapBound = mc.Bounds;

        // UI
        UI_GameScene ui = Managers.UI.ShowSceneUI<UI_GameScene>();
        if (ui == null)
            Debug.LogError("ui is null");

        Managers.Game.GameSceneUI = ui;
        ui.SetHero(hero);
        //ui.SetBoss(boss);

        // Sound
        Managers.Sound.Play(ESound.Bgm, "Maniac", volume: 0.3f);

        return true;
    }

    public override void Clear()
    {
        
    }
}
