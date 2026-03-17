using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BouncingMatress : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpForce;

    [SerializeField] private AudioClip bounceSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.TryGetComponent<MovementScript>(out MovementScript player))
        {
            AudioManager.Instance.PlayenvironmentAudi(bounceSound,1.1f,0.9f);

            transform.DOScale(new Vector3(0.6f,0.6f,1f), 0.2f).SetLoops(2, LoopType.Yoyo);
            Debug.Log("MatUse");

            player.AddJumpForce(jumpForce);

            MapManager.Instance.SlowDownBounce(8f);
        }
    }
}
