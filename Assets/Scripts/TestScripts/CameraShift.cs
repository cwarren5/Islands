using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    [SerializeField] float camSensitivity = 0.5f;
    [SerializeField] float mouseRange = 15.0f;
    private float mouseXClamp = 0;
    private float mouseYClamp = 0;
    private Quaternion defaultTilt = default;

    [SerializeField] private float transitionSpeed = 1.0f;
    private float lerpProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultTilt = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X"); //get x
        float mouseY = Input.GetAxis("Mouse Y"); //get y

        mouseXClamp += mouseX;
        mouseXClamp = Mathf.Clamp(mouseXClamp, -1 * mouseRange, mouseRange);
        mouseYClamp += mouseY;
        mouseYClamp = Mathf.Clamp(mouseYClamp, -1 * mouseRange, mouseRange);

        Debug.Log(mouseXClamp);
        if(mouseYClamp == mouseRange || mouseYClamp == (-1 * mouseRange))
        {
            mouseY = 0;
        }
        if (mouseXClamp == mouseRange || mouseXClamp == (-1 * mouseRange))
        {
            mouseX = 0;
        }


        if (Input.GetMouseButton(0))
        {
            transform.localRotation = defaultTilt;
        }
        else
        {
            Vector3 movementVectorX = new Vector3(mouseY, 0, mouseX);
            transform.Rotate(movementVectorX * camSensitivity);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
    }
}
