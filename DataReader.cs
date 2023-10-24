using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using UnityEngine;

public struct PlayerBackground{
    public int id;
    public string role;
    public string description;
}

public static class DataReader{

    public static List<Quest> questList = new List<Quest>();
    public static Dictionary<int,PlayerBackground> backgorundDictionary = new Dictionary<int, PlayerBackground>();

    public static void ReadBackgroundDataBase(){
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
    }

    public static void ReadQuestDataBase(){
        questList.Clear();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("./Assets/GameData/QuestData.xml");

        XmlNodeList questDataList = xmlDoc.GetElementsByTagName("quest");
        foreach (XmlNode node in questDataList)
        {
            string title = node["title"].InnerText;
            string description = node["description"].InnerText;
            int moneyReward = int.Parse(node["moneyReward"].InnerText);
            int honorReward = int.Parse(node["honorReward"].InnerText);
            GoalType goalType = (GoalType)int.Parse(node["goalType"].InnerText);
            string targetID = node["targetID"].InnerText;
            int targetAmount = int.Parse(node["targetAmount"].InnerText);
            
            Quest quest = new Quest(title,description,moneyReward,honorReward);
            quest.goalChecker = new GoalChecker(goalType, targetAmount);
            questList.Add(quest);
        }
    }

    public static void ReadDataBase(){
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
    }
}
