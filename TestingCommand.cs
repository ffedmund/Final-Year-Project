using UnityEngine;
using IngameDebugConsole;
using FYP;

public class TestingCommand : MonoBehaviour{
    
    [ConsoleMethod( "attribute", "Edit player attribute by key and value")]
    public static void attribute(string key, int value, bool isReplace = false){
        if(isReplace)UIController.playerData.ReplacePlayerData(key,value);
        else UIController.playerData.AddPlayerData(key,value);
    }

}