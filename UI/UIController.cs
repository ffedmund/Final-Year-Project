using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class UIController : MonoBehaviour {

    public Transform stateUI;
    public Transform interactionTipUI;
    public Transform questUI;
    public Transform playerInfoUI;
    public static PlayerData playerData;

    public Transform currentInteractObject;

    List<Transform> activeUIWindows = new List<Transform>();

    void Start(){
        playerData = FindAnyObjectByType<PlayerStats>().playerData;
        DataReader.ReadDataBase();
    }

    [System.Obsolete]
    private void Update() {
        questUI.gameObject.SetActive(playerData.quests.Count > 0);

        if(Input.GetKeyDown(KeyCode.U)){
            stateUI.gameObject.SetActive(!stateUI.gameObject.active);
            stateUI.GetComponent<StatusUIManager>().UpdateText();
            DataReader.ReadDataBase();
            activeUIWindows.Add(stateUI);
        }
        if(Input.GetKeyDown(KeyCode.P)){
            playerInfoUI.gameObject.SetActive(!playerInfoUI.gameObject.active);
            playerInfoUI.FindChild("ContentArea").GetComponent<PlayerInfoContentScript>().ShowBackgroundInfo();
            DataReader.ReadDataBase();
            activeUIWindows.Add(playerInfoUI);
        }
        if(Input.GetKeyDown(KeyCode.F)){
            if(currentInteractObject != null && currentInteractObject.GetComponent<InteractableScript>().isUITrigger){
                currentInteractObject.GetComponent<InteractableScript>().targetTransform.gameObject.SetActive(true);
                activeUIWindows.Add(currentInteractObject.GetComponent<InteractableScript>().targetTransform);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            foreach(Transform window in activeUIWindows){
                window.gameObject.SetActive(false);
            }
            activeUIWindows.Clear();
        }
        if(playerData.quests.Count > 0){
            string questInfo = "Quest List\n";
            foreach(Quest quest in playerData.quests){
                questInfo += quest.ToString();
            }
            questUI.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(questInfo);
        }
    }
}