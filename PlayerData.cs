using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData{
    readonly string[] attributeKeys = {"level","honor","vitality","strength","intelligence","dexterity","endurance","luck"};
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

    public void UpdatePlayerData(string key, int value, bool isReplace = false) {
        if(_playerAttributesData.ContainsKey(key)){
            int originalValue = _playerAttributesData[key];
            _playerAttributesData[key] = isReplace? value:originalValue+value;
        }
        if(key == "money"){
            _money = isReplace? value:_money+value;
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
        return _playerAttributesData[key];
    }

    public Dictionary<string,int> GetAttributes(){
        return _playerAttributesData;
    }

    public string[] ToStringArray(bool fullAttributes = false){
        int outputLength = _playerAttributesData.Count - (fullAttributes?0:2);
        string[] output = new string[outputLength];
        int count = 0;
        for(int i = fullAttributes?0:2; i < _playerAttributesData.Count; i++){
            output[count++] = string.Format("{0}: {1}",attributeKeys[i],_playerAttributesData[attributeKeys[i]]);
        }
        return output;
    }


}