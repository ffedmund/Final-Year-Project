using System.Collections.Generic;
using System.Threading.Tasks;
using FYP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestList : MonoBehaviour {
    
    public QuestType[] questTypes;
    public int[] specialQuestIds;
    public int questRefreshFrequency;
    public List<Quest> currentQuestList = new List<Quest>();
    public float previousTime = 0;

    Dictionary<int,List<Quest>> questDictionary = new Dictionary<int, List<Quest>>();

    public async Task LoadQuestList(){
        await DataReader.ReadQuestDataBase();
        await Task.Run(() => {
            foreach(QuestType questType in questTypes){
                AddQuestList(questType);
            }
            InitQuests();
        });     
        previousTime = Time.time;   
    }

    void AddQuestList(QuestType questType){
        switch(questType){
            case QuestType.Special:
                foreach(int id in specialQuestIds){
                    if(!questDictionary.ContainsKey((int)QuestType.Rank*10)){
                        questDictionary.Add((int)questType*10,new List<Quest>());
                    }   
                    questDictionary[(int)questType*10].Add(DataReader.specialQuestList.Find(quest => quest.id == id));
                }
                break;
            case QuestType.Rank:
                foreach(HonorRank honorRank in DataReader.rankQuestList.Keys){
                    questDictionary.Add((int)questType*10+(int)honorRank,DataReader.rankQuestList[honorRank]); 
                }
                break;
            default:
                if(!questDictionary.ContainsKey((int)QuestType.Rank*10)){
                    questDictionary.Add((int)questType*10, DataReader.regularQuestList);
                }
                break;
        }
    }

    void InitQuests(){
        if(specialQuestIds.Length > 0){
            currentQuestList = questDictionary[(int)QuestType.Special*10];
        }else{
            foreach(int key in questDictionary.Keys){
                for(int i = 0; i < 2; i ++){
                    if(questDictionary[key].Count > i){
                        // int rndQuestNumber = Random.Range(0,questDictionary[key].Count);
                        currentQuestList.Add(questDictionary[key][i]);
                    }
                }
            }
        }
    }

    public void GetQuests(){
        if(specialQuestIds.Length > 0){
            currentQuestList = questDictionary[(int)QuestType.Special*10];
        }else if(Time.time - previousTime > questRefreshFrequency){
            foreach(Quest quest in currentQuestList){
                if(quest.isFinished){
                    if(quest.questType == QuestType.Regular){
                        if(quest.isActive && quest.isFinished){
                            quest.isFinished = false;
                            quest.isActive = false;
                            quest.goalChecker.currentAmount = 0;
                        }
                    }
                    currentQuestList.Remove(quest);
                    int questDictKey = (int)quest.questType*10+(int)quest.honorRank;
                    Quest newRankQuest = questDictionary[questDictKey][Random.Range(0,questDictionary[questDictKey].Count)];
                    currentQuestList.Add(newRankQuest);
                }
            }
            previousTime = Time.time;
        }
    }

    public bool CanReportQuest(Quest quest){
        for(int i = 0; i < questTypes.Length; i ++){
            if(quest.questType == questTypes[i]){
                if(quest.questType == QuestType.Regular || quest.questType == QuestType.Rank){
                    return true;
                }else{
                    foreach(int id in specialQuestIds){
                        if(quest.id == id){
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    
}