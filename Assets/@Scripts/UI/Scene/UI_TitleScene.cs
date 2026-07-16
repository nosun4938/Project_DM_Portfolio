using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_TitleScene : UI_Scene
{
    private bool _isLoading = false;

    enum Texts
    {
        DisplayText
    }

    enum Buttons
    {
        StartButton,
        ExitButton,
        //LoadButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.StartButton).BindEvent(over: (evt) =>
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        });
        GetButton((int)Buttons.ExitButton).BindEvent(over: (evt) =>
        {
            EventSystem.current.SetSelectedGameObject(this.gameObject);
        });


        GetButton((int)Buttons.StartButton).BindEvent( 
        click: (evt) =>
        {
            Debug.Log("TitleScene => GameScene : Click");
            Debug.Log("--------------------------------------------------");

            if (_isLoading)
            return;
            _isLoading = true;

            Managers.Scene.LoadScene(EScene.GameScene);
        },
        enter: () =>
        {
            if (EventSystem.current.alreadySelecting)
            {
                Debug.Log("TitleScene => GameScene : Enter");
                Debug.Log("--------------------------------------------------");

                if (_isLoading)
                    return;
                _isLoading = true;

                Managers.Scene.LoadScene(EScene.GameScene);
            }

            else
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject);
            }
        });

        GetButton((int)Buttons.ExitButton).BindEvent(
        click: (evt) =>
        {
            Application.Quit();
        },
        enter: () =>
        {
            if (_isLoading)
                return;
            _isLoading = true;

            Application.Quit();
        });


        StartCoroutine(oneFrameAfter());

        return true;
    }

    IEnumerator oneFrameAfter()
    {
        yield return null;
        EventSystem.current.firstSelectedGameObject = GetButton((int)Buttons.StartButton).gameObject;
    }
}
