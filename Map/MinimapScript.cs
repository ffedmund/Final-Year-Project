using UnityEngine;

public class MinimapScript : MonoBehaviour {
    public Transform followPlayer;

    void Update(){
        transform.position = new Vector3(followPlayer.position.x,20*EndlessTerrain.scale,followPlayer.position.z);
    }
}