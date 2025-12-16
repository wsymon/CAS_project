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

//Come back later to add domain restrictions for camera locations so that it can barely see (on max zoom) the tiles 
//on the line x = 0, y = 0 and the other side of the map however large it is...
//perhaps adjust max zoom if map is smaller depending on Henry tile sizing...