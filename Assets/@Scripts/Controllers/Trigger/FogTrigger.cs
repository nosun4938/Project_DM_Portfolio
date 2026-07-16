using UnityEngine;

public class FogTrigger : InitBase
{
    public bool FogAgain;
    public GameObject Fog;
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

        Fog.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        if (FogAgain)
            Fog.gameObject.SetActive(true);
    }
}
