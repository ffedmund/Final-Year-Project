using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestReceiver : MonoBehaviour {
    QuestList questList;

    void Start(){
        questList = GetComponent<QuestList>();
        if(!questList){
            enabled = false;
        }
    }


    public void ReportQuests(List<Quest> quests){
        foreach (Quest quest in quests)
        {
            if(questList.CanReportQuest(quest) && quest.goalChecker.isReached()){
                quests.Remove(quest);
                PlayerData playerData = FindAnyObjectByType<PlayerManager>().playerData;
                playerData.AddPlayerData("money",quest.moneyReward);
                playerData.AddPlayerData("honor",quest.honorReward);
                quest.isFinished = true;
                if(quest.goalChecker.goalType == GoalType.Delivery){
                    ReceiveItem(quest);
                }
            }
        }
    }

    public void ReportQuest(Quest reportQuest){
        if(questList.CanReportQuest(reportQuest) && reportQuest.goalChecker.isReached()){
            PlayerData playerData = FindAnyObjectByType<PlayerManager>().playerData;
            playerData.quests.Remove(reportQuest);
            playerData.AddPlayerData("money",reportQuest.moneyReward);
            playerData.AddPlayerData("honor",reportQuest.honorReward);
            FindAnyObjectByType<UIController>().UpdateHonorUI();
            reportQuest.isFinished = true;
            if(reportQuest.goalChecker.goalType == GoalType.Delivery){
                ReceiveItem(reportQuest);
            }
        }
    }

    void ReceiveItem(Quest quest){
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        playerManager = FindAnyObjectByType<PlayerManager>();
        playerInventory = playerManager.GetComponent<PlayerInventory>();
        TryGetComponent(out NPCInventory npcInventory);

        for(int i = 0; i < quest.goalChecker.targetAmount; i++){
            MaterialItem materialItem = playerInventory.materialsInventory.Find(item => item.name == quest.goalChecker.targetId);
            playerInventory.materialsInventory.Remove(materialItem);
            
            //If Receiver is NPC add it into NPC's inventory
            if(npcInventory){
                npcInventory.npcInventory.Add((Item)(object)materialItem);
            }
        }
        playerInventory.materialsNumberDictionary[quest.goalChecker.targetId]-=quest.goalChecker.targetAmount;
    }

}