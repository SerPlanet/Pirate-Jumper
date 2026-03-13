using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChestOpen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Chest Parts")]
    [SerializeField] private RectTransform chestRoot;
    [SerializeField] private RectTransform chestBody;
    [SerializeField] private RectTransform chestLid;

    [Header("Item")]
    [SerializeField] private RectTransform item;

    [Header("Effects")]
    [SerializeField] private Image flashImage;
    [SerializeField] private Image glow;
    [SerializeField] private Image chestLidImage;
    [SerializeField] private Image itemImage;

    [SerializeField] private SparkelEffekt sparkelEffekt;
    [SerializeField] private SparkelEffekt sparkelEffekt2;

    [SerializeField] private AudioClip openRevealSound;

    [Header("Settings")]
    [SerializeField] private float dropHeight = 800f;
    [SerializeField] private float pressScale = 0.9f;

    [Header ("Sprites")]

    [SerializeField] private Sprite openChestSprite;

    [SerializeField] private Sprite closedChestSprite;

    [SerializeField] private Image goBackRaycast;

    bool chestOpened = false;
    bool canPress = false;
    Rarity rarity;

    Vector2 startPosItem;

    private void Start()
    {
        startPosItem = item.anchoredPosition;
        sparkelEffekt.Hide();
        sparkelEffekt2.Hide();
    }

    public void SetUpChest(Rarity rarity, Sprite charachterIcon)
    {
        itemImage.sprite = charachterIcon;
        this.rarity = rarity;
       
    }
    public void SetupInitialState()
    {
        chestOpened = false;
        canPress = false;
        item.anchoredPosition = startPosItem;
        chestLidImage.sprite = closedChestSprite;
        item.localScale = Vector3.zero;
        Color currentGlowCollor = glow.color;
        goBackRaycast.raycastTarget = false;
        sparkelEffekt.Hide();
        sparkelEffekt2.Hide();

        //Tween Reset
        glow.transform.DOKill();
        item.DOKill();
        chestRoot.DOKill();

        glow.color = new Color(currentGlowCollor.r, currentGlowCollor.g, currentGlowCollor.b,0);

        flashImage.color = new Color(1,1,1,0);
        Vector2 startPos = chestRoot.anchoredPosition + new Vector2(0, dropHeight);
    }

    public void DropChest()
    {
        Vector2 startPos = chestRoot.anchoredPosition + new Vector2(0, dropHeight);
        chestRoot.anchoredPosition = startPos;

        chestRoot.DOAnchorPosY(0, 0.6f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                canPress = true;
                ImpactShake();
            });
    }

    void ImpactShake()
    {
        chestRoot.DOShakePosition(
            duration:0.2f,
            strength:10f,
            vibrato:20,
            randomness:90
        );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(chestOpened &&  goBackRaycast.raycastTarget) GameManager.Instance.GameLobby();
        if (!canPress || chestOpened) return;

        chestRoot.DOScale(pressScale, 0.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!canPress || chestOpened) return;

        chestRoot.DOScale(1f, 0.1f);

        OpenChest();
    }

    void OpenChest()
    {
        chestOpened = true;

        Sequence seq = DOTween.Sequence();

        seq.Append(OpenLid())
        .AppendCallback(() =>
        {
            PlayFlash();
            PlayGlow();
            sparkelEffekt.StartSparkel();
            sparkelEffekt2.StartSparkel();
            RevealItem();
        });
        AudioManager.Instance.PlayUIAudi(openRevealSound, 1f,1f);
    }

    Tween OpenLid()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(chestLid.transform.DOScale(0.9f, 0.1f).SetEase(Ease.InQuad));

        seq.Join(chestBody.transform.DOScale(0.9f, 0.1f).SetEase(Ease.InQuad));

        seq.AppendCallback(() =>
        {
            chestLidImage.sprite = openChestSprite;
        });

        seq.Append(chestLid.transform.DOScale(1.1f, 0.15f).SetEase(Ease.OutBack));
        seq.Join(chestBody.transform.DOScale(1.1f, 0.15f).SetEase(Ease.OutBack));

        seq.Append(chestLid.transform.DOScale(1f, 0.1f));
        seq.Join(chestBody.transform.DOScale(1f, 0.1f));

        

        

        return seq;
    }

    void RevealItem()
    {
        
        // Startposition
        Vector3 startPos = item.anchoredPosition;
        item.localScale = Vector3.zero;

        // Animation Sequence
        Sequence seq = DOTween.Sequence();

        // 1️⃣ Scale von 0 → 1.3 + Move nach oben
        seq.Append(
            item.DOScale(1.3f, 0.5f).SetEase(Ease.OutBack)
        );
        seq.Join(
            item.DOAnchorPosY(startPos.y + 320, 0.25f).SetEase(Ease.OutQuad)
        );

        // 2️⃣ Scale zurück auf 1 + Position leicht zurück (bouncend)
        seq.Append(
            item.DOScale(1f, 0.15f)
        );
        seq.Join(
            item.DOAnchorPosY(startPos.y + 270, 0.15f).SetEase(Ease.OutQuad)
        );

        // Optional: kleine Endposition Korrektur
        seq.Append(
            item.DOAnchorPosY(startPos.y+270, 0.1f).SetEase(Ease.OutQuad)
        );
        goBackRaycast.raycastTarget = true;
        
    }

    void PlayFlash()
    {
        flashImage
            .DOFade(0.8f,0.10f)
            .OnComplete(() =>
            {
                flashImage.DOFade(0f,0.25f);
            });
    }

    void PlayGlow()
    {
         // Pop Animation
        switch (rarity)
        {
            case(Rarity.common):
            glow.color = Color.grey;
            sparkelEffekt.SetColou(Color.grey);
            sparkelEffekt2.SetColou(Color.grey);
            break;
            case(Rarity.rare):
            glow.color = Color.blue;
            sparkelEffekt.SetColou(Color.blue);
            sparkelEffekt2.SetColou(Color.blue);
            break;
            case(Rarity.epic):
            glow.color = Color.magenta;
            sparkelEffekt.SetColou(Color.magenta);
            sparkelEffekt2.SetColou(Color.magenta);
            break;
            case(Rarity.legendary):
            glow.color = new Color (1,0.7529413f,0.3019608f);
            sparkelEffekt.SetColou(new Color (1,0.7529413f,0.3019608f));
            sparkelEffekt2.SetColou(new Color (1,0.7529413f,0.3019608f));
            break;
        }
        glow.transform.localScale = Vector3.one;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(glow.DOFade(1f, 0.2f));
        seq.Join(glow.transform.DOScale(1.5f, 0.4f).SetEase(Ease.OutQuad));
        seq.Append(glow.DOFade(0.6f, 0.2f));
        seq.Join(glow.transform.DOScale(1f, 0.2f));

        // Dauerrotation starten
        StartGlowRotation();
    }

    void StartGlowRotation()
    {
        // Reset Rotation
        glow.transform.localRotation = Quaternion.identity;

        // Endlos-Rotation
        glow.transform.DOLocalRotate(new Vector3(0, 0, 360f), 5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); // endlos
    }

    

}