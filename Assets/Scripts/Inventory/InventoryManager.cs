using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool isOpen;
    public Transform character;

    public GameObject UIBG;
    public Transform inventoryPanel;
    public float reachDistance = 2f;
    public List<InventorySlot> slots = new List<InventorySlot>();
    
    private void Awake() {
        UIBG.SetActive(true);
    }

    private void Start() {
        for (int i = 0; i < inventoryPanel.childCount; i++) {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null) {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            } 
        }
        UIBG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isOpen = !isOpen;
            if (isOpen) {
                UIBG.SetActive(true);
                inventoryPanel.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                UIBG.SetActive(false);
                inventoryPanel.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        RaycastHit hit;

        Debug.DrawRay(character.position, character.forward * reachDistance, Color.green);

        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(character.position, character.forward, out hit, reachDistance)) {
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
