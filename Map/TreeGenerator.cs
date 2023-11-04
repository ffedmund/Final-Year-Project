using UnityEngine;


public class TreeGenerator: MonoBehaviour
{
    public const float coverage = 35;

    [SerializeField]
    GameObject[] treePrefabs;
    public LayerMask layerMask;
    [Range(0,10)]
    public float rndOffsetRange;

    public static bool GenerateTreeMapUnit(Vector2 position, float seed)
    {

        float xCoord = position.x + seed;
        float yCoord = position.y + seed;
        float noise = Mathf.PerlinNoise(xCoord, yCoord);
        // If the noise is greater than coverage rate, place a tree
        return noise > (1 - coverage/100);
        
    }

    public void CreateTrees(Transform parentChunk, bool[,] treeMap, Vector2 chunkPosition){
            float scale = EndlessTerrain.scale;
            // Debug.Log(chunkPosition);
            for(int y = 0; y < treeMap.GetLength(1); y++){
                for(int x = 0; x < treeMap.GetLength(0); x++){
                        //[Need Fixed] Dont know why the matrix need to be rotate
                        if(treeMap[x,y]){
                            GameObject rndTreePrefab = treePrefabs[Random.Range(0,treePrefabs.Length)];

                            float rndOffset = Random.Range(-rndOffsetRange,rndOffsetRange);
                            float treePositionX =(chunkPosition.x + x-treeMap.GetLength(0)/2)*scale;
                            float treePositionZ =(chunkPosition.y+ y-treeMap.GetLength(0)/2)*scale;
                            Vector3 treePosition = new Vector3(treePositionX,50,treePositionZ);
                            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 180f), 0f);
                            if (Physics.Raycast(new Vector3(treePositionX,100,treePositionZ), Vector3.down ,out RaycastHit hit, 200f, layerMask)) {
                                treePosition = new Vector3(treePositionX + rndOffset,hit.point.y,treePositionZ + rndOffset);
                            }
                            // Debug.Log(treePositionX + " " + treePositionZ);
                            GameObject tree = Instantiate(rndTreePrefab,treePosition, randomRotation);
                            tree.isStatic = true;
                            tree.transform.parent = parentChunk;
                            tree.transform.localScale *= scale/10;
                        }
                }
            }
    } 
}
