using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP
{

    public class EquipmentWindowUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;

        HandEquipmentSlotUI[] handEquipmentSlotsUI;

        private void Start()
        {
            handEquipmentSlotsUI = GetComponentsInChildren<HandEquipmentSlotUI>();
        }

        public void LoadWeaponOnEquipmentScreen(PlayerInventory playerInventory)
        {
            // check if handEquipmentSlotsUI is null
            if (handEquipmentSlotsUI == null)
            {
                handEquipmentSlotsUI = GetComponentsInChildren<HandEquipmentSlotUI>();
            }

            for (int i = 0; i < handEquipmentSlotsUI.Length; i++)
            {
                if (handEquipmentSlotsUI[i].rightHandSlot01)
                {
                    handEquipmentSlotsUI[i].AddItem(playerInventory.weaponsItemsInRightHandSlots[0]);
                }
                else if (handEquipmentSlotsUI[i].rightHandSlot02)
                {
                    handEquipmentSlotsUI[i].AddItem(playerInventory.weaponsItemsInRightHandSlots[1]);
                }
                else if (handEquipmentSlotsUI[i].leftHandSlot01)
                {
                    handEquipmentSlotsUI[i].AddItem(playerInventory.weaponsItemsInLeftHandSlots[0]);
                }
                else if (handEquipmentSlotsUI[i].leftHandSlot02)
                {
                    handEquipmentSlotsUI[i].AddItem(playerInventory.weaponsItemsInLeftHandSlots[1]);
                }
                
            }
        }

        public void SelectRightHandSlot01()
        {
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }
    }

}