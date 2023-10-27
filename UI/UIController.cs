using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace FYP
{
    public class UIController : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentWindowUI equipmentWindowUI;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject weaponInventoryWindow;


        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotParent;
        WeaponInventorySlot[] weaponInventorySlots;

        public Transform stateUI;
        public Transform interactionTipUI;
        public Transform questUI;
        public Transform playerInfoUI;
        public static PlayerData playerData;

        public Transform currentInteractObject;

        public List<Transform> activeUIWindows = new List<Transform>();

        private void Awake()
        {
            // equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        }

        void Start()
        {
            playerData = FindAnyObjectByType<PlayerManager>().playerData;

            weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();

            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
        }

        [System.Obsolete]
        private async void Update()
        {
            questUI.gameObject.SetActive(playerData.quests.Count > 0);

            if (Input.GetKeyDown(KeyCode.U))
            {
                
                activeUIWindows.Add(stateUI);
            }
            if(Input.GetKeyDown(KeyCode.P)){
                playerInfoUI.gameObject.SetActive(!playerInfoUI.gameObject.active);
                playerInfoUI.FindChild("ContentArea").GetComponent<PlayerInfoContentScript>().ShowBackgroundInfo();
                await DataReader.ReadBackgroundDataBase();
                activeUIWindows.Add(playerInfoUI);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (Transform window in activeUIWindows)
                {
                    window.gameObject.SetActive(false);
                }
                activeUIWindows.Clear();
            }
            if (playerData.quests.Count > 0)
            {
                string questInfo = "Quest List\n";
                foreach (Quest quest in playerData.quests)
                {
                    questInfo += quest.ToString();
                }
                questUI.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(questInfo);
            }
        }
        
        public void UpdateUI()
        {
            #region Weapon Inventory Slots

            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotParent);
                        weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    
                    weaponInventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();
                }
            }

            #endregion

            #region Player Stats

            stateUI.gameObject.SetActive(!stateUI.gameObject.activeSelf);
            stateUI.GetComponent<StatusUIManager>().UpdateText();

            #endregion
        }       

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            weaponInventoryWindow.SetActive(false);
        }
    }
}