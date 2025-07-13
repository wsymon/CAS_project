using Unity.Cinemachine;
using UnityEngine;

public class Camera_ : MonoBehaviour
{

    //references the cinemachine camera not the actual camera as cinemachine camera
    //has priority in camera settings for some reason??
    [SerializeField]
    CinemachineCamera Camera_object;


    //cam zoom defines
    private float zoom;
    private float zoomMultiplier = 4f;
    private float minZoom = 2f;
    private float maxZoom = 14f;
    private float velocity = 0f;
    private float smoothTime = 0.25f;


    private void Start()
    {
        //defining zoom to be the orthographic size (under 'Lens') of the cinemachine cam
        zoom = Camera_object.Lens.OrthographicSize;
    }

    private void Update()
    {
        //what makes the camera's size move smoothly as you scroll the mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * zoomMultiplier;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Camera_object.Lens.OrthographicSize = Mathf.SmoothDamp(Camera_object.Lens.OrthographicSize, zoom, ref velocity, smoothTime);
    }
}
