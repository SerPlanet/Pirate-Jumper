using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [SerializeField] private Collider2D magnetCollider;

    [SerializeField] private Transform parentTransfrom;

    private bool isMagnetAktiv, magnetIsUsed;

    private void OnEnable()
    {
        magnetIsUsed = false;
        GameManager.MagnetIsActive += checkForMagnet;
        isMagnetAktiv = GameManager.Instance.GetIsMagnetAktiv();
        checkForMagnet(isMagnetAktiv);
    }

    private void OnDestroy()
    {
        GameManager.MagnetIsActive -= checkForMagnet;
    }
    private void OnDisable()
    {
        magnetIsUsed = false;
        checkForMagnet(isMagnetAktiv);
        GameManager.MagnetIsActive -= checkForMagnet;
    }

    private void checkForMagnet(bool obj)
    {
        isMagnetAktiv = obj;
        magnetIsUsed = obj;
        magnetCollider.enabled= obj;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isMagnetAktiv)
        {
            if (magnetIsUsed)
            {
                magnetIsUsed=!magnetIsUsed;
                
                 StartCoroutine(MoveToPlayer());
            }
        }     
    }

    IEnumerator MoveToPlayer()
    {
        float speed = 30f; // Geschwindigkeit wie schnell Coin zum Spieler fliegt
        Transform player = GameManager.Instance.GetPlayer().transform;

        while (Vector2.Distance(transform.position, player.position) > 0.1f)
        {
            // Coin bewegt sich Richtung Spieler
            parentTransfrom.position = Vector2.MoveTowards(parentTransfrom.position, player.position, speed * Time.deltaTime);
            yield return null; // Warte auf nächsten Frame
        }

        isMagnetAktiv = false;
        // Optional: Coin deaktivieren / recyceln, wenn Pool benutzt wird
        gameObject.SetActive(false);
    }
}
