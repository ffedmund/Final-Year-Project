using System;
using System.Collections.Generic;
using FYP;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class QuestBoardUI : MonoBehaviour {
    
    public Transform questUIPrefabs;

    Transform interactingQuestBoard;
    List<Quest> currentQuestList = new List<Quest>();
    QuestList questList;
    QuestGiver questGiver;
    QuestReceiver questReceiver;

    public void Setup(QuestList questList){
        this.questList = questList;
        this.currentQuestList = questList.currentQuestList;
        if(!TryGetComponent(out questGiver) || !TryGetComponent(out questReceiver)){
            this.enabled = false;
        }
    }

    void OnDisable() {
        if(interactingQuestBoard){
            interactingQuestBoard.TryGetComponent(out Collider collider);
            collider.enabled = true;
            interactingQuestBoard = null;
        }
    }

    public void UpdateQuestWindow(){
        if(questList)questList.GetQuests();
        PlayerData playerData = FindAnyObjectByType<PlayerManager>().playerData;
        List<Quest> suitableQuestList = new List<Quest>();
        suitableQuestList.AddRange(currentQuestList.FindAll(quest => quest.questType == QuestType.Regular));
        suitableQuestList.AddRange(currentQuestList.FindAll(quest => quest.questType == QuestType.Rank && ((int)quest.honorRank == playerData.GetHonorLevel() || (int)quest.honorRank == playerData.GetHonorLevel()+1)));
        int questPostUINumber = transform.childCount;
        while(questPostUINumber!=suitableQuestList.Count){
            if(questPostUINumber<suitableQuestList.Count){
                Instantiate(questUIPrefabs,transform);
                questPostUINumber++;
            }else{
                Destroy(transform.GetChild(transform.childCount-1));
                questPostUINumber--;
            }
        }

        for(int i = 0; i < suitableQuestList.Count; i++){
            Transform questUI = transform.GetChild(i);
            Transform acceptButtonTransform = questUI.Find("AcceptButton");
            Transform reportButtonTransform = questUI.Find("ReportButton");
            Quest quest = suitableQuestList[i];
            if(!quest.isFinished){
                acceptButtonTransform.GetComponent<Button>().onClick.RemoveAllListeners();
                questUI.Find("Title").GetComponent<TextMeshProUGUI>().SetText(quest.title);
                questUI.Find("RankText").GetComponent<TextMeshProUGUI>().SetText("RANK " +quest.honorRank.ToString());
                questUI.Find("ScrollArea").Find("Content").Find("Description").GetComponent<TextMeshProUGUI>().SetText(quest.description);

                if(quest.goalChecker.isReached() && questList.CanReportQuest(quest)){
                    acceptButtonTransform.gameObject.SetActive(false);
                    reportButtonTransform.gameObject.SetActive(true);
                    reportButtonTransform.GetComponent<Button>().onClick.AddListener(() => questReceiver.ReportQuest(quest));
                    reportButtonTransform.GetComponent<Button>().onClick.AddListener(() => UpdateQuestWindow());
                }else{
                    acceptButtonTransform.GetComponent<Button>().onClick.AddListener(() => questGiver.AcceptQuest(quest));
                    acceptButtonTransform.GetComponent<Button>().onClick.AddListener(() => UpdateQuestWindow());
                    acceptButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(quest.isActive?"ACCEPTED":"ACCEPT");
                    acceptButtonTransform.gameObject.SetActive(true);
                    reportButtonTransform.gameObject.SetActive(false);
                }

                questUI.gameObject.SetActive(true);
            }else{
                questUI.gameObject.SetActive(false);

            }
        }
    }

    public void InteractingQuestBoard(Transform transform){
        interactingQuestBoard = transform;
        transform.TryGetComponent(out Collider collider);
        collider.enabled = false;
        UpdateQuestWindow();
    } 
}