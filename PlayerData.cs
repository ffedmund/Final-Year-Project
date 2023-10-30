using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum HonorRank{
    D = 0,
    C = 1,
    B = 2,
    A = 3,
    S = 4
}

[System.Serializable]
public class PlayerData{
    readonly string[] attributeKeys = {"backgroundId","level","honor","vitality","strength","intelligence","dexterity","endurance","luck"};
    readonly string[] moneySign = {"","K","M","B"};

    public List<Quest> quests = new List<Quest>();

    Dictionary<string,int> _playerAttributesData;
    int _money;

    public PlayerData(){
        _playerAttributesData = new Dictionary<string, int>();
        foreach(string key in attributeKeys){
            _playerAttributesData[key] = 1;
        }
        _money = 0;
    }

    public void AddPlayerData(string key, int value) {
        if(_playerAttributesData.ContainsKey(key)){
            _playerAttributesData[key] += value;
        }
        if(key == "money"){
            _money += value;
        }
    }

    public void ReplacePlayerData(string key, int value){
        if(_playerAttributesData.ContainsKey(key)){
            _playerAttributesData[key] = value;
        }
        if(key == "money"){
            _money = value;
        }
    }

    public string GetMoneyAmount(){
        int temp = _money;
        int count = 0;
        string output = temp.ToString();
        while(temp >= 10){
            output = temp.ToString() + moneySign[count];
            temp/=1000;
            count++;
        }
        return output;
    }

    public int GetAttribute(string key){
        if(key == "money")return _money;
        return _playerAttributesData[key];
    }

    public int GetHonorLevel(){
        int honorLevel = Mathf.FloorToInt(Mathf.Sqrt(_playerAttributesData["honor"]/100));

        return honorLevel;
    }

    public Dictionary<string,int> GetAttributes(){
        return _playerAttributesData;
    }

    public string[] ToStringArray(bool fullAttributes = false){
        int outputLength = _playerAttributesData.Count - (fullAttributes?0:3);
        string[] output = new string[outputLength];
        int count = 0;
        for(int i = fullAttributes?0:3; i < _playerAttributesData.Count; i++){
            output[count++] = string.Format("{0}: {1}",attributeKeys[i],_playerAttributesData[attributeKeys[i]]);
        }
        return output;
    }


}