using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FYP
{
    public class InventorySlot : MonoBehaviour
    {

        public Image icon;
        public TextMeshProUGUI text;
        Item item;

        public void AddItem<T>(T newItem)
        {
            if(newItem is Item){
                item = (Item)(object)newItem;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                text.enabled = false;
                gameObject.SetActive(true);
            }
        }

        public void AddItem<T>(T newItem, int number)
        {
            if(newItem is Item){
                item = (Item)(object)newItem;
                icon.sprite = item.itemIcon;
                icon.enabled = true;
                text.enabled = true;
                text.SetText(number>0?number.ToString():"");
                gameObject.SetActive(true);
            }
        }

        public void AddItem(WeaponItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            text.enabled = false;
            gameObject.SetActive(true);
        }

        public Item GetItem(){
            return item;
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }
    }
}

