using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public Transform targetTransform;
    public bool isUITrigger;
    public bool isInteracting;
    UIController canvasUIController;
    public EventTrigger.TriggerEvent customCallback;

    void Start(){
        canvasUIController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    void Update(){
        if(isInteracting){
            if(Input.GetKeyDown(KeyCode.F)){
                // BaseEventData eventData= new BaseEventData(EventSystem.current);
                // eventData.selectedObject=this.gameObject;
                // customCallback.Invoke(eventData);
            }
        }
    }

    void OnTriggerStay(Collider other) {
        print(other);
        if(other.tag == "Player"){
            canvasUIController.interactionTipUI.gameObject.SetActive(true);
            canvasUIController.currentInteractObject = transform;
            isInteracting = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            canvasUIController.interactionTipUI.gameObject.SetActive(false);
            if(canvasUIController.currentInteractObject = transform){
                canvasUIController.currentInteractObject = null;
                isInteracting = false;
            }
        }
    }
}
