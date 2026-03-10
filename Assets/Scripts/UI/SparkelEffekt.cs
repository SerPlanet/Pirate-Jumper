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

    private void Start()
    {
        foreach (Image img in sparkles)
        {
            AnimateSparkle(img);
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


}
