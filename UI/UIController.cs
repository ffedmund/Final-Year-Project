using UnityEngine;

public class UIController : MonoBehaviour {

    public Transform stateUI;

    [System.Obsolete]
    private void Update() {
        if(Input.GetKeyDown(KeyCode.U)){
            stateUI.gameObject.SetActive(!stateUI.gameObject.active);
        }
    }
}