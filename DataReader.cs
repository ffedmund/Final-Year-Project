using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System;

[System.Serializable]
public struct PlayerRole{
    public PlayerBackground playerBackground;
    public PlayerAbility playerSkill;
    public PlayerAbility playerTalent;
    public List<PlayerTarget> playerTargets;
}

[System.Serializable]
public struct PlayerBackground{
    public int id;
    public string role;
    public string description;
}

[System.Serializable]
public struct PlayerTarget{
    public int id;
    public string title;
    public string detail;
    public string shortDescription;
    public bool isRoleTarget;
}

[System.Serializable]
public struct PlayerAbility{
    public int id;
    public string name;
    public string description;
}

public static class DataReader{

    public static List<Quest> regularQuestList = new List<Quest>();
    public static Dictionary<HonorRank,List<Quest>> rankQuestList = new Dictionary<HonorRank, List<Quest>>();
    public static List<Quest> specialQuestList = new List<Quest>();
    public static Dictionary<int,PlayerBackground> backgorundDictionary = new Dictionary<int, PlayerBackground>();
    public static Dictionary<int,PlayerAbility> skillDictionary = new Dictionary<int, PlayerAbility>();
    public static Dictionary<int,PlayerAbility> talentDictionary = new Dictionary<int, PlayerAbility>();
    public static Dictionary<int,PlayerTarget> roleTargetDictionary = new Dictionary<int, PlayerTarget>();
    public static Dictionary<string,int> basicPriceDictionary = new Dictionary<string, int>();
    public static List<PlayerTarget> targetList = new List<PlayerTarget>();

    static bool questDataLoaded;
    static bool backgroundDataLoaded;
    static bool targetDataLoaded;
    static bool abilityDataLoaded;
    static bool priceDataLoaded;

    public static async Task ReadPriceDataBase(){
        if(priceDataLoaded)return;
        await Task.Run(() => {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/PriceData.xml");

            XmlNodeList priceList = xmlDoc.GetElementsByTagName("price");
            foreach(XmlNode node in priceList){
                string itemId = node["id"].InnerText;
                int price = int.Parse(node["basicPrice"].InnerText);
                basicPriceDictionary.Add(itemId,price);
            }
        });
        priceDataLoaded = true;
    }


    public static async Task ReadBackgroundDataBase(){
        if(backgroundDataLoaded)return;
        await Task.Run(() => {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/BackgroundData.xml");

            XmlNodeList backgroundList = xmlDoc.GetElementsByTagName("background");
            foreach (XmlNode node in backgroundList)
            {
                PlayerBackground playerBackground;
                playerBackground.id = int.Parse(node["id"].InnerText);
                playerBackground.description = node["description"].InnerText;
                playerBackground.role = node["role"].InnerText;
                if(!backgorundDictionary.ContainsKey(playerBackground.id)){
                    backgorundDictionary.Add(playerBackground.id,playerBackground);
                }
            }
        });
        backgroundDataLoaded = true;
    }

    public static async Task ReadQuestDataBase(){
        if(questDataLoaded)return;
        await Task.Run(() => {
            rankQuestList.Clear();
            regularQuestList.Clear();
            specialQuestList.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/QuestData.xml");

            XmlNodeList questDataList = xmlDoc.GetElementsByTagName("quest");
            foreach (XmlNode node in questDataList)
            {   
                int id = int.Parse(node["id"].InnerText);
                QuestType questType = (QuestType)int.Parse(node["questType"].InnerText);
                HonorRank honorRank = questType == QuestType.Rank? (HonorRank)int.Parse(node["requiredRank"].InnerText):HonorRank.D;
                string title = node["title"].InnerText;
                string description = node["description"].InnerText;
                int moneyReward = int.Parse(node["moneyReward"].InnerText);
                int honorReward = int.Parse(node["honorReward"].InnerText);
                string itemReward = node["itemReward"].InnerText;
                int itemRewardAmount = itemReward != ""?int.Parse(node["itemRewardAmount"].InnerText):0;
                string targetNPC = node["targetNPC"].InnerText;
                string completeDialog = node["completeDialog"].InnerText;
                GoalType goalType = (GoalType)int.Parse(node["goalType"].InnerText);
                string targetID = node["targetID"].InnerText;
                int targetAmount = int.Parse(node["targetAmount"].InnerText);
                Quest quest = new Quest(id,questType,honorRank,title,description,moneyReward,honorReward,itemReward,itemRewardAmount,targetNPC,completeDialog);
                quest.goalChecker = new GoalChecker(goalType, targetAmount,targetID);
                
                switch(questType){
                    case QuestType.Special:
                        specialQuestList.Add(quest);
                        break;
                    case QuestType.Rank:
                        if(!rankQuestList.ContainsKey(honorRank)){
                            rankQuestList.Add(honorRank,new List<Quest>());
                        }
                        rankQuestList[honorRank].Add(quest);
                        break;
                    default:
                        regularQuestList.Add(quest);
                        break;
                }
            }
        });
        questDataLoaded = true;
    }

    public static async Task ReadTargetDataBase(){
        if(targetDataLoaded)return;
        await Task.Run(() => {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/TargetData.xml");

            XmlNodeList targetDataList = xmlDoc.GetElementsByTagName("target");
            foreach (XmlNode node in targetDataList)
            {
                PlayerTarget playerTarget;
                playerTarget.id = int.Parse(node["id"].InnerText);
                playerTarget.title = node["title"].InnerText;
                playerTarget.detail = node["detail"].InnerText;
                playerTarget.shortDescription = node["short"].InnerText;
                playerTarget.isRoleTarget = Boolean.Parse(node["roleTarget"].InnerText);
                if(playerTarget.isRoleTarget){
                    roleTargetDictionary[int.Parse(node["roleId"].InnerText)] = playerTarget;
                }else{
                    targetList.Add(playerTarget);
                }
            }
        });
        targetDataLoaded = true;
    }

     public static async Task ReadAbilityDataBase(){
        if(abilityDataLoaded)return;
        await Task.Run(() => {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/AbilityData.xml");

            XmlNodeList skillDataList = xmlDoc.GetElementsByTagName("skill");
            foreach (XmlNode node in skillDataList)
            {
                PlayerAbility playerAbility;
                playerAbility.id = int.Parse(node["id"].InnerText);
                playerAbility.name = node["name"].InnerText;
                playerAbility.description = node["description"].InnerText;
                if(!skillDictionary.ContainsKey(playerAbility.id)){
                    skillDictionary.Add(playerAbility.id,playerAbility);
                }
            }

            XmlNodeList talentDataList = xmlDoc.GetElementsByTagName("talent");
            foreach (XmlNode node in talentDataList)
            {
                PlayerAbility playerAbility;
                playerAbility.id = int.Parse(node["id"].InnerText);
                playerAbility.name = node["name"].InnerText;
                playerAbility.description = node["description"].InnerText;
                if(!talentDictionary.ContainsKey(playerAbility.id)){
                    talentDictionary.Add(playerAbility.id,playerAbility);
                }
            }
        });
        abilityDataLoaded = true;
    }
}
