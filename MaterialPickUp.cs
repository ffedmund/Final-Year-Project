using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FYP
{
    public class MaterialPickUp : InteractableScript
    {
        public MaterialItem material;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        protected void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;

            animatorHandler.PlayTargetAnimation("Pick Up Item", true);

            playerInventory.materialsInventory.Add(material);
            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = material.itemName;
            playerManager.itemInteractableGameObject.SetActive(true);

            foreach(Quest quest in playerManager.playerData.quests){
                quest.goalChecker.ItemCollected(material.itemName);
            }

            Destroy(gameObject);
        }
    }
}
