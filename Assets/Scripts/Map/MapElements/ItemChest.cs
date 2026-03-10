using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemChest : MonoBehaviour
{
    [SerializeField] private GameObject chest;
    [SerializeField] private GameObject item;
    [SerializeField] private Animator animatorChest;
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private SpriteRenderer itemRenderer;

    [SerializeField] private GameObject kanonItem;

    [SerializeField] private BoxCollider2D collider2D;

    [SerializeField] private Light2D light2D;

    [SerializeField] private List<ScriptableItem> scriptableItems;

    private bool isClosed = true;
    private Vector3 startScale;
    
   private ItemName[] allItems = 
    {
    ItemName.Kanon,
    ItemName.Heiligenschein,
    ItemName.DoubleMoney,
    ItemName.Magnet
    };
    private ItemName currentItem;
    private ScriptableItem scriptableItem;
    private void OnEnable()
    {
        int spawnRandom = Random.Range(1,5);//wahrscheinlichkeit das eine spawned
        if(spawnRandom > 1)
        {
            Destroy(gameObject);
        }
        chest.SetActive(true);
        item.SetActive(false);
        kanonItem.SetActive(false);
        int random = Random.Range(0, allItems.Length);
        currentItem = allItems[random];
        ChangeItemVisual(currentItem);

        startScale = transform.localScale;
        StartCoroutine(PulseRoutine());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Open Chest");
        collider2D.enabled = false;
        GameManager.Instance.UseItem();
        VisualyOpenChest();
    }

    private IEnumerator PulseRoutine()
{
    while (isClosed)
    {
        //Sprite pulse
        float t = Mathf.PingPong(Time.time * 2f, 1f);
        float scaleMultiplier = Mathf.Lerp(1f, 1.1f, t);
        transform.localScale = startScale * scaleMultiplier;
        //light
        float t2 = Mathf.PingPong(Time.time * 2f, 1f);
        light2D.intensity = Mathf.Lerp(0.5f, 0.3f, t2);
        yield return null;
    }
        light2D.intensity = Mathf.Lerp(0f, 0f, 1);

}

    private void VisualyOpenChest()
    {
        isClosed = false;
        animatorChest.SetTrigger("OpenChest");
    }

    public void OnChestOpenFinished()
    {
        ShowItem();
        
    }

    public void ShowItem()
    {
        item.SetActive(true);
        itemAnimator.SetTrigger("UseItem");
    }


    public void ActivateItem()
    {
        chest.SetActive(false);
        switch (currentItem)
        {
            case(ItemName.DoubleMoney):
                UIManager.Instance.SetUpItem(scriptableItem.itemSprite, scriptableItem.itemDuration);
                GameManager.Instance.GetPlayerScript().UseDoubleMoney(scriptableItem.itemDuration);
                itemRenderer.enabled = false;
                GameManager.Instance.GameRunning();
            break;
            case(ItemName.Kanon):
                kanonItem.SetActive(true);
                item.SetActive(false);
                GameManager.Instance.GetPlayerScript().UseKanon(item.transform.position);
            //GameManager.Instance.GameRunning();
            break;
            case(ItemName.Magnet):
                GameManager.Instance.GetPlayerScript().UseMagnet(scriptableItem.itemDuration);
                UIManager.Instance.SetUpItem(scriptableItem.itemSprite, scriptableItem.itemDuration);
                itemRenderer.enabled = false;
                GameManager.Instance.GameRunning();
            break;
            case(ItemName.Heiligenschein):
                GameManager.Instance.GetPlayerScript().UseHeiligenschein(scriptableItem.itemSprite, scriptableItem.itemDuration);
                UIManager.Instance.SetUpItem(scriptableItem.itemSprite, scriptableItem.itemDuration);
                itemRenderer.enabled = false;
                GameManager.Instance.GameRunning();
            break;
            case(ItemName.DoubleJump):
            break;
        }
        
    }


    private void ChangeItemVisual(ItemName itemName)
    {
        foreach(ScriptableItem item in scriptableItems)
        {
            if(itemName == item.itemName)
            {
                scriptableItem = item;
                this.itemAnimator.runtimeAnimatorController = item.itemAnimator;
                itemRenderer.sprite = item.itemSprite;
                break;
            }
        }
    }
}

public enum ItemName
{
    Kanon,
    Magnet,
    DoubleMoney,
    Heiligenschein,
    DoubleJump
}
