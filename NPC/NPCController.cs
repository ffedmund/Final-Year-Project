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
    
    private Transform playerTransform;
    private Quaternion initialRotation;
    private NPCInteraction npcInteraction;
    private NPCInventory npcInventory;

    void Start()
    {
        Debug.Log($"NPC ID: {npc.id}");
        Debug.Log($"NPC Name: {npc.npcName}");

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

        if(TryGetComponent(out npcInteraction)){
            npcInteraction.Setup(npc);
        }
        if(TryGetComponent(out npcInventory)){
            npcInventory.Setup(npc);
        }
    }

    void Update()
    {
        // Calculate the distance between the NPC and the player
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // If the distance is less than maxDistance, turn the head of the NPC towards the player using an Animator
        if (distanceToPlayer < maxDistance)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < 90.0f && angleToPlayer > -90.0f) 
            {
                // animator.SetFloat("Direction", 0.5f);
                float angleBetweenForwardAndDirectionToPlayer = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);
                float direction = (angleBetweenForwardAndDirectionToPlayer + 90.0f) / 180.0f;
                // animator.SetFloat
                DOTween.To(()=>animator.GetFloat("direction"),x=>animator.SetFloat("direction",x),direction,1);
            }
            else if(angleToPlayer < 120.0f && angleToPlayer > -120.0f)
            {
                animator.SetFloat("direction", directionToPlayer.x > 0 ? 1.0f : 0.0f);
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.DORotateQuaternion(targetRotation, turnSpeed);
            }
        }
        else
        {
            // If the distance is greater than or equal to maxDistance, reset both objects' rotation using DOTween
            animator.SetFloat("direction", 0.5f);
            transform.DORotateQuaternion(initialRotation, turnSpeed);
            npcInteraction.Init();
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
