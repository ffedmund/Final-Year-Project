using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleManager : MonoBehaviour
{   
    public float time;
    public float currentTime;
    [SerializeField]
    Transform lightSource;
    Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = lightSource.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        currentTime = time%360;
        UpdateLightIntensity();
    }

    void UpdateLightIntensity(){
        light.intensity = 1-Mathf.Cos(Mathf.Deg2Rad*currentTime);
    }

    void UpdateLightAngle(){
        // Get the current rotation angles
        Vector3 rotation = lightSource.eulerAngles;

        // Set the x angle based on time
        rotation.x = -Mathf.Cos(Mathf.Deg2Rad*currentTime) * 74;

        // Apply the new rotation angles
        lightSource.eulerAngles = rotation;
    }
}
