using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeShopUI : MonoBehaviour
{
    public GameObject recipeShopBG;
    public GameObject recipeShopPanel;
    public InventoryObject playerInventory;
    public InventoryObject shopInventory { get; set; }

    public Transform panelContainerPos;
    public Transform detailsContainerPos;
    [SerializeField] Transform itemBannerContainerPos;
    public TextMeshProUGUI purchasedOrRejectedText;

    List<GameObject> tempItemBanner = new List<GameObject>();

    List<GameObject> tempPanelsGameObjs = new List<GameObject>();
    public GameObject requiredItemBanner { get; set; }

    public bool isOpen { get; set; } = false;

    

    

    void GenerateItemBanner()
    {
        GameObject _itemBanner = Instantiate(requiredItemBanner);
        _itemBanner.transform.SetParent(itemBannerContainerPos.gameObject.transform, false);

        tempItemBanner.Add(_itemBanner);
    }

    void UpdateItemBannerData()
    {
        if (tempItemBanner[0] != null)
        {

            RequiredItemsBanner _itemsBanner = tempItemBanner[0].GetComponent<RequiredItemsBanner>();
            _itemsBanner.TurnOnUpTo(_itemsBanner.RequiredItems.Count);
        }
    }

    public void OpenSellShop()
    {
        isOpen = true;
        LeanTween.scale(recipeShopBG, Vector3.one, .3f);

        GenerateItemBanner();
        UpdateItemBannerData();

        GenerateShopPanels();
    }

    public void CloseShop()
    {
        if (isOpen)
        {
            isOpen = false;

            ResetAllPanelStates();
            LeanTween.scale(recipeShopBG, Vector3.zero, .2f);
            for (int i = 0; i < tempPanelsGameObjs.Count; i++)
            {
                Destroy(tempPanelsGameObjs[i]);
            }
            tempPanelsGameObjs.Clear();

            if (tempItemBanner[0] != null)
            {
                Destroy(tempItemBanner[0]);
                tempItemBanner.Clear();
            }
        }
    }

    void GenerateShopPanels()
    {
        ResetAllPanelStates();
        for (int i = 0; i < shopInventory.Container.Count; i++)
        {
            GameObject panelObj = Instantiate(recipeShopPanel);
            panelObj.transform.SetParent(panelContainerPos.gameObject.transform, false);

            var objInfo = panelObj.GetComponent<ShopPanel>();

            objInfo.PanelPos = i;

            objInfo.itemName.text = shopInventory.Container[i].item.ItemName;
            objInfo.recipeShoppingPanel.recipeSheetImage.sprite = shopInventory.Container[i].item.itemSprite;
            objInfo.recipeShoppingPanel.detailsRecipeSheetImage.sprite = shopInventory.Container[i].item.itemSprite;
            objInfo.recipeShoppingPanel.description.text = shopInventory.Container[i].item.description;
            //objInfo.recipeShoppingPanel.recipePrice.text = shopInventory.Container[i].item.BuyPrice.ToString();

            switch (shopInventory.Container[i].item.ItemRecipeGrade) 
            {
                case RecipeGrade.U:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[0];
                    break;
                case RecipeGrade.D:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[1];
                    break;
                case RecipeGrade.C:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[2];
                    break;
                case RecipeGrade.B:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[3];
                    break;
                case RecipeGrade.A:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[4];
                    break;
                case RecipeGrade.S:
                    objInfo.recipeShoppingPanel.recipeGradeImage.sprite = objInfo.recipeShoppingPanel.recipeGradeSprites[5];
                    break;
            }

            for (int j = 0; j < shopInventory.Container[i].item.RecipeItems.Count; j++) 
            {
                LeanTween.scale(objInfo.recipeShoppingPanel.ingredientsIMGs[j].gameObject, Vector3.one, .1f);
                LeanTween.scale(objInfo.recipeShoppingPanel.ingredientAmts[j].gameObject, Vector3.one, .1f);

                objInfo.recipeShoppingPanel.ingredientsIMGs[j].sprite = shopInventory.Container[i].item.RecipeItems[j].ingredientItem.itemSprite;
                objInfo.recipeShoppingPanel.ingredientAmts[j].text = shopInventory.Container[i].item.RecipeItems[j].numberOfItem.ToString();
            }

            tempPanelsGameObjs.Add(panelObj);
        }

    }
    public void BuyRecipe(int panelPos)
    {
        bool canBuy = false;

        if (playerInventory.playerTotalSolcs >= shopInventory.Container[panelPos].item.BuyPrice)
        {
            canBuy = true;
        }

        if (canBuy)
        {
            if (playerInventory.Container.Count < 5)
            {
                playerInventory.AddItem(shopInventory.Container[panelPos].item, 1);

                playerInventory.playerTotalSolcs -= shopInventory.Container[panelPos].item.BuyPrice;
                playerInventory.Save();

                StartCoroutine(ShowFundsText("Purchased"));
            }
            else
            {
                Debug.Log("Insufficient Space");
                StartCoroutine(ShowFundsText("Insufficient Space"));
            }
        }
        else
        {
            Debug.Log("Insufficient Funds");
            StartCoroutine(ShowFundsText("Insufficient Funds"));
        }
    }

    public void ResetAllPanelStates()
    {
        for (int i = 0; i < tempPanelsGameObjs.Count; i++)
        {
            var sPanel = tempPanelsGameObjs[i].GetComponent<ShopPanel>();
            sPanel.recipeShoppingPanel.buyButton.SetActive(true);
            sPanel.recipeShoppingPanel.confirmButton.SetActive(false);


            sPanel.panelState = ShopPanelState.Base;

            sPanel.CloseDetailsPanel();
        }
    }

    #region Funds and Fading Functions
    IEnumerator ShowFundsText(string textToShow)
    {
        purchasedOrRejectedText.text = textToShow;
        yield return FadeIn(purchasedOrRejectedText, .2f);
        yield return new WaitForSeconds(1.5f);
        yield return FadeOut(purchasedOrRejectedText, 2f);
    }

    public IEnumerator FadeIn(TextMeshProUGUI textToFade, float time)
    {
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 0);
        while (textToFade.color.a < 1.0f)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a + (Time.deltaTime / time));
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
    }
    public IEnumerator FadeOut(TextMeshProUGUI textToFade, float time)
    {
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 1);
        while (textToFade.color.a > 0.0f)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a - Time.deltaTime / time);
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
    }
    #endregion

    public void UpdateCurrentRecipeSolcCost(int listPos) 
    {
        if (tempItemBanner[0] != null)
        {

            RequiredItemsBanner _itemsBanner = tempItemBanner[0].GetComponent<RequiredItemsBanner>();
            _itemsBanner.SetNewSolcCostText(shopInventory.Container[listPos].item.BuyPrice , 0);
        }
    }
}
