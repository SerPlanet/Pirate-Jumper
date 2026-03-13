using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SparkelEffekt : MonoBehaviour
{
    [SerializeField] private List<Image> sparkles; // deine 7 Sparkle UI Images
    [SerializeField] private float alphaMin = 0f;
    [SerializeField] private float alphaMax = 1f;
    [SerializeField] private float minDuration = 0f;
    [SerializeField] private float maxDuration = 1.2f;

    [SerializeField] private bool isClockWise;

    private void Start()
    {
        foreach (Image img in sparkles)
        {
            AnimateSparkle(img);
        }
        if (isClockWise)
        {
            StartSparkleRotation();
        }
        else
        {
            StartSparkleRotationAntiClock();
        }
        
    }

    public void Hide()
    {
        transform.DOKill();
        gameObject.SetActive(false);
    }

    public void SetColou(Color color)
    {
        foreach(Image img in sparkles)
        {
            img.color = color;
        }
    }

    public void StartSparkel()
    {
        gameObject.SetActive(true);
        if (isClockWise)
        {
            StartSparkleRotation();
        }
        else
        {
            StartSparkleRotationAntiClock();
        }
    }
    private void AnimateSparkle(Image img)
    {
        // Setze initial Alpha zufällig
        img.color = new Color(img.color.r, img.color.g, img.color.b, alphaMin);

        Pulse(img);
    }

    private void Pulse(Image img)
    {
        float duration = Random.Range(minDuration, maxDuration);
        float targetAlpha = Random.Range(alphaMin, alphaMax);

        img.DOFade(targetAlpha, duration)
           .SetEase(Ease.InOutSine)
           .OnComplete(() => Pulse(img)); // Endlosschleife
    }

    void StartSparkleRotation()
    {
        // Reset Rotation
        transform.transform.localRotation = Quaternion.identity;

        // Endlos-Rotation
        transform.transform.DOLocalRotate(new Vector3(0, 0, -360f), 50f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); // endlos
    }

    void StartSparkleRotationAntiClock()
    {
        // Reset Rotation
        transform.transform.localRotation = Quaternion.identity;

        // Endlos-Rotation
        transform.DOLocalRotate(new Vector3(0, 0, 360f), 20f, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart);
    }

     


}
