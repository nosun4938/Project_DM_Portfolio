using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using static Define;

public class BackgroundController : InitBase
{
    private Camera _camera;
    public Camera Camera
    {
        get { return _camera; }
        set { _camera = value; }
    }
    public float XFactor;
    public float YFactor;

    private Vector3 _prevCamPos;
    private Vector3 _startPos;
    private PixelPerfectCamera _ppc;
    private ParallaxLayer _parallaxLayer;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        if (_camera == null)
            _camera = Camera.main;
        
        _ppc = Camera.main.GetComponent<PixelPerfectCamera>();
        _parallaxLayer = gameObject.GetOrAddComponent<ParallaxLayer>();

        return true;
    }

    public void SetInfo(EBackgroundType type)
    {
        switch (type)
        {
            case EBackgroundType.Background:
                XFactor = 1.0f;
                YFactor = 0f;
                break;

            case EBackgroundType.Middleground:
                XFactor = 0.1f;
                YFactor = 0f;
                break;

            case EBackgroundType.Foreground:
                XFactor = 0f;
                YFactor = 0f;
                break;
        }

        _parallaxLayer.SetInfo(XFactor, YFactor);
    }
}
