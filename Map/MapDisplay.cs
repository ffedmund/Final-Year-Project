using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
   public Renderer textureRender;
   public MeshFilter meshFilter;
   public MeshRenderer meshRenderer;
   public Transform meshTransform;

   public GameObject[] treePrefabs;

     //Create Texture(Color)
     public void DrawTexture(Texture2D texture){
          textureRender.sharedMaterial.mainTexture = texture;
          textureRender.transform.localScale = new Vector3(texture.width,1,texture.height);
     }

     //Create Mesh(Shape)
     public void DrawMesh(MeshData meshData, Texture2D texture2D){
          meshFilter.sharedMesh = meshData.CreateMesh();
          meshRenderer.sharedMaterial.mainTexture = texture2D;
          if(TryGetComponent(out MapGenerator mapGenerator)){
               meshTransform.localScale = mapGenerator.useEndlessTerrainScale?Vector3.one*EndlessTerrain.scale:meshTransform.localScale;
          }
          // WaterGenerator.CreateWaterMap(Vector3.zero,waterMaterial);
     }

     public void DrawTree(bool[,] treeMap, float[,] heightMap){
          foreach (Transform transform in meshTransform)
          {
               DestroyImmediate(transform.gameObject);
          }
          GetComponent<TreeGenerator>().CreateTrees(meshTransform,treeMap,Vector3.zero,heightMap);
     }
}
