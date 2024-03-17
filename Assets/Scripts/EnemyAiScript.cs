using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class EnemyAiScript : MonoBehaviour
{
    //variables and references
    //private Transform player; switching to array of players so we can account for all players
    public Transform[] playerList;
    public AudioSource aS;
    public AudioClip yellSound;
    public AudioSource chaseSound;
    public AudioSource heartbeat;
    public AudioSource attackedSound;
    public bool yellPlayed = false;
    public Transform targetPlayer;
    private NavMeshAgent agent;
    public Animator anim;
    public NetworkManagerUI NetworkUI;

    // Start is called before the first frame update
    void Start()
    {
        //initializing the agent
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //make an array of gameobjects and find them by the player tag
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerList = new Transform[players.Length];
        for(int i = 0; i < players.Length; i++){
            playerList[i] = players[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //referencing the networkUI script to get the network variable
        NetworkUI = GameObject.FindObjectOfType<NetworkManagerUI>();
        //updating list for changes
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerList = new Transform[players.Length];
        for(int i = 0; i < players.Length; i++){
            playerList[i] = players[i].transform;
        }
        //find the player closest to chase
        findPlayerClosest();
        if(transform.position == Vector3.zero){
            anim.SetFloat("Speed", 0);
        }
        //once we have found 2 objectives the enemy will be alerted and plays the warning audio
        if(NetworkUI.score.Value == 2 && !yellPlayed){
            aS.PlayOneShot(yellSound);
            yellPlayed = true;
        }
        //chasing once we have found at least 2 objectives
        if(NetworkUI.score.Value >= 2 && NetworkUI.score.Value != 4){
            chasePlayer();
            chaseSound.Play();
            chaseSound.loop = true;
        }
        //play heartbeat audio once the enemy is close
        if(targetPlayer != null){
        if(Vector3.Distance(transform.position, targetPlayer.position) <= 5f){
            heartbeat.Play();
        }
        //once we win the enemy will disappear
        if(NetworkUI.score.Value == 4){
            gameObject.SetActive(false);
        }
        }
    }

 //method for find the player that is closest to the enemy
    void findPlayerClosest(){
    //default value / placeholder vlaue
    float closest = Mathf.Infinity;
    //calculating each distance for each player from the enemy
    foreach(Transform player in playerList){
        float currentDistance = Vector3.Distance(transform.position, player.position);
        //update closest distance if the distance is smaller
        if(currentDistance < closest){
            closest = currentDistance;
            targetPlayer = player;
        }
    }
   } 

 //method for the enemy to chase the player
    void chasePlayer(){
        if(targetPlayer != null) {
         agent.destination = targetPlayer.position;
         anim.SetFloat("Speed", 1f);
        }
    }

    void OnTriggerEnter(Collider other){
     // Check if the collided object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
           attackedSound.Play();
           other.gameObject.SetActive(false);
           heartbeat.Stop();
           NetworkUI.updateSurvivors();

        }
    }
    
}
