using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LostScreen : NetworkBehaviour
{
    public bool GameLost = false;
    public GameObject lostUI;


    // Update is called once per frame
    void Update()
    {
        //when the game is lost show the screen and freeze the game
        if(GameLost){
            lostUI.SetActive(true);
            freezeTimeClientRpc();
        }
        else{
            lostUI.SetActive(false);
            enableTimeClientRpc();
        }
        
    }

    //using clientRpc to sync the values across all clients
    [ClientRpc]
    public void GameLostClientRpc(){
        GameLost = true;
    }

    [ClientRpc]
    void freezeTimeClientRpc(){
        Time.timeScale = 0f;
    }

    [ClientRpc]
    void enableTimeClientRpc(){
        Time.timeScale = 1f;
    }


}
