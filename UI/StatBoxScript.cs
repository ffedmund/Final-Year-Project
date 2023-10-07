using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatBoxScript : MonoBehaviour
{
    [SerializeField]
    Transform[] stateBoxes;
    PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerData = new PlayerData();
        string[] playerAttributesArray = playerData.ToStringArray();
        for(int i = 0; i < playerAttributesArray.Length; i++){
            stateBoxes[i].GetComponent<TextMeshProUGUI>().SetText(playerAttributesArray[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
