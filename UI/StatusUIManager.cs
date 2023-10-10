using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUIManager : MonoBehaviour
{
    public Transform moneyTextUI;
    public Transform stateBox;

    TextMeshProUGUI moneyUITMPro;
    StatBoxScript statBoxScript;

    private void Awake() {
        moneyUITMPro = moneyTextUI.GetComponent<TextMeshProUGUI>();
        statBoxScript = stateBox.GetComponent<StatBoxScript>();
    }

    // Update is called once per frame
    public void UpdateText()
    {
        moneyUITMPro.SetText(UIController.playerData.GetMoneyAmount());
        statBoxScript.UpdateText();
    }
}
