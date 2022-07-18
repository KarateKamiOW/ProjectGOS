using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum InvItemDescPanelState {Base, ConsumeDiscard, Discard, Toss  }
public class InventoryItemDescrPanelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerUpHandler
{
    public GameObject panelGameObj;
    public int invPos;

    [Header("Storage Panel")]
    public bool isStoragePanel;
    public Image storageBG;
    [SerializeField] List<Sprite> storageSpritesList;

    [Header("Data/Info")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescription;
    public TextMeshProUGUI carryCapacityText;
    public TextMeshProUGUI sellPriceText;
    public Image itemSprite;
    public Image pointerSprite;
    public ConsumeDiscardWindow consumeDiscardWindow;
    //bool infoUpdated = false;

    [Header("Temp /copy Fields")]
    [SerializeField] GameObject draggablePanelGameObject;
    public Image tempInventoryPanelIMG;

    public ItemDataObject ItemData { get; set; }
    public DisplayInventory DisplayInv { get; set; }

    public bool PanelIsPopulatedAndReadyForUse { get; set; }
    InvItemDescPanelState descPanelState;

    RectTransform rectTransform;
    CanvasGroup tempCanvasGroup;
    RectTransform tempRectTransform;
    GameObject tempInventoryPanelGO;
    Canvas canvas;
    Vector3 offset;
    ItemType TypeOfItem;

    private void Start()
    {
        LeanTween.scale(panelGameObj, Vector3.zero, .15f);
        //infoUpdated = false;
        pointerSprite.gameObject.SetActive(false);
        //DisplayInv = GetComponentInParent<DisplayInventory>();
        DisplayInv = GetComponentInParent(typeof (DisplayInventory)) as DisplayInventory;
        //StorageLogUI = GetComponentInParent(typeof (StorageLogUI)) as StorageLogUI;

        LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.zero, .1f);

        if(consumeDiscardWindow.consumeDiscardWindowGameObj != null)
            consumeDiscardWindow.TurnOffBars();

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            Debug.Log("canvas still null");
        //consumeDiscardWindow.consumeBar.gameObject.SetActive(false);
        //consumeDiscardWindow.discardBar.gameObject.SetActive(false);
    }


    void Update() 
    {
        if (descPanelState == InvItemDescPanelState.Discard)
        {
            if(!DisplayInv.currentInventoryObjectDragIsActivated)
                ConsumeDiscard(false);   
        }
        else if (descPanelState == InvItemDescPanelState.ConsumeDiscard) 
        {
            if(!DisplayInv.currentInventoryObjectDragIsActivated)
                ConsumeDiscard(true);
        }

    }
    

    

    void CheckForConsumationAbility(ItemType typeOfItem) 
    {
        //All items can be discarded, but only certain items can be consumed/used
        //Here is where we check
        if (consumeDiscardWindow.consumeDiscardWindowGameObj != null)
        {

            switch (typeOfItem)
            {
                case ItemType.Food:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[1];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.ConsumeDiscard;
                    TypeOfItem = ItemType.Food;

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(true);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeBar.gameObject.SetActive(true);
                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);
                    break;

                case ItemType.Default:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[0];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.Discard;
                    TypeOfItem = ItemType.Default;

                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(false);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);
                    break;

                case ItemType.Recipe:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[1];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.ConsumeDiscard;
                    TypeOfItem = ItemType.Recipe;

                    consumeDiscardWindow.consumeBar.gameObject.SetActive(true);
                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(true);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);
                    break;
                case ItemType.Spell:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[1];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.ConsumeDiscard;
                    TypeOfItem = ItemType.Spell;

                    consumeDiscardWindow.consumeBar.gameObject.SetActive(true);
                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(true);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);
                    break;
                case ItemType.Potion:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[1];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.ConsumeDiscard;
                    TypeOfItem = ItemType.Potion;

                    consumeDiscardWindow.consumeBar.gameObject.SetActive(true);
                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(true);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);
                    break;

                default:
                    consumeDiscardWindow.consumeDiscardBgImage.sprite = consumeDiscardWindow.consumeBGImageList[0];
                    LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.one, .1f).setEase(LeanTweenType.easeInQuad);
                    descPanelState = InvItemDescPanelState.Discard;
                    TypeOfItem = DisplayInv.inventory.Container[invPos].item.itemType;

                    consumeDiscardWindow.discardBar.gameObject.SetActive(true);

                    consumeDiscardWindow.consumeHint.gameObject.SetActive(true);
                    consumeDiscardWindow.discardHint.gameObject.SetActive(true);
                    break;



            }
        }

    }

    void ConsumeDiscard(bool consumes) 
    {
        if (DisplayInv.inventory.Container.Count > 0)
        {



            //Consume Outside of town
            if (consumes && !PlayerController.instance.InATownMap)
            {
                if (Input.GetMouseButton(0) && !Input.GetButtonDown("Fire2"))
                {

                    consumeDiscardWindow.discardPercentage = 0;
                    consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);

                    if (consumeDiscardWindow.consumePercentage < 100)
                    {
                        consumeDiscardWindow.consumePercentage += 8f * (Time.deltaTime * 10f);
                        consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                        if (consumeDiscardWindow.consumePercentage >= 100)
                        {
                            consumeDiscardWindow.consumePercentage = 0;
                            consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                            Consume(DisplayInv.inventory.Container[invPos].item);

                        }
                    }
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    if (consumeDiscardWindow.consumePercentage < 100)
                    {
                        consumeDiscardWindow.consumePercentage = 0;
                        consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);
                    }
                }
            }

            //Consume inside of town, but only for Recipes, Relics and Spellbooks
            if (consumes && PlayerController.instance.InATownMap)
            {
                if (DisplayInv.inventory.Container[invPos].item.itemType == ItemType.Recipe || DisplayInv.inventory.Container[invPos].item.itemType == ItemType.Spell ||
                    DisplayInv.inventory.Container[invPos].item.itemType == ItemType.Relic)
                {

                    if (Input.GetMouseButton(0) && !Input.GetButtonDown("Fire2"))
                    {

                        consumeDiscardWindow.discardPercentage = 0;
                        consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);

                        if (consumeDiscardWindow.consumePercentage < 100 && DisplayInv.inventory.Container[invPos] != null)
                        {
                            consumeDiscardWindow.consumePercentage += 8f * (Time.deltaTime * 10f);
                            consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                            if (consumeDiscardWindow.consumePercentage >= 100)
                            {
                                consumeDiscardWindow.consumePercentage = 0;
                                consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                                Consume(DisplayInv.inventory.Container[invPos].item);
                            }
                        }
                    }

                    if (Input.GetButtonUp("Fire1"))
                    {
                        if (consumeDiscardWindow.consumePercentage < 100)
                        {
                            consumeDiscardWindow.consumePercentage = 0;
                            consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);
                        }
                    }
                }
            }

            //Discard

            if (Input.GetMouseButton(1) && !Input.GetButtonDown("Fire1"))
            {

                consumeDiscardWindow.consumePercentage = 0;
                consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                if (consumeDiscardWindow.discardPercentage < 100)
                {
                    consumeDiscardWindow.discardPercentage += 8f * (Time.deltaTime * 10f);
                    consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);

                    if (consumeDiscardWindow.discardPercentage >= 100)
                    {
                        consumeDiscardWindow.discardPercentage = 0;
                        consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);
                        DisplayInv.inventory.DeleteItemAtPosition(invPos, 1);
                    }
                }
            }

            if (Input.GetButtonUp("Fire2"))
            {
                if (consumeDiscardWindow.discardPercentage < 100)
                {
                    consumeDiscardWindow.discardPercentage = 0;
                    consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);
                }
            }
        }
    }

    public void SetInvPosNum(int num) 
    {
       
        invPos = num;
    }

    public void Consume(ItemDataObject item) 
    {
        switch (item.itemType) 
        {
            case ItemType.Food:
                StartCoroutine(PlayerController.instance.overWorldData.UpdateHealth(-DisplayInv.inventory.Container[invPos].item.RestoreHealthValue));
                DisplayInv.inventory.DeleteItemAtPosition(invPos, 1);
                ExitedPanelBehavior();
                break;
            case ItemType.Potion:
                PlayerController.instance.playerPotionBuffs.AddBuff(DisplayInv.inventory.Container[invPos].item.PotionBuffID, DisplayInv.inventory.Container[invPos].item, invPos);
                DisplayInv.inventory.DeleteItemAtPosition(invPos, 1);
                ExitedPanelBehavior();
                break;
            case ItemType.Recipe:
                DisplayInv.inventory.AddRecipe(DisplayInv.inventory.Container[invPos].item);
                DisplayInv.inventory.DeleteItemAtPosition(invPos, 1);
                ExitedPanelBehavior();
                break;
            case ItemType.Spell:
                DisplayInv.inventory.UnlockSpell(DisplayInv.inventory.Container[invPos].item.AssociatedSpell);
                DisplayInv.inventory.DeleteItemAtPosition(invPos, 1);
                ExitedPanelBehavior();
                break;
            default:
                Debug.Log("Unaccounted For Consumable");
                break;

        }
    }

    #region Mouse Logic

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Over Registered");
        if (PanelIsPopulatedAndReadyForUse) // was && !infoUpdated
        {
            LeanTween.scale(panelGameObj, new Vector3(3, 3, 3), .15f);
            pointerSprite.gameObject.SetActive(true);
            //infoUpdated = true;
            if (consumeDiscardWindow.consumeDiscardWindowGameObj != null)
                CheckForConsumationAbility(ItemData.itemType);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExitedPanelBehavior();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(tempCanvasGroup != null)
            tempCanvasGroup.blocksRaycasts = true;

        if(DisplayInv != null)
            DisplayInv.currentInventoryObjectDragIsActivated = false;
        if (tempInventoryPanelGO != null)
            Destroy(tempInventoryPanelGO);

        if (isStoragePanel)
        {
            storageBG.sprite = storageSpritesList[0];
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var otherInvDetails = eventData.pointerDrag.GetComponent<InventoryItemDescrPanelUI>();
        int swapInvPosNum = otherInvDetails.invPos;
        
        if (!isStoragePanel)
        {
            if (!otherInvDetails.isStoragePanel) 
            {
                //Dragged a NON-Storage panel onto another
                if(DisplayInv != null)  //Means execution was done in inventory screen
                    DisplayInv.SwapInventoryPositions(swapInvPosNum, invPos);
                else    //Means It was executed in Storage Panel
                    GameManager.instance.storageLog.SwapInventoryWithInventory(swapInvPosNum, invPos);
            }
            else
            {
                //Withdrawing
                GameManager.instance.storageLog.WithdrawFromStorage(swapInvPosNum, invPos);
            }
        }
        else
        {
            if (!otherInvDetails.isStoragePanel) 
            {
                GameManager.instance.storageLog.SwapStorageAndInventory(invPos, swapInvPosNum);
            }
            else
            {
                GameManager.instance.storageLog.SwapStorageWithAnotherStorageSpot(invPos, swapInvPosNum);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*tempInventoryPanelGO = Instantiate(draggablePanelGameObject);
        tempInventoryPanelGO.transform.SetParent(rectTransform.gameObject.transform, false);
        tempInventoryPanelGO.SetActive(true);
        tempRectTransform = tempInventoryPanelGO.GetComponent<RectTransform>();
        tempCanvasGroup = tempInventoryPanelGO.GetComponent<CanvasGroup>();*/

        if (isStoragePanel)
        {
            storageBG.sprite = storageSpritesList[1];
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggablePanelGameObject != null && itemSprite.sprite != null)// && DisplayInv.inventory.Container.Count > invPos
        {
            tempInventoryPanelGO = Instantiate(draggablePanelGameObject);
            tempInventoryPanelGO.transform.SetParent(rectTransform.gameObject.transform, false);
            tempInventoryPanelGO.SetActive(true); 
            tempRectTransform = tempInventoryPanelGO.GetComponent<RectTransform>();
            tempCanvasGroup = tempInventoryPanelGO.GetComponent<CanvasGroup>();

            if(canvas != null)
                tempInventoryPanelGO.transform.SetParent(canvas.gameObject.transform, false);


            tempCanvasGroup.blocksRaycasts = false;

            if(DisplayInv != null)
                DisplayInv.currentInventoryObjectDragIsActivated = true;

            LeanTween.scale(panelGameObj, Vector3.zero, .15f);
            //infoUpdated = false;
            pointerSprite.gameObject.SetActive(false);
            LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.zero, .1f);

            if (consumeDiscardWindow.consumeDiscardWindowGameObj != null)
            {
                consumeDiscardWindow.consumePercentage = 0;
                consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

                consumeDiscardWindow.discardPercentage = 0;
                consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);

                consumeDiscardWindow.consumeBar.gameObject.SetActive(false);
                consumeDiscardWindow.discardBar.gameObject.SetActive(false);

                consumeDiscardWindow.consumeHint.gameObject.SetActive(false);
                consumeDiscardWindow.discardHint.gameObject.SetActive(false);
            }


        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggablePanelGameObject != null && itemSprite.sprite != null)
            tempInventoryPanelGO.transform.position = Input.mousePosition;
        
        //draggablePanelGameObject.transform.position = Input.mousePosition;
        //tempRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        //draggablePanelGameObject.transform.position = Input.mousePosition;


    }

    #endregion

    void ExitedPanelBehavior() 
    {
        LeanTween.scale(panelGameObj, Vector3.zero, .15f);
        //infoUpdated = false;
        pointerSprite.gameObject.SetActive(false);
        LeanTween.scale(consumeDiscardWindow.consumeDiscardWindowGameObj, Vector3.zero, .1f);

        if (consumeDiscardWindow.consumeDiscardWindowGameObj != null)
        {
            consumeDiscardWindow.consumePercentage = 0;
            consumeDiscardWindow.consumeBar.transform.localScale = new Vector3((consumeDiscardWindow.consumePercentage / 100), 1f);

            consumeDiscardWindow.discardPercentage = 0;
            consumeDiscardWindow.discardBar.transform.localScale = new Vector3((consumeDiscardWindow.discardPercentage / 100), 1f);

            consumeDiscardWindow.consumeBar.gameObject.SetActive(false);
            consumeDiscardWindow.discardBar.gameObject.SetActive(false);

            consumeDiscardWindow.consumeHint.gameObject.SetActive(false);
            consumeDiscardWindow.discardHint.gameObject.SetActive(false);
        }
        descPanelState = InvItemDescPanelState.Base;
    }
}
[System.Serializable]
public class ConsumeDiscardWindow 
{
    public GameObject consumeDiscardWindowGameObj;
    public Image consumeDiscardBgImage;
    public List<Sprite> consumeBGImageList;

    public GameObject consumeBar;
    public GameObject discardBar;

    public TextMeshProUGUI consumeHint;
    public TextMeshProUGUI discardHint;
    public float consumePercentage { get; set; }
    public float discardPercentage { get; set; }

    public void TurnOffBars() 
    { 
        consumeBar.SetActive(false);
        discardBar.SetActive(false);
    }
}
