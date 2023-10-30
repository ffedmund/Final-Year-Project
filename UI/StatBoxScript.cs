using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FYP
{
    public class StatBoxScript : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI[] stateBoxes;
        [SerializeField]
        TextMeshProUGUI levelText;

        public void UpdateText()
        {   
            levelText.SetText($"Level   {UIController.playerData.GetAttribute("level").ToString()}");
            string[] playerAttributesArray = UIController.playerData.ToStringArray();
            for (int i = 0; i < playerAttributesArray.Length; i++)
            {
                stateBoxes[i].SetText(playerAttributesArray[i]);
            }
        }

    }
}