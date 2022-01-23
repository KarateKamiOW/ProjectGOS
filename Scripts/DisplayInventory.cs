using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    //Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    public List<Image> itemSprites = new List<Image>();
    public List<TextMeshProUGUI> itemAmounts = new List<TextMeshProUGUI>();
    public List<Image> itemSelectors = new List<Image>();
    public TextMeshProUGUI solcsAmount;
    public TextMeshProUGUI magiGemsAmount;
    // Start is called before the first frame update
    void Start()
    {
        ResetDisplay();
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    void ResetDisplay() 
    {
        for (int i = 0; i < itemSelectors.Count; i++) 
        {
            LeanTween.scale(itemSprites[i].gameObject, Vector3.zero, .2f);
            itemSprites[i].sprite = null;
            itemAmounts[i].text = "-";
        }

        solcsAmount.text = "0";
        magiGemsAmount.text = "0";
    }

    void CreateDisplay() 
    {
        for (int i = 0; i < inventory.Container.Count; i++) 
        {
            var obj = inventory.Container[i].item;
            itemSprites[i].sprite = obj.itemSprite;
            itemAmounts[i].text = inventory.Container[i].amount.ToString();

        }
    }

    void UpdateDisplay() 
    {
       /* for (int i = 0; i < inventory.Container.Count; i++) 
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i])) 
            { 
                itemsDisplayed[inventory.Container[i]].GetComponent<>
            }
        } */

        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = inventory.Container[i].item;
            itemSprites[i].sprite = obj.itemSprite;
            itemAmounts[i].text = inventory.Container[i].amount.ToString();

            if (inventory.Container[i] != null)
                LeanTween.scale(itemSprites[i].gameObject, Vector3.one, .2f);

        }
    }
}
