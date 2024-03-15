using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkManagerScript : MonoBehaviour
{
    public NetworkVariable<int> NumPlayers = new NetworkVariable<int>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
