
using TMPro;
using UnityEngine;

public class Notice : MonoBehaviour {
    public SpriteRenderer icon;
    public TextMeshPro text;

    [Header("MiniMap")]
    public bool showInMinimap;
    public SpriteRenderer minimapIcon;

    public Sprite[] iconArray;

    public void UpdateNoticeDirection(Vector3 direction){
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }

    public void SetNotice(Sprite image, string textContent = ""){
        
        icon.sprite = image;
        if(showInMinimap){
            minimapIcon.sprite = image;
        }
        text.SetText(textContent);
    }

    public void SetNotice(int iconIndex, string textContent = ""){
        
        icon.sprite = iconArray[iconIndex];
        if(showInMinimap){
            minimapIcon.sprite = iconArray[iconIndex];
        }
        text.SetText(textContent);
    }

    public void ClearNotice(){
        icon.sprite = null;
        if(showInMinimap){
            minimapIcon.sprite = null;
        }
        text.SetText("");
    }
}