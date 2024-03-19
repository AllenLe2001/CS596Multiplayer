using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    //variables 
    public float movementSpeed = 0;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    public float rotationSpeed = 720f;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioSource aS;



    Vector3 moveDirection;
    Vector3 velocity;

    public bool isGround;
    public float groundDistance;
    public LayerMask maskForGround;
    public float gravity = -9.81f;
    public float jumpHeight = 5f;
    public float playerDistance = 5f;
    public NetworkManagerUI NetworkUI;

    

    //REFERENCES
    private CharacterController character;
    public Animator anim;

    public override void OnNetworkSpawn(){
        //to spawn players in random fixed locations so we dont have players spawning on top of each other
        PlayerSpawnClientRpc();
    }
    // Start is called before the first frame update
    void Start()
    {
        //referencing the networkUI script to get the network variable
        NetworkUI = GameObject.FindObjectOfType<NetworkManagerUI>();
        NetworkUI.updatePlayerClientRpc();
        //spawn position
        character =  GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //checking if we are the host
        if(!IsOwner) return;
        Move();
    }

    //move function for movement so the code looks cleaner
    void Move(){

        //checking if we are on the ground by drawing sphere on our feet and checking if we are on the layer ground
        isGround = Physics.CheckSphere(transform.position, groundDistance, maskForGround);

        if(isGround && velocity.y < 0){
            velocity.y = -2f;
        }

        //float variables for the axises inputs from W A S D
        float zMovement = Input.GetAxis("Vertical");
        float xMovement = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(xMovement, 0 , zMovement);
        moveDirection = transform.TransformDirection(moveDirection);

        if(isGround){
        //When we are walking (when there is movement but shift has not been pressed)
        if(moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift)){
            //walk
           // walkSounds();
            walk();
        }
        //when we are running (when there is movement and shift has been clicked)
        else if(moveDirection !=  Vector3.zero && Input.GetKey(KeyCode.LeftShift)){
            //run
           // walkSounds();
            run();
        }   
        //we are idle when there is no movement
        else if(moveDirection == Vector3.zero){
            //idle
            idle();
        }

        if(Input.GetKey(KeyCode.Space)){
            aS.loop = false;
            aS.clip = jumpSound;
            aS.Play();
            jump();
        }

          moveDirection *= movementSpeed;

        //for sound effects (have to do it this way as there was a delay and this is more synced towards our movement)
        if(Input.GetKeyDown(KeyCode.W)){
            Debug.Log("Playing walk sound");
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.W)){
            aS.Stop();
        }
        if(Input.GetKeyDown(KeyCode.A)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.A)){
            aS.Stop();
        }
        if(Input.GetKeyDown(KeyCode.S)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.S)){
            aS.Stop();
        }
         if(Input.GetKeyDown(KeyCode.D)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.D)){
            aS.Stop();
        }
        
        }

        character.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        character.Move(velocity * Time.deltaTime);

    }

    void idle(){
        anim.SetFloat("Speed", 0);
    }

    void walk(){
        movementSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.33f);
    }

    void run(){
        movementSpeed = runSpeed;
        anim.SetFloat("Speed", 0.66f);
    }
    //method for jumping refrenced the formula 
    void jump(){
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        anim.SetFloat("Speed", 1f);
    }

    private void walkSounds(){
         //for sound effects (have to do it this way as there was a delay and this is more synced towards our movement)
        if(Input.GetKeyDown(KeyCode.W)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.W)){
            aS.Stop();
        }
        if(Input.GetKeyDown(KeyCode.A)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.A)){
            aS.Stop();
        }
        if(Input.GetKeyDown(KeyCode.S)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.S)){
            aS.Stop();
        }
         if(Input.GetKeyDown(KeyCode.D)){
            aS.clip = walkSound;
            aS.loop = true;
            aS.Play();
        }
        else if (Input.GetKeyUp(KeyCode.D)){
            aS.Stop();
        }

    }


    //syncing spawn positions for clients
    [ClientRpc]
    private void PlayerSpawnClientRpc(){
         //to spawn players in random fixed locations so we dont have players spawning on top of each other
        float x = Random.Range(playerDistance, -playerDistance);
        float z = Random.Range(playerDistance, -playerDistance);
        transform.position = new Vector3 (x, 0 ,z);
    }


}
