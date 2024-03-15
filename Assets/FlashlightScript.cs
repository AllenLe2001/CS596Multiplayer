using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FlashlightScript : NetworkBehaviour
{
    //REFERENCES
    [SerializeField] private Light flashlight;
    [SerializeField] private GameObject light;


    // Update is called once per frame
    void Update()
    {
        //checking if we are the owner of this flashlight
        if(!IsOwner) return;
        //F is the button to use the flashlight
        if(Input.GetKeyDown(KeyCode.F)){
            //call upon the server rpc method
            flashlightServerRpc();
            
        }
    }


    [ServerRpc]
    //using server rpc to sync the flashlight to all clients
    private void flashlightServerRpc(){
        //only spawn in the flashlight once when F is first clicked
        if(flashlight != null && flashlight.enabled == false){
        //GameObject go = Instantiate(light);
        //go.GetComponent<NetworkObject>().Spawn();
        flashlight.enabled = true;
        }
        //else we are just turning it off
        else {
            flashlight.enabled = false;
        }
    }
}
