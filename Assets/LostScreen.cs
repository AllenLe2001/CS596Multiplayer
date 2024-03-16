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
            Time.timeScale = 0f;
        }
        else{
            lostUI.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }

    //using clientRpc to sync the values across all clients
    [ClientRpc]
    public void GameLostClientRpc(){
        GameLost = true;
    }
}
