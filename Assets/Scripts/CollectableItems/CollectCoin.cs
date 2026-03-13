using System.Collections;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] AudioClip collectCoinAudiClip;
    [SerializeField] int amountWorth = 1;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject coinMagnet;





    private void OnTriggerEnter2D(Collider2D collision)
    {
            CollCoin();
    }


    private void CollCoin()
    {
        coinMagnet.SetActive(false);
        GameManager.Instance.AddAmountMoneyIngame(amountWorth);
        AudioManager.Instance.PlayCoinCollectSound();
        animator.SetTrigger("Collected");
    }


}
