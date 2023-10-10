using UnityEngine;
using IngameDebugConsole;

public class TestingCommand : MonoBehaviour{
    
    [ConsoleMethod( "attribute", "Edit player attribute by key and value")]
    public static void attribute(string key, int value){
        UIController.playerData.UpdatePlayerData(key,value);
    }

}