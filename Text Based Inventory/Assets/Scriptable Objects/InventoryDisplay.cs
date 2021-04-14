using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryObject inventory;
    public Text inventoryDisplay;

    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    void Start()
    {
        createDisplay(); 
    }

    void Update()
    {
        //updateDisplay();
    }

    //public void updateDisplay()
    //{
    //    for (int i = 0; i < inventory.Container.Count; i++)
    //    {
    //        if (itemsDisplayed.ContainsKey(inventory.Container[i]))
    //            itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
    //        else
    //        {
    //            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
    //            obj.GetComponent<RectTransform>().localPosition = getPosition(i);
    //            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
    //            obj.GetComponentInChildren<TextMeshProUGUI>().alignment = TextAlignmentOptions.BottomRight;
    //            obj.GetComponentInChildren<TextMeshProUGUI>().fontSizeMax = 8;
    //            itemsDisplayed.Add(inventory.Container[i], obj);
    //        }

    //    }
    //}

    public void createDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            inventoryDisplay.text += "\n" + (i+1) + ". " + inventory.Container[i].item.name.ToString() + " - " + inventory.Container[i].amount.ToString("n0");
        }
    }
}
