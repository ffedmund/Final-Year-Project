using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using UnityEngine;

public static class DataReader{

    public static List<Quest> questList = new List<Quest>();

    public static void ReadDataBase(){
        questList.Clear();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("./Assets/GameData/Data.xml");

        XmlNodeList passiveList = xmlDoc.GetElementsByTagName("passive");
        foreach (XmlNode node in passiveList)
        {
            string name = node["name"].InnerText;
            string id = node["id"].InnerText;
            string description = node["description"].InnerText;
            string iconId = node["icon_id"].InnerText;
            string background = node["background"].InnerText;
        }

        XmlNodeList artifactsList = xmlDoc.GetElementsByTagName("artifacts");
        foreach (XmlNode node in artifactsList)
        {
            string name = node["name"].InnerText;
            string id = node["id"].InnerText;
            string iconId = node["icon_id"].InnerText;
            string prefabId = node["prefab_id"].InnerText;
            string description = node["description"].InnerText;
        }

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
}
