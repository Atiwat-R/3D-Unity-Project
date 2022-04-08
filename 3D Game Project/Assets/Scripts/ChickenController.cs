

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChickenController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController; 
    Animator animator;                          
    Vector2 currentMovementInput; 
    Vector3 currentMovement;
    Vector3 currentRunMovement; 

    bool isMovementPressed;
    bool isRunPressed;     
    bool isAttackPressed;      

    public float maxHealth = 1000f;
    public float HP; 
    public HealthbarManager healthBar;

    float rotationFactorPerFrame = 1.0f;    
    float runMultiplier = 2.0f;   
    public float pushPower = 2.0F;   

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();                            
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;         
        playerInput.CharacterControls.Run.canceled += onRun;   
        playerInput.CharacterControls.Attack.performed += OnAttack;   

        this.HP = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void onRun(InputAction.CallbackContext context)     
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnAttack(InputAction.CallbackContext context) {
        animator.SetTrigger("isAttack");
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
            currentMovementInput = context.ReadValue<Vector2>();
            currentMovement.x = currentMovementInput.x;
            currentMovement.z = currentMovementInput.y;
            currentRunMovement.x = currentMovementInput.x * runMultiplier;         
            currentRunMovement.z = currentMovementInput.y * runMultiplier;         
            isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
    }

    // When Chicken hits something, it knocks them back
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower; //* characterController.velocity.magnitude;
    }

    // When Chicken is hit by something
    void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == "Bullet")
		{
            // Debug.Log($"HP: {this.HP}");

            this.HP -= 15f; // Bullet Damage to Chicken
            healthBar.SetHealth(HP);

            // Check if Dead
            if (this.HP <= 0) {
                Debug.Log("DEAD!!!!!!!!!!!!!!!!!!!!!!!!!");
                SceneManager.LoadScene("GameOverScreen");
            }
		}
	}

    void handleRotation()               
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;
        if(isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame);
        }
    }

    void handleAnimation()                                  
    {
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if(isMovementPressed && !isWalking)   
        {
            animator.SetBool("isWalking", true);
        }
        else if(!isMovementPressed && isWalking) 
        {
            animator.SetBool("isWalking", false);
        }
        if(isRunPressed && isWalking)   
        {
            animator.SetBool("isRunning", true);
        } 
        else if (!isRunPressed && isRunning)
        {
            animator.SetBool("isRunning", false);
        }
    }

    void handleGravity()    // <--- Added the whole method
    {
        if(characterController.isGrounded) 
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } else {
            float gravity = -5.8f;
            currentMovement.y = gravity;
            currentRunMovement.y = gravity;
        }
    }

    void Update() 
    {
        handleGravity();     // <--- Added
        handleRotation();           
        handleAnimation();           
        if(isRunPressed)                                                        
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else 
        {
            characterController.Move(currentMovement * Time.deltaTime);
        } 
        
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }
    
    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }




}







// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerController : MonoBehaviour
// {

//     PlayerInput playerInput;
//     CharacterController characterController;
//     Animator animator;

//     Vector2 currentMovementInput;
//     Vector3 currentMovement;
//     bool isMovementPressed;
//     float rotationFactorPerFrame = 0.5f;

//     // Instantiate object of class PlayerInput
//     void Awake()
//     {
//         playerInput = new PlayerInput();
//         characterController = GetComponent<CharacterController>();
//         animator = GetComponent<Animator>();

//         playerInput.CharacterControls.Move.started += context => { 
//             currentMovementInput = context.ReadValue<Vector2>();
//             currentMovement.x = currentMovementInput.x;
//             currentMovement.z = currentMovementInput.y;
//             isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
//             Debug.Log(context.ReadValue<Vector2>()); 
//         };
//         playerInput.CharacterControls.Move.performed += context => { 
//             currentMovementInput = context.ReadValue<Vector2>();
//             currentMovement.x = currentMovementInput.x;
//             currentMovement.z = currentMovementInput.y;
//             isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
//             Debug.Log(context.ReadValue<Vector2>()); 
//         };
//         playerInput.CharacterControls.Move.canceled += context => { 
//             currentMovementInput = context.ReadValue<Vector2>();
//             currentMovement.x = currentMovementInput.x;
//             currentMovement.z = currentMovementInput.y;
//             isMovementPressed = currentMovement.x != 0 || currentMovement.z != 0;
//             Debug.Log(context.ReadValue<Vector2>()); 
//         };

//     }

//     void handleRotation() {
//         Vector3 positionToLookAt;
//         positionToLookAt.x = currentMovement.x;
//         positionToLookAt.y = 0.0f;
//         positionToLookAt.z = currentMovement.z;

//         Quaternion currentRotation = transform.rotation;
//         if (isMovementPressed) {
//             Quaternion targetlocation = Quaternion.LookRotation(positionToLookAt);
//             transform.rotation = Quaternion.Slerp(currentRotation, targetlocation, rotationFactorPerFrame);
//         }

//     }

//     void handleAnimation() {
//         bool isWalking = animator.GetBool("isWalking");
//         bool isRunning = animator.GetBool("isRunning");

//         if (isMovementPressed && !isWalking) {
//             animator.SetBool("isWalking", true);
//         }
//         else if (isMovementPressed && isWalking) {
//             animator.SetBool("isWalking", false);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         handleRotation();
//         handleAnimation();
//         characterController.Move(currentMovement * Time.deltaTime);
//     }

//     void OnEnable()
//     {
//         playerInput.CharacterControls.Enable();
//     }

//     void OnDisable()
//     {
//         playerInput.CharacterControls.Disable();
//     }


// }
