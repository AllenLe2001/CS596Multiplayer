using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkSpawnerScript : NetworkBehaviour
{
    //REFERENCES
    public GameObject laptop;
    public GameObject briefcase;
    public GameObject tablet;
    public GameObject phone;
    public GameObject enemy;

    //using an array for fixed spawn positions for the laptop (it can spawn in 1 of 4 positions)
    public Transform[] spawnSpots;
    public Transform[] BriefSpots;
    public Transform[] tabletSpots;
    public Transform[] phoneSpots;
    public Transform enemySpot;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if(IsHost){
            SpawnItemServerRpc();
            SpawnEnemyServerRpc();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //method in order to get random spawn locations for the laptop
    Vector3 RandomSpawnPosition(){
        //choosing a random index out of the spawnSpots array 
        int spawnIndex =  Random.Range(0, spawnSpots.Length);
        Vector3 spawn = spawnSpots[spawnIndex].position;
        return spawn;
    }

    //same method as above but for the briefcases
    Vector3 RandomBriefPosition(){
        int briefIndex = Random.Range(0, BriefSpots.Length);
        Vector3 spawnBrief = BriefSpots[briefIndex].position;
        return spawnBrief;
    }

    //same method as above but for the tablets
    Vector3 RandomTabPosition(){
        int tabIndex = Random.Range(0, tabletSpots.Length);
        Vector3 spawnTab = tabletSpots[tabIndex].position;
        return spawnTab;
    }

    //same method as above but for the phones
    Vector3 RandomPhonePosition(){
        int pIndex = Random.Range(0, phoneSpots.Length);
        Vector3 spawnPhone = phoneSpots[pIndex].position;
        return spawnPhone;
    }

    //putting into server rpc so we can spawn on the server for all clients
    [ServerRpc]
    private void SpawnItemServerRpc(){
        GameObject laptopObject = Instantiate(laptop, RandomSpawnPosition(), Quaternion.identity);
        GameObject briefObject = Instantiate(briefcase, RandomBriefPosition(), Quaternion.identity);
        GameObject tabObject = Instantiate(tablet, RandomTabPosition(),Quaternion.identity);
        GameObject pObject = Instantiate(phone, RandomPhonePosition(), Quaternion.identity);
        laptopObject.GetComponent<NetworkObject>().Spawn();
        briefObject.GetComponent<NetworkObject>().Spawn();
        tabObject.GetComponent<NetworkObject>().Spawn();
        pObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void SpawnEnemyServerRpc(){
        GameObject enemyObject = Instantiate(enemy, enemySpot.position, Quaternion.identity);
        enemyObject.GetComponent<NetworkObject>().Spawn();
    }

}
