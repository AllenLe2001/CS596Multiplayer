using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class IntelScript : NetworkBehaviour
{
    public NetworkManagerUI NetworkUI;
    // Start is called before the first frame update
    void Start()
    {
        NetworkUI = GameObject.FindObjectOfType<NetworkManagerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            NetworkUI.updateScore();
            Debug.Log("Collided with user");
            DespawnClientRpc();
        }
    }


    [ClientRpc]
    private void DespawnClientRpc(){
        Debug.Log("Mehtod called");
        gameObject.SetActive(false);
    }
}
