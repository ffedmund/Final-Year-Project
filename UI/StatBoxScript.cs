using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FYP
{
    public class StatBoxScript : MonoBehaviour
    {
        [SerializeField]
        Transform[] stateBoxes;

        public void UpdateText()
        {
            string[] playerAttributesArray = UIController.playerData.ToStringArray();
            for (int i = 0; i < playerAttributesArray.Length; i++)
            {
                stateBoxes[i].GetComponent<TextMeshProUGUI>().SetText(playerAttributesArray[i]);
            }
        }

    }
}