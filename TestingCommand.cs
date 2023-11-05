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

    [ConsoleMethod( "bake", "Bake the AI navigation")]
    public static void bake(){
        FindAnyObjectByType<TestingCommand>().navMeshSurface.BuildNavMesh();
    }

    [ConsoleMethod( "treechunk", "Get the world tree chunk")]
    public static void treechunk(){
        Debug.Log(EndlessTerrain.worldTreePosition/(MapGenerator.mapChunkSize-1));
    }

}