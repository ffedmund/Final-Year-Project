using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FYP
{
    public class WeaponPickUp : InteractableScript
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            // playerLocomotion.rigidbody.angularVelocity = Vector3.zero;

            animatorHandler.PlayTargetAnimation("Pick Up Item", true);

            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemInteractableGameObject.SetActive(true);

            foreach(Quest quest in playerManager.playerData.quests){
                quest.goalChecker.ItemCollected(weapon.itemName);
            }

            Destroy(gameObject);
        }
    }
}