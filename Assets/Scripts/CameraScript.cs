using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CameraScript : NetworkBehaviour
{
    //variables
    //variable for mouse sensitivity
    public float mouseSens;
    public Transform parent;


    //REFERENCES
    public GameObject playerCamera;
    public Vector3 offset;

    public override void OnNetworkSpawn(){
        if(IsOwner){
            SpawnCam();
        }
    }

    public void Update(){
        if(!IsOwner) return;
        Rotate();
    }

    //used to rotate our character using our mouse;
    private void Rotate(){
        //getting input from our mouse for both x and y axes
        float xMouse = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        //rotating our character based on our mouse
        parent.Rotate(Vector3.up, xMouse);
    }

    //used to spawn the camera for the player
    private void SpawnCam(){
        playerCamera.SetActive(true);
        //obtain the character position without factoring in the rotation
        playerCamera.transform.position = transform.position + offset;
        //locking our cursor to a specific position
        Cursor.lockState = CursorLockMode.Locked;
    }
}
