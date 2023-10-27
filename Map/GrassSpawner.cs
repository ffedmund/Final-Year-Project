
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GrassSpawner : MonoBehaviour {
    public Mesh mesh;
    public Material material;
    [Range(0.1f,100)]
    public float density;
    [Range(1,20)]
    public int visibleRange;
    List<Matrix4x4[]> matricesList = new List<Matrix4x4[]>();
    Material matclone;
    Vector3 position;
    Quaternion rotation;
    Vector3 scale;
    Vector2 previousCoordinate;

    Dictionary<Vector2,Matrix4x4> positionGrassDictionary = new Dictionary<Vector2, Matrix4x4>();
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();// keep the reference for all generated terrain chunks
    

    void Start(){
    }

    public void SetUpTerrainChunkpDictionary(Dictionary<Vector2, TerrainChunk> targetDictionary){
        terrainChunkDictionary = targetDictionary;
        Debug.Log("Hello");
    }

    //New Update Visible Grass Function
    //Can See the Grass not only in one chunk, across the chunk is posiible
    //Using world coordinate as the posiiton x and z
    //Using height map coordinate for position y
    //Todo: Sometime the height is wrong.
    public void UpdateVisibleGrass(float[,] heightMap, Vector2 viewerPosition){
        int coordinateX = Mathf.RoundToInt(viewerPosition.x);
        int coordinateY = Mathf.RoundToInt(viewerPosition.y);
        if(new Vector2(coordinateX,coordinateY) != previousCoordinate){
            matricesList.Clear();
            float diastem = Mathf.RoundToInt(1.0f/density*1000)/1000.0f;
            matclone = new Material(material);

            // Debug.Log("[Viewer]" + viewerPosition.x +" , " + viewerPosition.y);
            // Debug.Log(coordinateX +" , "+ coordinateY);

            Matrix4x4[] matrices = new Matrix4x4[1023];
            for(float i = 0, y = coordinateY - visibleRange; y <= coordinateY + visibleRange - diastem; y+=diastem){
                for(float x = coordinateX - visibleRange; x <= coordinateX + visibleRange - diastem; x+=diastem){
                    // float height = heightMap[x,y];
                    float height = BilinearInterpolation(new Vector2(x,y),heightMap);
                    // Debug.Log(x +" , "+ y);
                    if(height>0.35 && height<0.6){
                        float positionX = Mathf.FloorToInt(x * 1000)/1000.0f;
                        float positionY = Mathf.FloorToInt(y * 1000)/1000.0f;
                        Vector2 positionKey = new Vector2(positionX,positionY);
                        position = new Vector3(positionX,EndlessTerrain.mapGenerator.meshHeightCurve.Evaluate(height)*EndlessTerrain.mapGenerator.meshHeightMultiplier,positionY) * EndlessTerrain.scale;
                        // if(i<10)Debug.Log(i+": "+position);
                        if(positionGrassDictionary.ContainsKey(positionKey)){
                            matrices[(int)i] = positionGrassDictionary[positionKey];
                        }else{
                            rotation = Quaternion.Euler(Random.Range(-20,20),Random.Range(0,359),0);
                            // scale = Vector3.one;
                            scale = new Vector3(0.6f,0.5f,0.6f);
                            // scale = new Vector3(0.2f,0.1f,0.2f);
                            matrices[(int)i] = Matrix4x4.TRS(position+new Vector3(Random.Range(-1.0f,1.0f),0,Random.Range(-1.0f,1.0f)),rotation,scale);
                            positionGrassDictionary[positionKey] = matrices[(int)i];
                        }
                        i++;
                        if(i >= 1023){
                            i = 0;
                            matricesList.Add(matrices);
                            matrices = new Matrix4x4[1023];
                        }
                    }
                }
            }
            matricesList.Add(matrices);
        }
    }

    ////Only can see the grass in one chunk only
    ////Can not see grass across the chunk
    // public void UpdateVisibleGrass(float[,] heightMap, Vector2 viewerPosition){
    //     int coordinateX = Mathf.RoundToInt(viewerPosition.x+ MapGenerator.mapChunkSize/2);
    //     int coordinateY = Mathf.RoundToInt(MapGenerator.mapChunkSize/2 - viewerPosition.y);
    //     if(new Vector2(coordinateX,coordinateY) != previousCoordinate){
    //         matricesList.Clear();
    //         float diastem = Mathf.RoundToInt(1.0f/density*1000)/1000.0f;
    //         int visibleRange = 8;
    //         previousCoordinate = new Vector2(coordinateX,coordinateY);
    //         matclone = new Material(material);

    //         // Debug.Log("[Viewer]" + viewerPosition.x +" , " + viewerPosition.y);
    //         // Debug.Log(coordinateX +" , "+ coordinateY);

    //         // Matrix4x4[] matrices = new Matrix4x4[Mathf.CeilToInt(Mathf.Pow(visibleRange*2+1+(density-1)*(visibleRange*2+1),2))];
    //         Matrix4x4[] matrices = new Matrix4x4[1023];
    //         for(float i = 0, y = coordinateY - visibleRange; y <= coordinateY + visibleRange - diastem; y+=diastem){
    //             for(float x = coordinateX - visibleRange; x <= coordinateX + visibleRange - diastem; x+=diastem){
    //                 // float height = heightMap[x,y];
    //                 float height = BilinearInterpolation(new Vector2(x,y),heightMap);
    //                 // Debug.Log(x +" , "+ y);
    //                 if(height>0.35 && height<0.6){
    //                     float positionX = Mathf.FloorToInt((x - MapGenerator.mapChunkSize/2)*1000)/1000.0f;
    //                     float positionY = Mathf.FloorToInt((MapGenerator.mapChunkSize/2 - y)*1000)/1000.0f;
    //                     Vector2 positionKey = new Vector2(positionX,positionY);
    //                     position = new Vector3(positionX,EndlessTerrain.mapGenerator.meshHeightCurve.Evaluate(height)*EndlessTerrain.mapGenerator.meshHeightMultiplier,positionY) * EndlessTerrain.scale;
    //                     // if(i<10)Debug.Log(i+": "+position);
    //                     if(positionGrassDictionary.ContainsKey(positionKey)){
    //                         matrices[(int)i] = positionGrassDictionary[positionKey];
    //                     }else{
    //                         rotation = Quaternion.Euler(Random.Range(-20,20),Random.Range(0,359),0);
    //                         // scale = Vector3.one;
    //                         scale = new Vector3(0.6f,0.5f,0.6f);
    //                         // scale = new Vector3(0.2f,0.1f,0.2f);
    //                         matrices[(int)i] = Matrix4x4.TRS(position+new Vector3(Random.Range(-1.0f,1.0f),0,Random.Range(-1.0f,1.0f)),rotation,scale);
    //                         positionGrassDictionary[positionKey] = matrices[(int)i];
    //                     }
    //                     i++;
    //                     if(i >= 1023){
    //                         i = 0;
    //                         matricesList.Add(matrices);
    //                         matrices = new Matrix4x4[1023];
    //                     }
    //                 }
    //             }
    //         }
    //         matricesList.Add(matrices);
    //     }
    // }

    public void GenerateGrass(float[,] heightMap, Vector2 chunkCoordinate, Bounds bounds){
        int size = MapGenerator.mapChunkSize;
        int unitSize = 19;
        int grassChunkSize = size/unitSize; //Matrix4x4 have limited size
        // matricesList.Clear();
        matclone = new Material(material);
        for(int count = 0; count < unitSize*unitSize; count++){
            Matrix4x4[] matrices = new Matrix4x4[Mathf.CeilToInt(Mathf.Pow(grassChunkSize+(density-1)*grassChunkSize,2))];
            for (float i = 0, y = 0; y < grassChunkSize-1.0f/density; y+=1.0f/density){
                for(float x = 0; x < grassChunkSize-1.0f/density; x +=1.0f/density){
                    // if(count == 0)Debug.Log("x: " + x + "y: " + y);
                    float heightMapPositionX = x + count%unitSize*grassChunkSize;
                    float heightMapPositionY = y + count/unitSize*grassChunkSize;
                    // Debug.Log("Height Map x: " + heightMapPositionX + "Height Map y: " + heightMapPositionY);
                    if(heightMapPositionX >= size || heightMapPositionY >= size){
                        continue;
                    }
                    float height = BilinearInterpolation(new Vector2(heightMapPositionX,heightMapPositionY),heightMap);
                    if(height>0.35 && height<0.6){
                        float positionX = x-MapGenerator.mapChunkSize/2+count%unitSize*grassChunkSize + MapGenerator.mapChunkSize*chunkCoordinate.x + Random.Range(-1.0f,1.0f);
                        float positionY = MapGenerator.mapChunkSize/2-y-count/unitSize*grassChunkSize + MapGenerator.mapChunkSize*chunkCoordinate.y + Random.Range(-1.0f,1.0f);
                        position = new Vector3(positionX,EndlessTerrain.mapGenerator.meshHeightCurve.Evaluate(height)*EndlessTerrain.mapGenerator.meshHeightMultiplier,positionY) * EndlessTerrain.scale;
                        rotation = Quaternion.Euler(Random.Range(-20,20),Random.Range(0,359),0);
                        scale = Vector3.one;
                        // if(count == 0)Debug.Log(matrices.Length +" "+i);
                        matrices[(int)i] = Matrix4x4.TRS(position,rotation,scale);
                        i++;
                    }
                }
            }
            matricesList.Add(matrices);
        }
    }

float BilinearInterpolation(Vector2 position, float[,] heightMap){
        float chunkSize = MapGenerator.mapChunkSize-1;
        int terrainCoordinateX = Mathf.RoundToInt(position.x/chunkSize);
        int terrainCoordinateY = Mathf.RoundToInt(position.y/chunkSize);
        heightMap = terrainChunkDictionary[new Vector2(terrainCoordinateX,terrainCoordinateY)].mapData.heightMap;
        // Debug.Log("chunk coordinate: " + terrainCoordinateX+","+terrainCoordinateY);
        float x = Mathf.Abs((position.x+chunkSize/2)%chunkSize+((chunkSize/2+position.x)<0?95:0));
        float y = Mathf.Abs((chunkSize/2-position.y)%chunkSize+((chunkSize/2-position.y)<0?95:0));
        int x_1 = Mathf.FloorToInt(x) + 1;
        int y_1 = Mathf.FloorToInt(y) + 1;
        int x_0 = Mathf.FloorToInt(x);
        int y_0 = Mathf.FloorToInt(y);
        // Debug.Log("x: " + x + " y: " + y);
        if(heightMap == null){
            return -1;
        }

        if(x_1 >= heightMap.GetLength(0) || y_1 >= heightMap.GetLength(1)){
            return heightMap[x_0,y_0];
        }

        float f_x_y0 = (x_1-x)/(x_1-x_0)*heightMap[x_0,y_0] + (x-x_0)/(x_1-x_0)*heightMap[x_1,y_0];
        float f_x_y1 = (x_1-x)/(x_1-x_0)*heightMap[x_0,y_1] + (x-x_0)/(x_1-x_0)*heightMap[x_1,y_1];

        return (y_1-y)/(y_1-y_0)*f_x_y0  + (y-y_0)/(y_1-y_0)*f_x_y1;
    }

    void Update(){
        if(matclone != null){
            foreach(Matrix4x4[] matrix4X4 in matricesList){
                Graphics.DrawMeshInstanced(mesh, 0, matclone, matrix4X4,matrix4X4.Length,null,ShadowCastingMode.Off);
            }   
        }
    }
}