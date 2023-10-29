using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP
{   
    [CreateAssetMenu(menuName = "NPC/New NPC")]
    public class NPC : ScriptableObject
    {
        [Header("NPC Information")]
        public string id;
        public string npcName;
        public string[] dialogues;
    }
}

