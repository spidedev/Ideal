using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManagement_GUI : MonoBehaviour
{
    [SerializeField] private GameObject prefab, optionsBox;
    [SerializeField] private Transform parentCanvas;
    public static bool DisplayingGUI = false;
    [SerializeField] private Transform select, target, op_select, op_target, op_use, op_toss, op_bio;
    [SerializeField] private TextMeshProUGUI nameHolder, op_use_text, op_toss_text;
    [SerializeField] private Image dsply;
    [SerializeField] private EventSystem es;
    private bool selectedItem = false;
    private bool bio;
    private int op_index;
    private GameObject currentlySelGame;
    private void OnEnable()
    {
        InventoryObject.addedItem += DisplayObjects;
        PlayerMovement._inventoryOpen += DisplayGUI;
        PlayerMovement._confirmed += OptionShow;
        PlayerMovement._upInv += UpInv;
        PlayerMovement._downInv += DownInv;
    }

    private void OnDisable()
    {
        InventoryObject.addedItem -= DisplayObjects;
        PlayerMovement._inventoryOpen -= DisplayGUI;
        PlayerMovement._confirmed -= OptionShow;
        PlayerMovement._upInv -= UpInv;
        PlayerMovement._downInv -= DownInv;
    }

    private void Update()
    {
        op_index = Mathf.Clamp(op_index, 0, 2);

        switch (op_index)
        {
            case 0:
                op_target = op_use;
                break;
            
            case 1:
                op_target = op_toss;
                break;
            
            case 2:
                op_target = op_bio;
                break;
        }
        
        if (!DisplayingGUI)
        {
            return;
        }

        if (selectedItem)
        {
            currentlySelGame = es.currentSelectedGameObject;
            es.enabled = false;
        }
        else
        {
            currentlySelGame = null;
            es.enabled = true;
        }

        if(selectedItem)
        {
            if (!currentlySelGame.GetComponent<OWN_GUI>().item.canUse)
            {
                op_use_text.color = Color.gray;
            }
            else
            {
                op_use_text.color = Color.white;
            }
            
            if (!currentlySelGame.GetComponent<OWN_GUI>().item.canToss)
            {
                op_toss_text.color = Color.gray;
            }
            else
            {
                op_toss_text.color = Color.white;
            }

            op_select.position = new Vector3(185, op_target.position.y);
        }

        if (selectedItem)
        {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            nameHolder.transform.parent.gameObject.SetActive(true);
            nameHolder.text = EventSystem.current.currentSelectedGameObject.GetComponent<OWN_GUI>().item.Name;
            dsply.sprite = EventSystem.current.currentSelectedGameObject.GetComponent<OWN_GUI>().item.sprite;
            
        }
        else
        {
            nameHolder.transform.parent.gameObject.SetActive(false);
        }

        if (parentCanvas.childCount != 0)
        {
            select.gameObject.SetActive(true);

            if (target != null)
            {
                select.position = target.position;
            }

            if (EventSystem.current.currentSelectedGameObject != null)
            {
                target = EventSystem.current.currentSelectedGameObject.transform;
            }
        }
        else
        {
            select.gameObject.SetActive(false);
        }
        
        
    }

    private void DisplayObjects(ItemObject obj, int amount)
    {
        foreach (Transform transform in parentCanvas)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Destroy(transform.gameObject);
        }
        
        foreach (InventorySlot slot in InventoryManagement_PLR.GetInstance().inventory.Container)
        {
            GameObject newSlot = Instantiate(prefab, parentCanvas);
            TextMeshProUGUI amountText = newSlot.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            ItemObject o_item = newSlot.GetComponent<OWN_GUI>().item = slot.item;
            
            amountText.text = slot.amount.ToString();
        }
    }

    public void DisplayGUI()
    {
        if (parentCanvas.childCount != 0)
        {
            EventSystem.current.SetSelectedGameObject(null);
            StartCoroutine(SelectFirstChoice());
        }

        if (!DisplayingGUI)
        {
            DisplayingGUI = true;
            parentCanvas.parent.gameObject.SetActive(true);
        }
        else
        {
            DisplayingGUI = false;
            parentCanvas.parent.gameObject.SetActive(false);
        }
    }

    public void OptionShow()
    {
        if (DisplayingGUI && EventSystem.current.currentSelectedGameObject != null)
        {
            selectedItem = true;
            optionsBox.SetActive(true);
        }
    }

    private void UpInv()
    {
        if (selectedItem)
        {
            op_index -= 1;
        }
    }

    private void DownInv()
    {
        if (selectedItem)
        {
            op_index += 1;
        }
    }
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(parentCanvas.GetChild(0).gameObject);
    }


}
