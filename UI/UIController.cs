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
        // public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public GameObject stateUI;
        public GameObject questUI;
        public GameObject playerInfoUI;


        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotParent;
        WeaponInventorySlot[] weaponInventorySlots;
        

        [Header("Inventory")]
        public GameObject inventorySlotPrefab;
        public Transform inventorySlotParent;
        InventorySlot[] inventorySlots;

        public static PlayerData playerData;
        int showingInventoryIndex;

        [Header("Other")]
        public GameObject currentInteractWindow;
        public List<GameObject> activeUIWindows = new List<GameObject>();

        private void Awake()
        {
            // equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        }

        void Start()
        {
            playerData = FindAnyObjectByType<PlayerManager>().playerData;

            weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
            inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();

            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
        }

        [System.Obsolete]
        private async void Update()
        {
            questUI.gameObject.SetActive(playerData.quests.Count > 0);

            if (Input.GetKeyDown(KeyCode.U))
            {
                UpdateUI();
                stateUI.SetActive(!stateUI.activeSelf);
                if(stateUI.activeSelf){
                    activeUIWindows.Add(stateUI);
                }
            }
            if(Input.GetKeyDown(KeyCode.P)){
                playerInfoUI.gameObject.SetActive(!playerInfoUI.gameObject.active);
                playerInfoUI.transform.FindChild("ContentArea").GetComponent<PlayerInfoContentScript>().ShowBackgroundInfo();
                await DataReader.ReadBackgroundDataBase();
                activeUIWindows.Add(playerInfoUI);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (GameObject window in activeUIWindows)
                {
                    window.SetActive(false);
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
                questUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(questInfo);
            }
        }
        
        public void UpdateUI()
        {
            #region Weapon Inventory Slots

            // UpdateInventorySlot(playerInventory.weaponsInventory);

            #endregion

            #region Inventory Slots
            switch(showingInventoryIndex){
                case 1:
                    UpdateInventorySlot(playerInventory.materialsInventory);
                    break;
                default:
                    UpdateInventorySlot(playerInventory.weaponsInventory);
                    break;
            }
            #endregion

            stateUI.GetComponent<StatusUIManager>().UpdateText();
        }

        void UpdateInventorySlot<T>(List<T> inventory) {
            List<string> createdSlotList = new List<string>();
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (i < inventory.Count)
                {
                    if (inventorySlots.Length < inventory.Count)
                    {
                        Instantiate(inventorySlotPrefab, inventorySlotParent);
                        inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();
                    }
                    if(inventory[i] is WeaponItem){
                        inventorySlots[i].AddItem(inventory[i]);
                    }else if(!createdSlotList.Contains(((Item)(object)inventory[i]).name)){
                        inventorySlots[i].AddItem(inventory[i],playerInventory.materialsNumberDictionary[((Item)(object)inventory[i]).name]);
                        createdSlotList.Add(((Item)(object)inventory[i]).name);
                    }else{
                        inventorySlots[i].ClearInventorySlot();
                    }
                }
                else
                {
                    inventorySlots[i].ClearInventorySlot();
                }
            }
        }

        public void ChangeShowingInventory(int index){
            showingInventoryIndex = index;
            UpdateUI();
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
            // equipmentScreenWindow.SetActive(false);
        }
    }
}