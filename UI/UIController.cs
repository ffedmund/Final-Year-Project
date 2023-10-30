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
        public GameObject chatBoxWindow;
        public GameObject tradeWindow;


        [Header("Weapon Inventory")]
        // public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotParent;
        // WeaponInventorySlot[] weaponInventorySlots;
        

        [Header("Inventory")]
        public GameObject inventorySlotPrefab;
        public Transform inventorySlotParent;
        InventorySlot[] inventorySlots;

        public static PlayerData playerData;
        int showingInventoryIndex;

        [Header("Other")]
        public TextMeshProUGUI honorLevelText;
        public GameObject currentInteractWindow;
        public List<GameObject> activeUIWindows = new List<GameObject>();

        private void Awake()
        {
            // equipmentWindowUI = FindObjectOfType<EquipmentWindowUI>();
        }

        void Start()
        {
            playerData = FindAnyObjectByType<PlayerManager>().playerData;

            // weaponInventorySlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponInventorySlot>();
            inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();

            equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
            SetHonorText();
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
                await DataReader.ReadBackgroundDataBase();
                playerInfoUI.gameObject.SetActive(!playerInfoUI.gameObject.active);
                playerInfoUI.transform.FindChild("ContentArea").GetComponent<PlayerInfoContentScript>().ShowBackgroundInfo();
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
                    if(quest.goalChecker.isReached()){
                        questInfo += quest.ToString(color:"green");
                    }else{
                        questInfo += quest.ToString();
                    }
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
                    // Debug.Log("Material Inventory: " + playerInventory.materialsInventory[0]);
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
                        inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>(true);
                    }
                    if(inventory[i] is WeaponItem){
                        inventorySlots[i].AddItem(inventory[i]);
                    }else if(inventory[i] is MaterialItem && !createdSlotList.Contains(((Item)(object)inventory[i]).name)){
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

        public void SetHonorText(){
            string honorLevelSting = "D";
            switch(playerData.GetHonorLevel()){
                case 1:
                    honorLevelSting = "C";
                    break;
                case 2:
                    honorLevelSting = "B";
                    break;
                case 3:
                    honorLevelSting = "A";
                    break;
                case 4:
                    honorLevelSting = "S";
                    break;
                default:
                    break;
            }
            honorLevelText.SetText(honorLevelSting);
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