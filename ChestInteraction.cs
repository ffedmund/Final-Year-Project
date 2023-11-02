using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYP;
using DG.Tweening;

public class ChestInteraction : InteractableScript
{
    readonly string aniamtionClipName = "Fantasy_Polygon_Chest_Animation_Open";

    public int range;
    public int awardNumber;
    public GameObject itemPrefab;
    public WeaponItem[] awardWeapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        StartCoroutine(OpenChest());
    }

    IEnumerator OpenChest()
    {
        GetComponent<Collider>().enabled = false;
        if(TryGetComponent(out Animator chestAnimator))
        {
            AnimationClip clip = null;
            foreach(AnimationClip animationClip in chestAnimator.runtimeAnimatorController.animationClips){
                if(animationClip.name == aniamtionClipName){
                    clip = animationClip;
                    break;
                }
            }
            chestAnimator.Play(aniamtionClipName);
            yield return new WaitForSeconds(clip.length-0.7f);
        }

        //Action after animation
        System.Random random = new System.Random(0);
        for(int i = 0; i < awardNumber; i++)
        {
            float itemPositionX = transform.position.x+random.Next(-range,range);
            float itemPositionZ = transform.position.z+random.Next(-range,range);
            Vector3 itemPosition = new Vector3(itemPositionX,transform.position.y,itemPositionZ);
            print(awardWeapon.Length);
            WeaponItem weaponItem = (WeaponItem)awardWeapon[random.Next(0,awardWeapon.Length)];
            GameObject item = Instantiate(itemPrefab,transform.position,Quaternion.identity);
            if(item.TryGetComponent(out WeaponPickUp weaponPickUp))
            {
                WeaponItem weapon = Instantiate(weaponItem);
                weapon.name = weaponItem.itemName;
                weaponPickUp.weapon = weapon;
            }
            item.transform.DOJump(itemPosition, 2, 1, 1);
        }
    }
}
