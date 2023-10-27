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
        public bool isUITrigger;
        public UIController canvasUIController;
        public EventTrigger.TriggerEvent customCallback;

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        public virtual void Interact(PlayerManager playerManager) {
            if(customCallback != null){
                BaseEventData eventData = new BaseEventData(EventSystem.current);
                eventData.selectedObject = this.gameObject;
                customCallback.Invoke(eventData);
            }
            if(isUITrigger && targetTransform.TryGetComponent(out RectTransform rectTransform)){
                canvasUIController.activeUIWindows.Add(targetTransform);
                targetTransform.gameObject.SetActive(true);
            }
            Debug.Log("Interacting");
        }
    }
}