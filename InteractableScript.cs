using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using System;
using DG.Tweening;

namespace FYP
{
    public class InteractableScript : MonoBehaviour
    {
        public float radius = 0.6f;
        public string interactableText;

        public GameObject targetUIWindow;
        public bool isUITrigger;
        public UIController canvasUIController;
        public EventTrigger.TriggerEvent customCallback;

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private void OnDestroy() {
            if(DOTween.IsTweening(transform.gameObject)){
                DOTween.Kill(transform.gameObject);
            }
        }

        public virtual void Interact(PlayerManager playerManager) {
            if(customCallback != null){
                BaseEventData eventData = new BaseEventData(EventSystem.current);
                eventData.selectedObject = this.gameObject;
                customCallback.Invoke(eventData);
                // customCallback.Invoke(playerManager);
            }
            if(isUITrigger && targetUIWindow.TryGetComponent(out RectTransform rectTransform)){
                canvasUIController.activeUIWindows.Add(targetUIWindow);
                targetUIWindow.SetActive(true);
            }
        }
    }
}