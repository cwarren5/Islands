using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private bool pressed = false;
    private bool hasBomb = false;
    [SerializeField] private GameObject boatRender = default;
    [SerializeField] private GameObject nightShade = default;
    [SerializeField] private GameObject explodingParticles = default;
    [SerializeField] private GameObject bomb = default;
    [SerializeField] private GameObject bombIndicator = default;

    // Start is called before the first frame update
    void Start()
    {
        nightShade.SetActive(false);
        bomb.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("b") && pressed && hasBomb)
        {
            bomb.SetActive(true);
            bomb.transform.position = gameObject.transform.position;
        }
        if (Input.GetKeyUp("b"))
        {
            bomb.SetActive(false);
            hasBomb = false;
            bombIndicator.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "island")
        {
            explodingParticles.SetActive(true);
            boatRender.SetActive(false);
        }

        if (other.tag == "enemybomb" && !pressed)
        {
            explodingParticles.SetActive(true);
            boatRender.SetActive(false);
        }
    }

    void OnMouseDown()

    {

        mZCoord = Camera.main.WorldToScreenPoint(
        gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
        nightShade.SetActive(true);
        pressed = true;
        hasBomb = true;
        bombIndicator.SetActive(true);
    }

    private void OnMouseUp()
    {
        pressed = false;
        nightShade.SetActive(false);
    }


    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }



    void OnMouseDrag()
    {
        transform.position = GetMouseAsWorldPoint() + mOffset;
    }
}
