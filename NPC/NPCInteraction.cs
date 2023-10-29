using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYP;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : InteractableScript
{
    public TextMeshProUGUI chatText;
    public NPC npc;

    Button closeButton;
    QuestReceiver questReceiver;

    public void Setup(NPC npc){
        this.npc = npc;
    }

    void Start() {
        canvasUIController = FindAnyObjectByType<UIController>();
        TryGetComponent(out questReceiver);
        targetUIWindow = canvasUIController.chatBoxWindow;
        chatText = targetUIWindow.transform.Find("ChatBox").GetComponentInChildren<TextMeshProUGUI>();
        closeButton = targetUIWindow.transform.GetComponentInChildren<Button>();
        isUITrigger = true;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        if(questReceiver)
        {
            Quest quest = playerManager.playerData.quests.Find(quest => quest.goalChecker.isReached()&&questReceiver.questIdList.Contains(quest.id));
            if(quest != null)
            {
                questReceiver.ReportQuest(quest);
                SetChatContent(quest.completeDialog);
            }
            else
            {
                SetChatContent();
            }
        }
        else
        {
            SetChatContent();
        }
        GetComponent<Collider>().enabled = false;
    }

    public void Init(){
        HideChatContent();
    }

    protected void HideChatContent(){
        canvasUIController.activeUIWindows.Remove(targetUIWindow);
        targetUIWindow.SetActive(false);
        chatText.SetText("");
        GetComponent<Collider>().enabled = true;
        closeButton.onClick.RemoveAllListeners();
    }

    protected void SetChatContent(string specificText){
        chatText.SetText($"<line-height=150%><size=120%>{npc.name}</size>\n{specificText}");
        closeButton.onClick.AddListener(()=>{HideChatContent();});
    }

    protected void SetChatContent(){
        if(npc.dialogues.Length > 0){
            chatText.SetText($"<line-height=150%><size=120%>{npc.name}</size>\n{npc.dialogues[Random.Range(0,npc.dialogues.Length)]}");
        }
        closeButton.onClick.AddListener(()=>{HideChatContent();});
    }
}