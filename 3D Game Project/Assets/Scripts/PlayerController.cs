

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController; 
    // Animator animator;                          
    Vector2 currentMovementInput; 
    Vector3 currentMovement;
    Vector3 currentRunMovement;                 
    bool isMovementPressed;
    bool isRunPressed;                          
    float rotationFactorPerFrame = 1.0f;    
    float runMultiplier = 2.0f;      

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        // animator = GetComponent<Animator>();                            
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;         
        playerInput.CharacterControls.Run.canceled += onRun;        
    }

    void onRun(InputAction.CallbackContext context)     
    {
        isRunPressed = context.ReadValueAsButton();
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

    // void handleAnimation()                                  
    // {
    //     bool isWalking = animator.GetBool("isWalking");
    //     bool isRunning = animator.GetBool("isRunning");
    //     if(isMovementPressed && !isWalking)   
    //     {
    //         animator.SetBool("isWalking", true);
    //     }
    //     else if(!isMovementPressed && isWalking) 
    //     {
    //         animator.SetBool("isWalking", false);
    //     }
    //     if(isRunPressed && isWalking)   
    //     {
    //         animator.SetBool("isRunning", true);
    //     } else if (!isRunPressed && isRunning)
    //     {
    //         animator.SetBool("isRunning", false);
    //     }
    // }

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
        // handleAnimation();             
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
