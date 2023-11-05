using UnityEngine;

public class WorldTreeSpawner : MonoBehaviour {
    public GameObject worldTree;
    static bool isCreated;

    public static void Create(Transform parent = null){
        if(isCreated)return;
        Vector3 position =new Vector3(EndlessTerrain.worldTreePosition.x,0,EndlessTerrain.worldTreePosition.y)*EndlessTerrain.scale;
        GameObject worldTreeObject = Instantiate(FindAnyObjectByType<WorldTreeSpawner>().worldTree,position,Quaternion.identity);
        isCreated = true;
        if(parent){
            worldTreeObject.transform.SetParent(parent);
        }
    }   
}