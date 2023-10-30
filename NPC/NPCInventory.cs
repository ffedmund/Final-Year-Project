using UnityEngine;
using FYP;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class NPCInventory:MonoBehaviour{
    NPC npc;
    public List<Item> npcInventory;
    public bool saveInventory;
    
    public void Setup(NPC npc){
        this.npc = npc;
        if(saveInventory){
            npcInventory = npc.npcInventory;
        }else{
            foreach(var item in npc.npcInventory){
                var itemCopy = Instantiate(item);
                itemCopy.name = item.name;
                this.npcInventory.Add(itemCopy);

            }
        }
    }

}