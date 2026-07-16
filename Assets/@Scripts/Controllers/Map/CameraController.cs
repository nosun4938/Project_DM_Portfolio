using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraController : InitBase
{
    private BaseObject _target;
    public BaseObject Target
    {
        get { return _target; }
        set { _target = value; }
    }

    private Bounds _bounds;
    public Bounds MapBound
    {
        get { return _bounds; }
        set { _bounds = value; }
    }

    private Camera _cam;
    private PixelPerfectCamera _camera;
    private ParallaxCamera _parallaxCamera;

    private float _halfWidth;
    private float _halfHeight;

    private bool _isLocked = false;
    private bool _isMovingToLock = false;
    private Vector3 _lockPosition;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Game.NowCamera = this;

        _cam = Camera.main;
        _camera = _cam.GetComponent<PixelPerfectCamera>();
        _camera.assetsPPU = 4;
        _camera.refResolutionX = 640;
        _camera.refResolutionY = 360;
        _camera.gridSnapping = PixelPerfectCamera.GridSnapping.UpscaleRenderTexture;

        _parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();
        _parallaxCamera.assetsPPU = 4;

        return true;
    }

    private void LateUpdate()
    {
        if (_isMovingToLock)
            return;

        if (_isLocked)
        {
            transform.position = PixelPerfectClamp(_lockPosition);
            return;
        }

        if (Target == null)
            return;

        CalculateCameraSize();
        float BoundedX = Mathf.Clamp(Target.CenterPosition.x, _bounds.min.x + _halfWidth, _bounds.max.x - _halfWidth);
        float BoundedY = Mathf.Clamp(Target.CenterPosition.y + 22f, _bounds.min.y + _halfHeight, _bounds.max.y - _halfHeight);

        Vector3 targetPosition = new Vector3(BoundedX, BoundedY, -10f);
        transform.position = PixelPerfectClamp(targetPosition);
    }

    private Vector3 PixelPerfectClamp(Vector3 position)
    {
        if (_camera == null)
            return position;

        float unitsPerPixel = 1f / _camera.assetsPPU;

        position.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
        position.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;

        return position;
    }

    private void CalculateCameraSize()
    {
        _halfHeight = _cam.orthographicSize;
        _halfWidth = _halfHeight * _cam.aspect;
    }

    public void SmoothLockCamera(Vector3 position, float duration = 0.5f)
    {
        StartCoroutine(CoSmoothLock(position, duration));
    }

    private IEnumerator CoSmoothLock(Vector3 targetPos, float duration)
    {
        _isMovingToLock = true;

        Vector3 startPos = transform.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float easedT = 1 - Mathf.Pow(1 - t, 3);

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, easedT);
            transform.position = newPos;
            yield return null;
        }

        _lockPosition = PixelPerfectClamp(targetPos);

        _isMovingToLock = false;
        _isLocked = true;

        transform.position = _lockPosition;
    }

    public void UnlockCamera()
    {
        _isLocked = false;
    }
}
