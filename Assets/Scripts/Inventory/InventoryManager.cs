using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool isOpen;
    private Camera mainCamera;

    public GameObject UIPanel;
    public Transform inventoryPanel;
    public float reachDistance = 30f;
    public List<InventorySlot> slots = new List<InventorySlot>();
    
    private void Awake() {
        UIPanel.SetActive(true);
    }

    private void Start() {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++) {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null) {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            } 
        }
        UIPanel.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isOpen = !isOpen;
            if (isOpen) {
                UIPanel.SetActive(true);
            } else {
                UIPanel.SetActive(false);
            }
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(ray, out hit, reachDistance)) {
                if (hit.collider.gameObject.GetComponent<Item>() != null) {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    private void AddItem(ItemScriptableObject _item, int _amount) {
        Debug.Log("_item " + _item);
        Debug.Log("_amount " + _amount);
        Debug.Log("slots " + slots);
        foreach(InventorySlot slot in slots) {
            if (slot.item == _item) {
                if (slot.amount + _amount <= _item.maximumAmount) {
                    slot.amount += _amount;
                    slot.itemAmount.text = slot.amount.ToString();
                    return;
                }
                
                break;
            }
        }

        foreach(InventorySlot slot in slots) {
            if (slot.isEmpty == true) {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmount.text = _amount.ToString();
                break;
            }
        }
    }
}
