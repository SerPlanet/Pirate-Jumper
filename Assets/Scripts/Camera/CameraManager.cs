using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Transform cameraTransform;

    Vector3 startPos;

    float currentYOffset;
    float targetYOffset;

    float shakeX;
    float shakeY;

    [Header("Jump & Land Feel")]
    [SerializeField] float jumpUpOffset = 0.15f;   // weniger radikal beim Sprung
    [SerializeField] float landDownOffset = -0.25f; // sanfter Aufprall
    [SerializeField] float smoothSpeed = 8f;

    [Header("Idle Bob")]
    [SerializeField] float idleBobAmount = 0.05f;
    [SerializeField] float idleBobSpeed = 2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        startPos = cameraTransform.localPosition;
    }

    void LateUpdate()
    {
         // Idle bob / kleine Auf/Ab Bewegung
       // float idleOffset = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;

        // X-Pendelbewegung
        float camXOffset = Mathf.Sin(Time.time * 3f) * 0.03f;

        // Smooth Jump / Land offset
        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, smoothSpeed * Time.deltaTime);

        // Endposition Kamera
        Vector3 finalPos = startPos +
                        new Vector3(camXOffset + shakeX, currentYOffset + shakeY, 0);

        cameraTransform.localPosition = finalPos;
    }

    #region Jump Feel

    public void JumpCamera()
    {
        // sanft nach oben
        targetYOffset = jumpUpOffset;
    }

    public void LandCamera()
    {
        // kleine negative Bewegung für Impact
        targetYOffset = landDownOffset;
       // StartCoroutine(LandRoutine());
    }

    private IEnumerator LandRoutine()
    {
        // zuerst Down-Impact
        targetYOffset = landDownOffset;
        yield return new WaitForSeconds(0.08f); // Dauer des Impacts

        // zurück zu neutral
        targetYOffset = 0;
    }

    #endregion

    #region ScreenShake

    public void Shake(float duration, float strength)
    {
        StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        float timer = 0;

        while (timer < duration)
        {
            shakeX = Random.Range(-1f, 1f) * strength;
            shakeY = Random.Range(-1f, 1f) * strength;

            timer += Time.deltaTime;
            yield return null;
        }

        shakeX = 0;
        shakeY = 0;
    }

    #endregion
}