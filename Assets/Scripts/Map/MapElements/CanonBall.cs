using UnityEngine;

public class CanonBall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //GameManager.Instance.GetPlayerScript().CanonHit();
    }


   private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.GetPlayerScript().CanonHit();
    } 
}
