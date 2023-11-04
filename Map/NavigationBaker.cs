using Unity.AI.Navigation;
using UnityEngine;

public class NavigationBaker : MonoBehaviour{
    static NavMeshSurface navMeshSurface;

    public void Awake(){
        navMeshSurface = FindAnyObjectByType<NavMeshSurface>();
    }

    public static void Bake(){
        if(navMeshSurface)navMeshSurface.BuildNavMesh();
    }
}