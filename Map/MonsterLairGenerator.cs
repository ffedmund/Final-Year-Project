using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LairSetting
{
    public GameObject chestPrefabs;
    public GameObject[] monsterPrefabsArray;
    public int chestNumber;
    public int monsterNumber;
    public int averageLevel;
    [Range(1,4)]
    public int monsterSpawnRange;
}

public class MonsterLairGenerator : MonoBehaviour
{   
    public LayerMask layerMask;
    public LairSetting[] lairSettings;

    public void CreateMonsterLair(List<Vector3> monsterLairCenters, Vector3 chunkCenterPosition){
        int seed = EndlessTerrain.mapGenerator.seed;

        System.Random random = new System.Random(seed);
        chunkCenterPosition = new Vector3(chunkCenterPosition.x,0,chunkCenterPosition.y);

        foreach (Vector3 lairCenter in monsterLairCenters)
        {
            LairSetting rndLairSetting = lairSettings[random.Next(0,lairSettings.Length)];
            int monsterPrefabsArrayLength = rndLairSetting.monsterPrefabsArray.Length;
            int monsterSpawnRange = rndLairSetting.monsterSpawnRange;

            for(int i = 0; i < rndLairSetting.chestNumber; i++){
                Quaternion randomRotation = Quaternion.Euler(0f, random.Next(0, 180), 0f);
                Vector3 position = (lairCenter + new Vector3(i/2.0f,10,i/2.0f) + chunkCenterPosition)*EndlessTerrain.scale;
                if (Physics.Raycast(position, Vector3.down ,out RaycastHit hit, 200f, layerMask)) {
                    position.y = hit.point.y;
                }
                Instantiate(rndLairSetting.chestPrefabs,position,randomRotation);
            }

            for(int i = 1; i < rndLairSetting.monsterNumber+1; i++){
                Vector3 position = (lairCenter + new Vector3(random.Next(-5,5)/10.0f*monsterSpawnRange,10,random.Next(-5,5)/10.0f*monsterSpawnRange) + chunkCenterPosition)*EndlessTerrain.scale;
                if (Physics.Raycast(position, Vector3.down ,out RaycastHit hit, 200f, layerMask)) {
                    position.y = hit.point.y;
                }
                Instantiate(rndLairSetting.monsterPrefabsArray[random.Next(0,monsterPrefabsArrayLength)],position,Quaternion.identity);
            }
        }
    }
}
