using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour {
    
    [SerializeField]
    List<Quest> quests = new List<Quest>();

    public PlayerData playerData;
    public Transform questUIPrefabs;

    Transform interactingQuestBoard;

    async void Awake() {
        if(quests.Count == 0){
            await DataReader.ReadQuestDataBase();
            quests = DataReader.questList;
            UpdateQuestWindow();
        }
    }

    void OnEnable() {
        UpdateQuestWindow();
    }

    void OnDisable() {
        if(interactingQuestBoard){
            interactingQuestBoard.TryGetComponent(out Collider collider);
            collider.enabled = true;
            interactingQuestBoard = null;
        }
    }

    public void UpdateQuestWindow(){
        foreach(Transform child in transform){
            Destroy(child.gameObject);
        }
        
        foreach(Quest quest in quests){
            Transform questUI = Instantiate(questUIPrefabs,this.transform);
            questUI.Find("Title").GetComponent<TextMeshProUGUI>().SetText(quest.title);
            questUI.Find("Description").GetComponent<TextMeshProUGUI>().SetText(quest.description);
            questUI.Find("AcceptButton").GetComponent<Button>().onClick.AddListener(() => AcceptQuest(quest));
            if(transform.childCount == 6){
                break; 
            }
        }
    }

    public void AcceptQuest(Quest quest){
        PlayerData playerData = FindAnyObjectByType<PlayerManager>().playerData;
        if(playerData != null && !playerData.quests.Contains(quest)){
            quest.isActive = true;
            playerData.quests.Add(quest);
            quests.Remove(quest);
        }
        UpdateQuestWindow();
    }

    public void InteractingQuestBoard(Transform transform){
        interactingQuestBoard = transform;
        transform.TryGetComponent(out Collider collider);
        Debug.Log(collider);
        collider.enabled = false;
    }

}