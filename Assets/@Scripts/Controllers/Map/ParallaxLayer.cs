using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxLayer : InitBase
{
    public float parallaxX;
    public float parallaxY;
    private Vector3 _startPos;
    private Vector3 _cameraPos;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(float X, float Y)
    {
        // 초기 위치 설정
        _startPos = transform.position;
        _cameraPos = Camera.main.transform.position;
        transform.position = new Vector3(_cameraPos.x, _startPos.y, _startPos.z);

        parallaxX = X;
        parallaxY = Y;

        ParallaxCamera.OnCameraMove += OnCameraMove;
    }

    void OnDisable()
    {
        ParallaxCamera.OnCameraMove -= OnCameraMove;
    }

    void OnDestroy()
    {
        ParallaxCamera.OnCameraMove -= OnCameraMove;
    }

    void OnCameraMove(Vector3 deltaMove, Vector3 offset)
    {
        Vector3 move = new Vector3(deltaMove.x * parallaxX, deltaMove.y * parallaxY, deltaMove.z);
        Vector3 correction = new Vector3(offset.x * parallaxX, offset.y * parallaxY, offset.z);
        transform.position += move; // + correction; Camera Controller와 Parallax Camera 모두 Pixel Perfect Clamp 중이라 사실상 0과 같은 상태
    }
}
