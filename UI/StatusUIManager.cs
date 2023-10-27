using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FYP
{
    public class StatusUIManager : MonoBehaviour
    {
        public Transform moneyTextUI;
        public Transform stateBox;

        TextMeshProUGUI moneyUITMPro;
        StatBoxScript statBoxScript;

        // Update is called once per frame
        public void UpdateText()
        {
            if(moneyUITMPro == null || statBoxScript == null){
                moneyUITMPro = moneyTextUI.GetComponent<TextMeshProUGUI>();
                statBoxScript = stateBox.GetComponent<StatBoxScript>();
            }
            moneyUITMPro.SetText(UIController.playerData.GetMoneyAmount());
            statBoxScript.UpdateText();
        }
    }
}