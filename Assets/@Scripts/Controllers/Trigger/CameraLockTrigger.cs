using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CameraLockTrigger : InitBase
{
    // Trigger 이름을 TemplateID로 해놔야함
    public int templateID;
    
    public Data.CameraData CameraData { get; private set; }

    private Vector3 lockPosition;
    private float moveDuration = 0.5f;
    private bool unlockOnExit = false;

    private CameraController _cameraController;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        templateID = int.Parse(gameObject.name);

        _cameraController = Camera.main.GetComponent<CameraController>();
        SetInfo(templateID);

        return true;
    }

    public void SetInfo(int templateID)
    {
        CameraData = Managers.Data.CameraDic[templateID];

        lockPosition = new Vector3(CameraData.VectorX, CameraData.VectorY, -10);
        moveDuration = CameraData.Duration;
        unlockOnExit = CameraData.ExitUnlock;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        _cameraController.SmoothLockCamera(lockPosition, moveDuration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (unlockOnExit == false)
            return;

        if (collision.CompareTag("Player") == false)
            return;

        _cameraController.UnlockCamera();
    }
}
