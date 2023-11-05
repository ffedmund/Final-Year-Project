using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYP;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System;

public class NPCInteraction : InteractableScript
{
    public TextMeshProUGUI chatText;
    public NPC npc;
    public bool isInteracting;

    Button closeButton;
    Button answerButton;
    QuestReceiver questReceiver;
    QuestGiver questGiver;
    Animator _animator;
    bool isTradable;

    new Camera camera;

    public void Setup(NPC npc, Animator animator){
        this.npc = npc;
        isTradable = npc.isTradable;
        _animator = animator;
        camera = GetComponentInChildren<Camera>();
        TryGetComponent(out questReceiver);
        TryGetComponent(out questGiver);
    }

    void Start() {
        canvasUIController = FindAnyObjectByType<UIController>();
        TryGetComponent(out questReceiver);
        TryGetComponent(out questGiver);
        targetUIWindow = canvasUIController.chatBoxWindow;
        chatText = targetUIWindow.transform.Find("ChatBox").GetComponentInChildren<TextMeshProUGUI>();
        answerButton = targetUIWindow.transform.Find("ChatBox").Find("AnswerButton").GetComponent<Button>();
        closeButton = targetUIWindow.transform.Find("ChatBox").Find("CloseButton").GetComponent<Button>();
        isUITrigger = true;
    }

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        
        if(questReceiver || questGiver)
        {
            Quest canReportQuest = questReceiver?playerManager.playerData.quests.Find(quest => quest.goalChecker.isReached()&&questReceiver.GetComponent<QuestList>().CanReportQuest(quest)):null;
            Quest canGiveQuest = questGiver?questGiver.GetComponent<QuestList>().currentQuestList.Find(quest => (int)quest.honorRank <= playerManager.playerData.GetHonorLevel() && !quest.isActive):null;

            if(canReportQuest != null)
            {
                questReceiver.ReportQuest(canReportQuest);
                SetChatContent(canReportQuest.completeDialog);
            }else if(canGiveQuest != null){
                SetChatContent(canGiveQuest.description);
                SetAnswerButton("Sure.",canGiveQuest);
            }
            else
            {
                SetChatContent();
            }
        } 
        if(!questGiver && !questReceiver)
        {
            SetChatContent();
        }
        SetTradeButton(playerManager);
        GetComponent<Collider>().enabled = false;
        camera.enabled = true;
        isInteracting = true;
        _animator.Play("Talking");
    }

    public void Init(){
        HideChatContent();
        ResetTradeButton();
        ResetAnswerButton();
        camera.enabled = false;
        isInteracting = false;
        GetComponent<Collider>().enabled = true;
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

    protected void ResetAnswerButton(){
        answerButton.gameObject.SetActive(false);
        answerButton.onClick.RemoveAllListeners();
    }

    protected void SetAnswerButton(string answerText,Quest quest){
        answerButton.gameObject.SetActive(true);
        answerButton.GetComponentInChildren<TextMeshProUGUI>().SetText(answerText);
        answerButton.onClick.AddListener(()=>{
            questGiver.AcceptQuest(quest);
        });
    }

    protected void HideChatContent(){
        canvasUIController.activeUIWindows.Remove(targetUIWindow);
        targetUIWindow.SetActive(false);
        chatText.SetText("");
        closeButton.onClick.RemoveAllListeners();
    }


    protected void SetChatContent(string specificText){
        chatText.SetText($"<line-height=150%><size=120%>{npc.name}</size>\n{specificText}");
        closeButton.onClick.AddListener(()=>{Init();});
    }

    protected void SetChatContent(){
        if(npc.dialogues.Length > 0){
            chatText.SetText($"<line-height=150%><size=120%>{npc.name}</size>\n{npc.dialogues[UnityEngine.Random.Range(0,npc.dialogues.Length)]}");
        }
        closeButton.onClick.AddListener(()=>{Init();});
    }
}