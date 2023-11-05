using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAdder : MonoBehaviour
{   
    public string particleId;
    public GameObject particle;
    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && other.transform.Find(particleId) == null){
            GameObject particleObject = Instantiate(particle,other.transform);
            particleObject.transform.position += new Vector3(0,15,0);
            particleObject.name = particleId;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.tag == "Player"){
            Destroy(other.transform.Find(particleId).gameObject);
        }
    }
}
