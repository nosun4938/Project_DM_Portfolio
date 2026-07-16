using UnityEngine;

public class WallTrigger : InitBase
{
    public bool WallAgain;
    public GameObject Wall;
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

        Wall.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        if (WallAgain)
            Wall.gameObject.SetActive(false);
    }
}
