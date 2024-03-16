using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Components;

public class NetworkManagerUI : NetworkBehaviour
{
    //referenced from YT Vid: "COMPLETE UNITY MULITPLAYER Tutorial(Netcode for Game Objects)"
    //script for the buttons for the NetworkManager

    //REFERENCES
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private TextMeshProUGUI playersCountText;
    [SerializeField] private TextMeshProUGUI scoreCount;
    public Canvas controlsUI;
    public AudioSource aS;
    public AudioClip alertSound;
    public AudioSource collected;
    public AudioSource gameLost;
    public GameObject winScreen;
    public GameObject lostScreen;
    public WinScreen winUI;
    public LostScreen lostUI;

    public NetworkVariable<int> NumPlayers = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    public NetworkVariable<int> score = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
     public NetworkVariable<int> deaths = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    // Start is called before the first frame update
    void Awake(){
        //finding the win and lost UI via tag
        winScreen = GameObject.FindWithTag("Win");
        lostScreen = GameObject.FindWithTag("Lose");
        winUI = winScreen.GetComponent<WinScreen>();
        lostUI = lostScreen.GetComponent<LostScreen>();
        //starting host when clicking on host button   
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            //destorying the control interface once we are in game
            if(controlsUI != null){
            Destroy(controlsUI.gameObject);
            }
            Debug.Log("IsHost Value: " + IsHost);
        });
        //starting client when clicking on client button
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            //destorying the control interface once we are in game
            if(controlsUI != null){
            Destroy(controlsUI.gameObject);
            }
        });

    }



    // Update is called once per frame
    void Update()
    {  
         if(score.Value == 2){
            aS.clip = alertSound;
            aS.Play();
         }
         scoreCount.text = "Objectives Collected: " + score.Value + "/4 ";
         playersCountText.text = "Players: " + NumPlayers.Value.ToString();
        //only being excuted on the server aka the HOST
        if(!IsHost) return;
        //obtaining number of players based on the clients open
        NumPlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
        Debug.Log("NumPlayers: " + NumPlayers.Value);
        //if our deaths match the number of players we lose
        if(deaths.Value == NumPlayers.Value && NumPlayers.Value != 0){
            Debug.Log("Mission Failed....");
        }
    }

    //method to be called on to update the score
    public void updateScore(){
        collected.Play();
        score.Value += 1;
        //if we hit the objective count then we show win screen by changing the variable gameWon to true 
        //using 4 for now but in a bigger scale we would just take the initial length of the objective array
        if(score.Value == 4) {
        winUI.GameWonClientRpc();
        }

    }

    //method to keep track the amount of deaths
    public void updateSurvivors(){
        deaths.Value += 1;
        if(deaths.Value == NumPlayers.Value && NumPlayers.Value != 0){
            gameLost.Play();
            lostUI.GameLostClientRpc();
        }
    }
}
