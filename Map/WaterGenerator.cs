using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterGenerator: MonoBehaviour{

    [Range(1,100)]
    public float density;
    public Material waterMaterial;
    [SerializeField]
    float waterShowingDistance;
    public LayerMask layerMask;


    float chunkSize;
    float waterUnitSize;
    Vector2Int previousViewerCoordinate;

    Dictionary<Vector2,GameObject> waterCoordinateDict = new Dictionary<Vector2, GameObject>();
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
                    waterCoordinateDict[waterCoordinate].SetActive(true);
                    lastFrameVisibleWater.Add(waterCoordinateDict[waterCoordinate]);
                }else{
                    waterCoordinateDict[waterCoordinate] = CreateWaterPlane(waterUnitSize, waterCoordinate);
                }
            }
        }

    }

    public GameObject CreateWaterPlane(float scale,Vector2Int waterCoodinate){
        Vector3 waterPlanePosition = new Vector3(waterCoodinate.x+0.1f,0,waterCoodinate.y+0.1f)*scale*10;
        if(Physics.Raycast(waterPlanePosition, Vector3.up ,out RaycastHit hit, 100f, layerMask)){
            if(hit.collider != null){
                return null;
            }
        }
        GameObject waterMap = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterMap.name = "Water Plane["+waterCoodinate.ToString()+"]";
        waterMap.transform.localScale = new Vector3(scale,1,scale);
        waterMap.transform.position = waterPlanePosition;
        waterMap.GetComponent<MeshRenderer>().material = waterMaterial;
        waterMap.transform.SetParent(transform);
        waterMap.SetActive(false);
        return waterMap;
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