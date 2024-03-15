using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class IntelDetector : NetworkBehaviour
{
    //references
    public GameObject[] intelArray; // putting all the intel game objects in here as an array of gameobjects
    public AudioSource detect;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        //checking if we are the host
        if(!IsOwner) return;
        //when we click on the detect button which is F
        if(Input.GetKey(KeyCode.F)){
        detectIntel();
        }
    }


    //method for the detector
    void detectIntel(){
            Debug.Log("button f has been clicked");
            //checking each object in the intelArray
            foreach(GameObject intel in intelArray){
                //also checking if the target exists
                if(intel != null){
                    //calculating the distance between the player and the itel
                    float distance = Vector3.Distance(transform.position, intel.transform.position);

                    //check if the intel is within range to be detected
                    if(distance <= 12f){
                        Debug.Log("In Range");
                        //calculating volume so that closer intel will have a louder volume
                        //float volume = 1f - (distance / 12f);
                        
                        //play the detection sound with its volume
                        detect.Play();

                    }
                    
                    else if (distance > 12f){
                        Debug.Log("Out of Range");
                    }
                }
            }
    }
}
