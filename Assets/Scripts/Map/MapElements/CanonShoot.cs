using System.Collections;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine;

public class CanonShoot : MonoBehaviour
{
     [Header("Cannon Settings")]
    [SerializeField] private List<Transform> canonBalls; // Kugeln zum Wiederverwenden
    [SerializeField] private float timeBetweenCannonBalls = 1.5f; 
    [SerializeField] private float speedCanonBalls = 10f;
    [SerializeField] private AudioClip canonShoot;

    [SerializeField] private BoxCollider2D boxcollider2D;

    [Header("Cannon Squash")]
    [SerializeField] private Vector3 shootSquashScale = new Vector3(0.8f, 1.2f, 1f); // X größer, Y kleiner
    [SerializeField] private float squashDuration = 0.1f;

    [SerializeField] private Transform squashVisual;

    private Vector3 originalScale;

    private List<Transform> inUseCanonBalls = new List<Transform>();
    private bool playerInRange = false;

   
    private int amountOfShots, countShots;

    private void Awake()
    {
        originalScale = squashVisual.localScale;
        amountOfShots = Random.Range(2, canonBalls.Count);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<MovementScript>(out MovementScript player))
        {
            Debug.Log(playerInRange + "Shoot");
            boxcollider2D.enabled = false;
            playerInRange = true;
            if (playerInRange)
            {
                StartCoroutine(StartCanonBall());
            }
            
        }
    }


    private IEnumerator StartCanonBall()
    {
        while (playerInRange)
        {
            // Finde die erste freie Kugel
            Transform ball = canonBalls.Find(b => !inUseCanonBalls.Contains(b));
            if(ball != null)
            {
                inUseCanonBalls.Add(ball);
                //ball.position = transform.position;
                ball.gameObject.SetActive(true);
                

                // Audio abspielen
                if(canonShoot != null)
                    AudioManager.Instance.PlayenvironmentAudi(canonShoot,1f,1f);

                 // Kanone squashen
                StartCoroutine(CannonSquashSmooth());
                // Kugel in Bewegung setzen
                StartCoroutine(MoveBall(ball));
                timeBetweenCannonBalls = Random.Range(1.5f,2f);
                countShots ++;
                if(countShots >= amountOfShots) playerInRange = false;
                // Warte Zeit zwischen Schüssen
                yield return new WaitForSeconds(timeBetweenCannonBalls);
            }
            else
            {
                // Wenn keine Kugel frei, warte einen Frame
                yield return null;
            }
        }
    }

    private IEnumerator MoveBall(Transform ball)
    {
        Debug.Log("MoveBall");
        Vector3 direction = Vector3.left; // Richtung nach links, ändere falls nötig
        while (ball.gameObject.activeInHierarchy)
        {
            ball.position += direction * speedCanonBalls * Time.deltaTime;

            // Kugel deaktivieren, wenn sie weit genug ist
            if (Vector3.Distance(transform.position, ball.position) > 100f)
            {
                ball.gameObject.SetActive(false);
                inUseCanonBalls.Remove(ball);
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CannonSquashSmooth()
    {
        float time = 0f;
        Vector3 startScale = squashVisual.localScale;
        while(time < squashDuration)
        {
            squashVisual.localScale = Vector3.Lerp(shootSquashScale, originalScale, time / squashDuration);
            time += Time.deltaTime;
            yield return null;
        }
        squashVisual.localScale = originalScale;
    }

}
