using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WinScreen : NetworkBehaviour
{
    public bool GameWon = false;
    public GameObject winUI;


    // Update is called once per frame
    void Update()
    {
        //when the game is won show the screen and freeze the game
        if(GameWon){
            winUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else{
            winUI.SetActive(false);
            Time.timeScale = 1f;
        }
        
    }

    //using clientRpc to sync the values across all clients
    [ClientRpc]
    public void GameWonClientRpc(){
        GameWon = true;
    }
}
