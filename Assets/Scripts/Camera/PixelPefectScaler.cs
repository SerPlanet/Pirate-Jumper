using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // Pixel Perfect Camera Namespace

public class PixelPerfectScaler : MonoBehaviour
{
    [SerializeField]private PixelPerfectCamera pixelPerfectCamera; // Referenz
    private int referenceWidth = 160;
    private int referenceHeight = 90;

    private int ppu = 16;

    void Start()
    {
        pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
        if(pixelPerfectCamera == null)
        {
            Debug.Log("missing");
        }

            //PixelPerfectCamera ppc = Camera.main.GetComponent<PixelPerfectCamera>();

            pixelPerfectCamera.assetsPPU = 16;
            pixelPerfectCamera.refResolutionX = 320;
            pixelPerfectCamera.refResolutionY = 180;

        //ScaleCamera();
    }

    void ScaleCamera()
    {
        int oldPPU = 96;
        int newPPU = 64;

        float ppuScale = (float)newPPU / oldPPU;

        float scaleX = (float)Screen.width / referenceWidth;
        float scaleY = (float)Screen.height / referenceHeight;
        int finalScale = Mathf.FloorToInt(Mathf.Min(scaleX, scaleY));

        PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();
        ppc.assetsPPU = newPPU;
        ppc.refResolutionX = Mathf.RoundToInt(referenceWidth * finalScale * ppuScale);
        ppc.refResolutionY = Mathf.RoundToInt(referenceHeight * finalScale * ppuScale);
    }
}