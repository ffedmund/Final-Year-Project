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
    bool isTradable;

    public void Setup(NPC npc){
        this.npc = npc;
        isTradable = npc.isTradable;
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
        SetTradeButton(playerManager);
        GetComponent<Collider>().enabled = false;
    }

    public void Init(){
        HideChatContent();
        ResetTradeButton();
    }

    protected void HideChatContent(){
        canvasUIController.activeUIWindows.Remove(targetUIWindow);
        targetUIWindow.SetActive(false);
        chatText.SetText("");
        GetComponent<Collider>().enabled = true;
        closeButton.onClick.RemoveAllListeners();
    }

    protected void ResetTradeButton(){
        Button tradeButton = targetUIWindow.transform.Find("ChatBox").Find("NavigationBar").Find("TradeButton").GetComponent<Button>();
        tradeButton.onClick.RemoveAllListeners();
    }

    protected void SetTradeButton(PlayerManager playerManager){
        if(isTradable){
            PlayerData playerData = playerManager.playerData;
            PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            System.Func<int> GetBuyerMoney = () => {return playerData.GetAttribute("money");};
            System.Action<int> SetBuyerMoney = (amount) => { playerData.AddPlayerData("money",amount);};
            Buyer buyer = new Buyer(SetBuyerMoney,GetBuyerMoney,playerInventory);
            Seller seller = new Seller(GetComponent<NPCInventory>(),$"{npc.name}'s Inventory");

            //targetUIWindow is "ChatBoxUI" for NPC and Player
            Button tradeButton = targetUIWindow.transform.Find("ChatBox").Find("NavigationBar").Find("TradeButton").GetComponent<Button>();
            tradeButton.onClick.AddListener(() => {canvasUIController.tradeWindow.GetComponent<TradingController>().StartTrade(buyer,seller);});
        }   
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