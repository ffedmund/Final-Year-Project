using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FYP;
using DG.Tweening;

public class NPCController : MonoBehaviour
{
    public NPC npc;
    public float maxRaycastDistance = 50.0f; // Set this to the maximum expected distance between the NPC and the ground
    public float turnSpeed = 5.0f; // Set this to adjust how quickly the NPC turns its head
    public float maxDistance = 5.0f; // Set this to adjust how close the player needs to be for the NPC to turn its head
    public Animator animator;
    public Notice notice;
    
    private Transform playerTransform;
    private Quaternion initialRotation;
    private NPCInteraction npcInteraction;
    private NPCInventory npcInventory;
    private QuestList questList;
    private QuestGiver questGiver;
    private QuestReceiver questReceiver;

    void Start()
    {
        #region Initiate NPC Object
        Debug.Log($"NPC ID: {npc.id}");
        Debug.Log($"NPC Name: {npc.npcName}");
        GameObject npcObject = Instantiate(npc.npcPrefab,this.transform);
        animator = transform.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = npc.npcAnimator;
        npcObject.layer = 9;
        foreach(Transform child in npcObject.transform){
            child.gameObject.layer = 9;
        } 
        #endregion

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Save the initial rotation of the NPC
        initialRotation = transform.rotation;

        // Cast a ray downwards from the position of this GameObject
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxRaycastDistance))
        {
            transform.position = hit.point;
        }
        else
        {
            Debug.LogError("No ground found below NPC!");
        }
        if(npc.questIdList.Length > 0){
            questList = gameObject.AddComponent<QuestList>();
            questList.Setup(npc.questIdList,QuestType.Special);
            if(npc.isQuestGiver){
                questGiver = gameObject.AddComponent<QuestGiver>();
            }
            if(npc.isQuestReceiver){
                questReceiver = gameObject.AddComponent<QuestReceiver>();
            }
        }
        if(TryGetComponent(out npcInteraction)){
            npcInteraction.Setup(npc,animator);
        }
        if(TryGetComponent(out npcInventory)){
            npcInventory.Setup(npc);
        }
        notice = GetComponentInChildren<Notice>();
    }

    void Update()
    {
        // Calculate the distance between the NPC and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // If the distance is less than maxDistance, turn the head of the NPC towards the player using an Animator
        if (distanceToPlayer < maxDistance)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < 90.0f && angleToPlayer > -90.0f) 
            {
                // animator.SetFloat("Direction", 0.5f);
                float angleBetweenForwardAndDirectionToPlayer = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);
                float direction = (angleBetweenForwardAndDirectionToPlayer + 90.0f) / 180.0f;
                // animator.SetFloat
                DOTween.Kill("directionTween"+"-"+npc.name);
                DOTween.To(()=>animator.GetFloat("direction"),x=>animator.SetFloat("direction",x),direction,1).SetId("directionTween"+"-"+npc.name);
            }
            else if(angleToPlayer < 120.0f && angleToPlayer > -120.0f)
            {
                animator.SetFloat("direction", directionToPlayer.x > 0 ? 1.0f : 0.0f);
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                DOTween.Kill("rotationTween"+"-"+npc.name);
                transform.DORotateQuaternion(targetRotation, turnSpeed).SetId("rotationTween"+"-"+npc.name);
            }

            if(notice){
                notice.gameObject.SetActive(true);
                notice.UpdateNoticeDirection(directionToPlayer);
                if(questList){
                    if(questReceiver && questList.currentQuestList.FindAll(quest => questList.CanReportQuest(quest)&&!quest.isFinished&&quest.isActive).Count > 0){
                        notice.SetNotice(1);
                    }else if(questGiver && questList.currentQuestList.FindAll(quest => !quest.isFinished && !quest.isActive && (int)quest.honorRank <= playerTransform.GetComponent<PlayerManager>().playerData.GetHonorLevel()).Count > 0){
                        notice.SetNotice(0);
                    }
                    else{
                        notice.ClearNotice();
                    }
                }
            }
        }
        else
        {
            // If the distance is greater than or equal to maxDistance, reset both objects' rotation using DOTween
            animator.SetFloat("direction", 0.5f);
            if(transform.rotation != initialRotation){
                DOTween.Kill("rotationTween"+"-"+npc.name);
                transform.DORotateQuaternion(initialRotation, turnSpeed).SetId("rotationTween"+"-"+npc.name);
            }
            if(npcInteraction.isInteracting)npcInteraction.Init();
            if(notice)notice.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player entered NPC's trigger zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exited NPC's trigger zone");
        }
    }
}
