using System.Diagnostics;
using System.Xml;
using UnityEngine;

public static class DataReader{

    public static void ReadDataBase(){
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
    }
}
