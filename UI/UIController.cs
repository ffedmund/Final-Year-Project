using UnityEngine;

public class UIController : MonoBehaviour {

    public Transform stateUI;
    public static PlayerData playerData;

    void Start(){
        playerData = new PlayerData();
    }

    [System.Obsolete]
    private void Update() {
        if(Input.GetKeyDown(KeyCode.U)){
            stateUI.gameObject.SetActive(!stateUI.gameObject.active);
            stateUI.GetComponent<StatusUIManager>().UpdateText();
            DataReader.ReadDataBase();
        }
    }
}