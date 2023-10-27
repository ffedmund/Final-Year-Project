using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP
{
    [CreateAssetMenu(menuName = "Items/Material Item")]
    public class MaterialItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;
    }
}