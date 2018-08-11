using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed;
    public float maxHeight;
    public float minHeight;


    private float zoomSpeed;
    private float currentHeight;
    private float zoomDistance;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0), Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime), Space.World);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);
        }
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            zoom(zoomDelta);
        }
    }
    
    private void zoom(float zoomDelta)
    {
        currentHeight = getHeight();
        if (currentHeight > minHeight && currentHeight < maxHeight)
        {
            zoomSpeed = currentHeight;
            // I multiply by -1 to invert the mouses scroll direction to match convention
            float timeDelta = getTimeDelta();
            zoomDistance = speed * timeDelta * zoomDelta * zoomSpeed * -1;
            translateCamera(zoomDistance);
            float newHeight = getHeight();
            Debug.Log(zoomDistance);
            Debug.Log(currentHeight);
            Debug.Log(newHeight);
            if (newHeight <= minHeight)
            {
                translateCamera(-zoomDistance);
            }
            else if (newHeight >= maxHeight)
            {
                translateCamera(-zoomDistance);
            }
        }
    }

    private float getTimeDelta()
    {
        return Time.deltaTime;
    }

    private float getHeight()
    {
        return transform.position.y;
    }

    private void translateCamera(float distance)
    {
        transform.Translate(new Vector3(0, distance , 0), Space.World);
    }
}
