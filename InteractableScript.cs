using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using System;

namespace FYP
{
    public class InteractableScript : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        public Transform targetTransform;
        public bool useRaycast;
        public bool isUITrigger;
        public bool isInteracting;
        UIController canvasUIController;
        public EventTrigger.TriggerEvent customCallback;

        void Start()
        {
            canvasUIController = GameObject.Find("PlayerUI").GetComponent<UIController>();
        }

        void Update()
        {
            // if (isInteracting && !isUITrigger)
            // {
            //     if (Input.GetKeyDown(KeyCode.F))
            //     {
            //         Interact();
            //     }
            // }
            if (useRaycast)
            {
                Vector3 forward = Vector3.forward + new Vector3(0, 0.5f, 0);
                Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), forward, out RaycastHit hit, 3f);
                Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), forward, hit.collider == null ? Color.red : Color.green);
            }
        }

        // void Interact()
        // {
        //     BaseEventData eventData = new BaseEventData(EventSystem.current);
        //     eventData.selectedObject = this.gameObject;
        //     customCallback.Invoke(eventData);
        // }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager) {
            Debug.Log("Interacting");
        }

        // void OnTriggerStay(Collider other)
        // {
        //     if (other.tag == "Player")
        //     {
        //         Debug.Log("Trigger Stay");
        //         canvasUIController.interactionTipUI.gameObject.SetActive(true);
        //         if (isUITrigger)
        //         {
        //             canvasUIController.currentInteractObject = transform;
        //         }
        //         isInteracting = true;
        //     }
        // }

        // void OnTriggerExit(Collider other)
        // {
        //     if (other.tag == "Player")
        //     {
        //         canvasUIController.interactionTipUI.gameObject.SetActive(false);
        //         if (isUITrigger && canvasUIController.currentInteractObject == transform)
        //         {
        //             canvasUIController.currentInteractObject = null;
        //         }
        //         isInteracting = false;
        //     }
        // }
    }
}