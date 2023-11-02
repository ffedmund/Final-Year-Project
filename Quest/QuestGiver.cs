using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour {

    public QuestList _questList;
    QuestBoardUI questBoardUI;

    async void Awake() {
        await _questList.LoadQuestList();
        if(TryGetComponent(out questBoardUI))
        {   
            questBoardUI.Setup(_questList);
            questBoardUI.UpdateQuestWindow();
        }
    }

    public void AcceptQuest(Quest quest){
        PlayerData playerData = FindAnyObjectByType<PlayerManager>().playerData;
        Dictionary<string,int> materialsNumberDictionary = FindAnyObjectByType<PlayerInventory>().materialsNumberDictionary;
        if(playerData != null && !playerData.quests.Contains(quest)){
            quest.isActive = true;
            playerData.quests.Add(quest);
        }
        if((quest.goalChecker.goalType == GoalType.Gathering || quest.goalChecker.goalType == GoalType.Delivery) && materialsNumberDictionary.ContainsKey(quest.goalChecker.targetId)){
            quest.goalChecker.currentAmount = materialsNumberDictionary[quest.goalChecker.targetId];
        }
    }

}