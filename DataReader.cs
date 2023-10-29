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

    public static List<Quest> questList = new List<Quest>();
    public static Dictionary<int,PlayerBackground> backgorundDictionary = new Dictionary<int, PlayerBackground>();
    public static Dictionary<int,PlayerAbility> skillDictionary = new Dictionary<int, PlayerAbility>();
    public static Dictionary<int,PlayerAbility> talentDictionary = new Dictionary<int, PlayerAbility>();
    public static Dictionary<int,PlayerTarget> roleTargetDictionary = new Dictionary<int, PlayerTarget>();
    public static List<PlayerTarget> targetList = new List<PlayerTarget>();


    public static async Task ReadBackgroundDataBase(){
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
    }

    public static async Task ReadQuestDataBase(){
        await Task.Run(() => {
            questList.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/QuestData.xml");

            XmlNodeList questDataList = xmlDoc.GetElementsByTagName("quest");
            foreach (XmlNode node in questDataList)
            {   
                int id = int.Parse(node["id"].InnerText);
                string title = node["title"].InnerText;
                string description = node["description"].InnerText;
                int moneyReward = int.Parse(node["moneyReward"].InnerText);
                int honorReward = int.Parse(node["honorReward"].InnerText);
                string targetNPC = node["targetNPC"].InnerText;
                string completeDialog = node["completeDialog"].InnerText;
                GoalType goalType = (GoalType)int.Parse(node["goalType"].InnerText);
                string targetID = node["targetID"].InnerText;
                int targetAmount = int.Parse(node["targetAmount"].InnerText);
                Quest quest = new Quest(id,title,description,moneyReward,honorReward,targetNPC,completeDialog);
                quest.goalChecker = new GoalChecker(goalType, targetAmount,targetID);
                questList.Add(quest);
            }
        });
    }

    public static async Task ReadTargetDataBase(){
        await Task.Run(() => {
            questList.Clear();

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
    }

     public static async Task ReadAbilityDataBase(){
        await Task.Run(() => {
            questList.Clear();

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
    }

    public static async Task ReadDataBase(){
        await Task.Run(() => {
            questList.Clear();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("./Assets/GameData/Data.xml");

            XmlNodeList artifactsList = xmlDoc.GetElementsByTagName("artifacts");
            foreach (XmlNode node in artifactsList)
            {
                string name = node["name"].InnerText;
                string id = node["id"].InnerText;
                string iconId = node["icon_id"].InnerText;
                string prefabId = node["prefab_id"].InnerText;
                string description = node["description"].InnerText;
            }
        });
    }
}
