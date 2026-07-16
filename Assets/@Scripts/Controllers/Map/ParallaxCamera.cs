using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ParallaxCamera : InitBase
{
    public static Action<Vector3, Vector3> OnCameraMove;

    private Vector3 _prevCamPos;
    public int assetsPPU = 4;
    private float UnitsPerPixel;

    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        UnitsPerPixel = 1f / assetsPPU;
        _prevCamPos = transform.position;

        return true;
    }

    private void LateUpdate()
    {
        Vector3 realPos = transform.position;
        Vector3 snappedPos = Clamp(realPos);

        Vector3 deltaMove = realPos - _prevCamPos;
        Vector3 offset = realPos - snappedPos;

        if (deltaMove.sqrMagnitude > 0f)
            OnCameraMove?.Invoke(deltaMove, offset);

        _prevCamPos = realPos;
    }

    public Vector3 Clamp(Vector3 position)
    {
        float unit = UnitsPerPixel;
        position.x = Mathf.Round(position.x / unit) * unit;
        position.y = Mathf.Round(position.y / unit) * unit;

        return position;
    }
}
