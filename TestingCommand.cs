using UnityEngine;
using IngameDebugConsole;
using FYP;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class TestingCommand : MonoBehaviour{
    
    public NavMeshSurface navMeshSurface;

    [ConsoleMethod( "attribute", "Edit player attribute by key and value")]
    public static void attribute(string key, int value, bool isReplace = false){
        if(isReplace)UIController.playerData.ReplacePlayerData(key,value);
        else UIController.playerData.AddPlayerData(key,value);
    }

    [ConsoleMethod( "bake", "Edit player attribute by key and value")]
    public static void bake(){
        FindAnyObjectByType<TestingCommand>().navMeshSurface.BuildNavMesh();
    }

}