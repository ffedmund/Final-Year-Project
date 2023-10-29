using System;
using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class QuestBoardUI : MonoBehaviour {
    
    public Transform questUIPrefabs;

    Transform interactingQuestBoard;
    List<Quest> quests = new List<Quest>();
    QuestGiver questGiver;
    QuestReceiver questReceiver;

    public void Setup(List<Quest> quests){
        this.quests = quests;
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
        int questPostUINumber = transform.childCount;
        while(questPostUINumber!=quests.Count){
            if(questPostUINumber<quests.Count){
                Instantiate(questUIPrefabs,transform);
                questPostUINumber++;
            }else{
                Destroy(transform.GetChild(transform.childCount-1));
                questPostUINumber--;
            }
        }

        for(int i = 0; i < quests.Count; i++){
            Transform questUI = transform.GetChild(i);
            Transform acceptButtonTransform = questUI.Find("AcceptButton");
            Transform claimButtonTransform = questUI.Find("ClaimButton");
            Quest quest = quests[i];
            if(!quest.isFinished &&  questGiver.questIdList.Contains(quest.id)){
                acceptButtonTransform.GetComponent<Button>().onClick.RemoveAllListeners();
                questUI.Find("Title").GetComponent<TextMeshProUGUI>().SetText(quest.title);
                questUI.Find("Description").GetComponent<TextMeshProUGUI>().SetText(quest.description);

                if(quest.goalChecker.isReached() && questReceiver.questIdList.Contains(quest.id)){
                    acceptButtonTransform.gameObject.SetActive(false);
                    claimButtonTransform.gameObject.SetActive(true);
                    claimButtonTransform.GetComponent<Button>().onClick.AddListener(() => questReceiver.ReportQuest(quest));
                    claimButtonTransform.GetComponent<Button>().onClick.AddListener(() => UpdateQuestWindow());
                }else{
                    acceptButtonTransform.GetComponent<Button>().onClick.AddListener(() => questGiver.AcceptQuest(quest));
                    acceptButtonTransform.GetComponent<Button>().onClick.AddListener(() => UpdateQuestWindow());
                    acceptButtonTransform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(quest.isActive?"ACCEPTED":"ACCEPT");
                    acceptButtonTransform.gameObject.SetActive(true);
                    claimButtonTransform.gameObject.SetActive(false);
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