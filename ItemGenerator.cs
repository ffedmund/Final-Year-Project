using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FYP;
using DG.Tweening;
using UnityEngine.Animations;

public class ItemGenerator : MonoBehaviour
{
    public enum ItemInitMovement{
        None,
        GoUp,
        FallDown,
        MoveFront,
        MoveBack,
        MoveLeft,
        MoveRight
    }

    [Header("Generate Setting")]
    public float generateCooldown;
    public bool isAutoGenerate;
    public bool isItemDealDamage;
    public bool isShaking;
    public int itemStorageAmount;
    [Header("Item Setting")] 
    public int extraGenerateRange;
    public Item item;
    public GameObject itemPrefab;
    public ItemInitMovement itemInitMovement;
    public float extraMovingDistance;
    public List<MonoBehaviour> itemMonoBehaviours;

    int rangeX;
    int rangeZ;
    int rangeY;
    int remainItemStorage;
    float previousGenerateTime;
    System.Random random;

    void Start(){
        #region Random Item Generate Position Setting
            random = new System.Random(0);
            rangeX = extraGenerateRange;
            rangeZ = extraGenerateRange;
            rangeY = 0;
            previousGenerateTime = 0;
            if(transform.parent.TryGetComponent(out Renderer renderer)){
                rangeX += (int)renderer.bounds.size.x/2;
                rangeZ += (int)renderer.bounds.size.z/2;
                rangeY += (int)renderer.bounds.size.y/2;
            }
        #endregion
        remainItemStorage = 0;
        if(isAutoGenerate){
            previousGenerateTime = Time.time - generateCooldown - Time.fixedDeltaTime;
            Generate();
            if(generateCooldown <= 0){
                generateCooldown = Time.fixedDeltaTime;
            }
        }
    }

    public void Generate(){
        Debug.Log("Generate "+item.name);
        if(isShaking){
            transform.parent.DOShakePosition(1, new Vector3(0.08f, 0f, 0.08f));
        }
        if(generateCooldown == 0 || Time.time-previousGenerateTime >= generateCooldown){
            float itemPositionX = 0;
            float itemPositionZ = 0;
            do{
                itemPositionX = transform.position.x+random.Next(-rangeX,rangeX+1);
                itemPositionZ = transform.position.z+random.Next(-rangeZ,rangeZ+1);
            }while(itemPositionX == itemPositionZ && itemPositionX == 0);
            
            Vector3 itemPosition = new Vector3(itemPositionX,transform.position.y+rangeY,itemPositionZ);

            GameObject itemObjectPrefab = itemPrefab;
            if(item is WeaponItem){
                itemObjectPrefab = ((WeaponItem)this.item).modelPrefab;
            }else if(item is MaterialItem){
                itemObjectPrefab = ((MaterialItem)this.item).modelPrefab;
            }
            GameObject itemObject = Instantiate(itemObjectPrefab,itemPosition,Quaternion.identity);
            itemObject.tag = "Interactable";
            Item newItem = Instantiate(item);
            newItem.name = item.name;

            if(!itemObject.TryGetComponent(out Collider collider)){
                SphereCollider sphereCollider = itemObject.AddComponent<SphereCollider>();
                sphereCollider.radius = 1;
                sphereCollider.isTrigger = true;
            }
            
            if(item is WeaponItem){
                itemObject.AddComponent<WeaponPickUp>().weapon = (WeaponItem)newItem;
                itemObject.GetComponent<WeaponPickUp>().interactableText = item.name;
                if(isItemDealDamage){
                    itemObject.transform.GetComponentInChildren<DamageCollider>().EnableDamageCollider();
                }
            }else{
                itemObject.AddComponent<MaterialPickUp>().material = (MaterialItem)newItem;
                itemObject.GetComponent<MaterialPickUp>().interactableText = item.name;
                if(isItemDealDamage){
                    itemObject.AddComponent<DamageCollider>().EnableDamageCollider();
                }
            }

            Vector3 movingDestination = new Vector3();

            switch(itemInitMovement){
                case ItemInitMovement.GoUp:
                    movingDestination = new Vector3(itemPositionX,itemPosition.y+extraGenerateRange,itemPositionZ);
                    break;
                case ItemInitMovement.FallDown:
                    movingDestination = new Vector3(itemPositionX,transform.position.y,itemPositionZ);
                    break;
                case ItemInitMovement.MoveRight:
                    movingDestination = new Vector3(itemPositionX+extraMovingDistance,itemPosition.y,itemPositionZ);
                    break;
                case ItemInitMovement.MoveLeft:
                    movingDestination = new Vector3(itemPositionX-extraMovingDistance,itemPosition.y,itemPositionZ);
                    break;
                case ItemInitMovement.MoveFront:
                    movingDestination = new Vector3(itemPositionX,itemPosition.y,itemPositionZ+extraMovingDistance);
                    break;
                case ItemInitMovement.MoveBack:
                    movingDestination = new Vector3(itemPositionX,itemPosition.y,itemPositionZ-extraMovingDistance);
                    break;
                default:
                    break;
            }
            itemObject.transform.DOMove(movingDestination,0.5f);
            previousGenerateTime = Time.time;
            if(isAutoGenerate){
                Invoke("Generate",generateCooldown+Time.fixedDeltaTime);
            }
        }
    }

}