using UnityEngine;

public class BossTrigger : InitBase
{
    private UI_GameScene ui;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        ui = Managers.UI.GetSceneUI<UI_GameScene>();
        ui._bossBar.SetActive(true);
        Managers.Game.GameSceneUI.SetBoss(Managers.Object.Boss);
        Managers.Object.Boss.IsAggro = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        ui = Managers.UI.GetSceneUI<UI_GameScene>();
        ui._bossBar.SetActive(false);

        Managers.Object.Boss.IsAggro = false;
    }
}
