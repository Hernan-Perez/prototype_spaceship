using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    public Transform target;
    public float vel = 3f;
    public float scrollSpeed = -10f;
    public float cameraDistance = 10f;
    public float offsetAltura = 2f;
    public float cameraDistanceMin = 5f;
    public float cameraDistanceMax = 25f;
    //private Rigidbody rb;
    //private Vector3 of;
    //private float timerResetCamera = 1.5f;
    private Vector3 ultimaPos;

    private Vector3 offset;
    private bool zoomCambio = false;

    

    // Use this for initialization
    void Start () {
        //target = transform.parent.transform;
        //of = new Vector3(0f, 4.25f, -8f);
        //of.Normalize();
        //rb = target.GetComponentInParent<Rigidbody>();
        //Cursor.visible = false;
        
        offset = new Vector3(0f, offsetAltura, 0f);
        ultimaPos = target.position;
        ActualizarPosicionCamara(true);
        

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            zoomCambio = true;
        }

        ActualizarPosicionCamara();

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
            transform.RotateAround(target.position, transform.parent.up, Input.GetAxis("Mouse X") * vel);
            transform.RotateAround(target.position, Vector3.Cross(transform.forward, transform.parent.up), Input.GetAxis("Mouse Y") * vel);
        }
        else
        {
            transform.LookAt(target.position + offset, transform.parent.up);
            transform.RotateAround(target.position, transform.parent.up, 0);
        }

        transform.LookAt(target.position + offset, transform.parent.up);
    }

    void ActualizarPosicionCamara(bool setup = false)
    {
        if (setup)
        {
            Vector3 aux = transform.position - (new Vector3(target.position.x, transform.position.y, target.position.z));
            aux.Normalize();
            transform.position = new Vector3(
                target.position.x + aux.x * cameraDistance,
                transform.position.y + aux.y * cameraDistance,
                target.position.z + aux.z * cameraDistance);//target.position + aux * cameraDistance;
            ultimaPos = target.position;
        }
        else if (ultimaPos != target.position || zoomCambio)
        {
            zoomCambio = false;
            Vector3 dif = target.position - ultimaPos;
            transform.position = transform.position + dif;
            ultimaPos = target.position;

            Vector3 aux = transform.position - (new Vector3(target.position.x, transform.position.y, target.position.z));
            aux.Normalize();
            transform.position = new Vector3(
                target.position.x + aux.x * cameraDistance,
                transform.position.y + aux.y * cameraDistance,
                target.position.z + aux.z * cameraDistance);//target.position + aux * cameraDistance;
            //ultimaPos = target.position;

        }
       
    }
}


