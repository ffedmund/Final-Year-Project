using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterGenerator: MonoBehaviour{

    [Range(1,100)]
    public float density;
    [SerializeField]
    float waterShowingDistance;
    public LayerMask layerMask;
    public Transform waterPlane;

    float chunkSize;
    float waterUnitSize;
    Vector2Int previousViewerCoordinate;

    Dictionary<Vector2,Dictionary<Vector2,GameObject>> waterCoordinateDict = new Dictionary<Vector2, Dictionary<Vector2,GameObject>>();
    List<GameObject> lastFrameVisibleWater = new List<GameObject>();
    
    void Awake(){
        chunkSize = MapGenerator.mapChunkSize - 1;
        waterUnitSize = chunkSize/density;
    }

    public void CreateWaterMap(Vector2 viewerPosition){
        //Calculate the viewer position in chunk coordinate

        int viewerCoordinateX = Mathf.RoundToInt(viewerPosition.x/waterUnitSize);
        int viewerCoordinateY = Mathf.RoundToInt(viewerPosition.y/waterUnitSize);
        Vector2Int viewerCoordinate = new Vector2Int(viewerCoordinateX,viewerCoordinateY);
        if(viewerCoordinate == previousViewerCoordinate){
            return;
        }

        previousViewerCoordinate = viewerCoordinate;
        int waterVisibleInViewDist = Mathf.RoundToInt(waterShowingDistance/waterUnitSize);

        //Debug Log
        // Debug.Log("Water Visible: " + waterVisibleInViewDist);
        // Debug.Log("View Position in Water Map: " + viewerCoordinate);
        
        for(int i = 0; i < lastFrameVisibleWater.Count; i++){
            lastFrameVisibleWater[i].SetActive(false);
        }

        lastFrameVisibleWater.Clear();

        for(int y = -waterVisibleInViewDist; y <= waterVisibleInViewDist; y++){
            for(int x = -waterVisibleInViewDist; x <= waterVisibleInViewDist; x++){
                Vector2Int waterCoordinate = new Vector2Int(viewerCoordinate.x+x,viewerCoordinate.y+y);
                if(waterCoordinateDict.ContainsKey(waterCoordinate) && waterCoordinateDict[waterCoordinate] != null){
                    // waterCoordinateDict[waterCoordinate].SetActive(true);
                    // lastFrameVisibleWater.Add(waterCoordinateDict[waterCoordinate]);
                }else{
                    // waterCoordinateDict[waterCoordinate] = CreateWaterPlane(waterUnitSize, waterCoordinate);
                }
            }
        }

    }

    public void CreateWater(int[,] waterMap,Vector2 chunkCoordinate){
        int size = waterMap.GetLength(0);
        int chunkSize = MapGenerator.mapChunkSize - 1;

        if(!waterCoordinateDict.ContainsKey(chunkCoordinate)){
            waterCoordinateDict[chunkCoordinate] = new Dictionary<Vector2, GameObject>();
            for(int y = 0; y < size; y++){
                for(int x = 0; x < size; x++){
                    if(waterMap[x,y] == 1){
                        Vector3 position = new Vector3((chunkCoordinate.x*chunkSize+x-size/2-0.1f)*EndlessTerrain.scale,-0.5f,(chunkCoordinate.y*chunkSize+size/2-y-0.1f)*EndlessTerrain.scale);
                        GameObject waterPlaneObject = Instantiate(waterPlane.gameObject,position,Quaternion.identity);
                        waterPlaneObject.name = String.Format("Chunk[{0},{1}] Water({2},{3})",chunkCoordinate.x,chunkCoordinate.y,x,y);
                        waterPlaneObject.transform.SetParent(this.transform);
                        waterPlaneObject.transform.localScale = new Vector3(1,10/EndlessTerrain.scale,1)*EndlessTerrain.scale/10;
                        waterCoordinateDict[chunkCoordinate][new Vector2(x,y)] = waterPlaneObject;
                        // waterPlaneObject.SetActive(false);
                    }else{
                        waterCoordinateDict[chunkCoordinate][new Vector2(x,y)] = null;
                    }
                }
            }
        }
    }


    public void ClearWaterMap(){
        while(transform.childCount > 0){
            foreach(Transform child in transform){
                DestroyImmediate(child.gameObject);
            }
        }
        lastFrameVisibleWater.Clear();
    }
}