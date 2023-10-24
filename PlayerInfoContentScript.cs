using System.Collections;
using System.Collections.Generic;
using FYP;
using TMPro;
using UnityEngine;

public class PlayerInfoContentScript : MonoBehaviour
{
    public Transform leftFrameTransform;
    public Transform rightFrameTransform;
    public Transform leftTextBox;
    public Transform rightTextBox;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    
    public void ShowBackgroundInfo(){
        int playerBackgroundId = FindObjectOfType<PlayerStats>().playerData.GetAttribute("backgroundId");
        PlayerBackground playerBackground = DataReader.backgorundDictionary[playerBackgroundId];
        Debug.Log(playerBackground.description);
        leftText.SetText("Background\n"+playerBackground.description);
        rightText.SetText("Skill");
    }

    public void ShowTalentInfo(){

    }
}
