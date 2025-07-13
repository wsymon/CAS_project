using UnityEngine;
using UnityEngine.InputSystem;


public class Camera_Movement : MonoBehaviour
{
    //cam movement defines
    private Vector2 MovementInput;
    private Rigidbody2D rb;
    public float panSpeed = 5f;

  
    //to reference actual camera game object in unity 
    [SerializeField]
    Camera _camera;

    private void Start()
    {
     //referencing the body of the object to be moved
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        //movement for the camera's following object
        rb.linearVelocity = panSpeed * MovementInput;
    }
    public void Move(InputAction.CallbackContext context)
    {
        //takes movement input and makes it readable to this program
        MovementInput = context.ReadValue<Vector2>();
    }
}

