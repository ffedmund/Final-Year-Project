using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using System;

public class InteractableScript : MonoBehaviour
{
    public Transform targetTransform;
    public bool useRaycast;
    public bool isUITrigger;
    public bool isInteracting;
    UIController canvasUIController;
    public EventTrigger.TriggerEvent customCallback;

    void Start(){
        canvasUIController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    void Update(){
        if(isInteracting && !isUITrigger){
            if(Input.GetKeyDown(KeyCode.F)){
                Interact();
            }
        }
        if(useRaycast){
            Vector3 forward = Vector3.forward + new Vector3(0,0.5f,0);
            Physics.Raycast(transform.position + new Vector3(0,0.5f,0),forward,out RaycastHit hit,3f);
            Debug.DrawRay(transform.position + new Vector3(0,0.5f,0), forward, hit.collider == null?Color.red :Color.green);
        }
    }

    void Interact(){
        BaseEventData eventData= new BaseEventData(EventSystem.current);
        eventData.selectedObject=this.gameObject;
        customCallback.Invoke(eventData);
    }

    void OnTriggerStay(Collider other) {
        if(other.tag == "Player"){
            canvasUIController.interactionTipUI.gameObject.SetActive(true);
            if(isUITrigger){
                canvasUIController.currentInteractObject = transform;
            }
            isInteracting = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            canvasUIController.interactionTipUI.gameObject.SetActive(false);
            if(isUITrigger && canvasUIController.currentInteractObject == transform){   
                canvasUIController.currentInteractObject = null;
            }
            isInteracting = false;
        }
    }
}
