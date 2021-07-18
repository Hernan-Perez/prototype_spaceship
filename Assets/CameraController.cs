using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float vel = 3f;
    public float scrollSpeed = -10f;
    public float cameraDistance = 10f;
    public float offsetAltura = 0f;
    public float cameraDistanceMin = 5f;
    public float cameraDistanceMax = 25f;

    private Vector3 offset;
    private bool zoomCambio = true;

    private Vector3 navePos;

    // Use this for initialization
    void Start () {
        offset = new Vector3(0f, offsetAltura, 0f);

        //mira = Resources.Load("mira") as Texture2D;
    }
	
	// Update is called once per frame


	void Update () {

        navePos = transform.parent.position;

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            zoomCambio = true;
        }

        if (zoomCambio)
        {
            zoomCambio = false;
            transform.position = transform.parent.position - transform.forward * cameraDistance;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
        }


        if (Input.GetMouseButton(1))
        {
            //Debug.Log(Vector3.Angle(transform.parent.up, transform.forward));
            transform.RotateAround(navePos, transform.parent.up, Input.GetAxis("Mouse X") * vel);
            if (Vector3.Angle(transform.parent.up, transform.forward) + Input.GetAxis("Mouse Y") * -vel <= 170f &&
                Vector3.Angle(transform.parent.up, transform.forward) + Input.GetAxis("Mouse Y") * -vel >= 10f)
            {
                transform.RotateAround(navePos, Vector3.Cross(transform.parent.up, transform.forward), Input.GetAxis("Mouse Y") * -vel);
            }
            
        }
       /* else
        {
            transform.LookAt(Vector3.zero + offset);
            //transform.RotateAround(Vector3.zero, Vector3.up, 0);
        }*/

        transform.LookAt(navePos + offset, transform.parent.up);

    }
}
