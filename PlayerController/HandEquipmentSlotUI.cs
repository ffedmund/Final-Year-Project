using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FYP
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIController uiController;

        public Image icon;
        WeaponItem weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool leftHandSlot01;
        public bool leftHandSlot02;

        private void Awake() {
            uiController = FindObjectOfType<UIController>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            weapon = newWeapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            uiController.ResetAllSelectedSlots();
            if (rightHandSlot01)
            {
                uiController.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiController.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiController.leftHandSlot01Selected = true;
            }
            else if (leftHandSlot02)
            {
                uiController.leftHandSlot02Selected = true;
            }
        }
    }
}