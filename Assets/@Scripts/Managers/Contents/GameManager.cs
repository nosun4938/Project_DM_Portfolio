using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.XR;
using static Define;

public class GameManager
{
    // Map
    public EGameState GameState { get; private set; } = EGameState.Playing;
    public CameraController NowCamera {  get; set; }
    public UI_GameScene GameSceneUI { get; set; }
    public MapController NowMap { get; set; }

    // Player
    public PlayerData Player { get; set; } = new PlayerData();

    #region GameState
    public void EnterPause(EGameState pauseReason)
    {
        GameState = pauseReason;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        GameState = EGameState.Playing;
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        EnterPause(EGameState.Pause);
        //Managers.UI.ShowPopupUI<UI_Pause>()
    }

    public void Dialogue()
    {
        EnterPause(EGameState.Dialogue);
        //Managers.UI.ShowPopupUI<UI_Dialogue>().SetItem(item);
    }
    #endregion

    #region Hero
    protected Hero _hero;
    public virtual Hero NowHero
    {
        get { return _hero; }
        set
        {
            if (_hero != value)
            {
                _hero = value;
            }
        }
    }

    public float HeroMaxHp
    {
        get
        {
            if ( _hero != null )
                return _hero.MaxHp;
            return 0;
        }
    }

    protected Boss _boss;
    public virtual Boss NowBoss
    {
        get { return _boss; }
        set
        {
            if (_boss != value)
            {
                _boss = value;
            }
        }
    }

    #endregion

    #region Input Variables
    public bool IsZPressed { get; set; } = false;
    public bool IsAPressed { get; set; } = false;
    public bool IsSPressed { get; set; } = false;
    public bool IsDPressed { get; set; } = false;
    public bool IsFPressed { get; set; } = false;
    #endregion

    #region Input Event
    public void OnHeroInput(ESkillSlot slot)
    {
        switch (slot)
        {
            case ESkillSlot.Z:
                IsZPressed = true;
                break;
            case ESkillSlot.A:
                IsAPressed = true;
                break;
            case ESkillSlot.S:
                IsSPressed = true;
                break;
            case ESkillSlot.D:
                IsDPressed = true;
                break;
            case ESkillSlot.F:
                IsFPressed = true;
                break;
        }
    }
    #endregion

    #region Battle UI
    public UI_Hero _uiHero { get; set; }
    public UI_Boss _uiBoss { get; set; }
    public UI_Monster _uiMonster { get; set; }
    #endregion
}
