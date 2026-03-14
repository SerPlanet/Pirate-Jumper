using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] int pixelsPerUnit = 100;
    float pixelSize;

    Vector3 startPos;

    float currentYOffset;
    float targetYOffset;

    float shakeX;
    float shakeY;

    private int referenceWidth = 320;   // logische Breite
    private int referenceHeight = 180;  // logische Höhe

    [Header("Jump & Land Feel")]
    [SerializeField] float jumpUpOffset = 8f;   // weniger radikal beim Sprung
    [SerializeField] float landDownOffset = -12f; // sanfter Aufprall
    [SerializeField] float smoothSpeed = 8f;

    [Header("Idle Bob")]
    [SerializeField] float idleBobAmount = 0.05f;
    [SerializeField] float idleBobSpeed = 2f;

    [SerializeField] private GameObject testBackground;
    private bool testBackgroundBool = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        startPos = cameraTransform.localPosition;
        pixelSize = 1f / pixelsPerUnit;
    }

    void LateUpdate()
    {
        // X Bewegung in Pixeln
        float camXOffset = Mathf.Sin(Time.time * 3f) * 3f; // 3 Pixel Bewegung

        // Jump/Land smoothing
        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, smoothSpeed * Time.deltaTime);

        // Pixel in World Units umrechnen
        float x = Mathf.Round(camXOffset) * pixelSize;
        float y = Mathf.Round((currentYOffset + shakeY)) * pixelSize;

        Vector3 finalPos = startPos + new Vector3(x + shakeX * pixelSize, y, 0);

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
            shakeX = Random.Range(-2f, 2f) * strength;
            shakeY = Random.Range(-2f, 2f) * strength;

            timer += Time.deltaTime;
            yield return null;
        }

        shakeX = 0;
        shakeY = 0;
    }

    #endregion

    #region Testing

    public void TriggerBackgreoundSwitch()
    {
        testBackgroundBool = !testBackgroundBool;
        testBackground.SetActive(testBackgroundBool);
    }
    #endregion
}