using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoPersonaje : MonoBehaviour {

    private enum Estado {Activado, Disponible, Cooldown, Apagando};

    public float fuerza = 10f;
    public float fuerzaHipervelocidad = 1000f;
    public float fuerzaTrasera = 5f;
    public float fuerzaRotacion = 1f;

    public float limiteVelocidad = 20f;
    public float limiteHipervelocidad = 100f;
    public float limiteVelocidadAngular = 2f;

    private float cdHipervelocidad = -10f;
    private float cdnHipervelocidad = 2f;
    private bool hipervelocidad = false;
    private bool apagandoHipervelocidad = false;

    private bool killRot = false;
    private bool speedBreak = false;
    private float speedBreakCD = -10f;
    private float speedBreakCDN = 5f;

    private float dragOriginal;
    private float angDragOriginal;

    private ParticleSystem PropL;
    private ParticleSystem PropR;
    private ParticleSystem PropC;
    private ParticleSystem PropU;
    private ParticleSystem PropD;
    private ParticleSystem PropB;
    private ParticleSystem PropLRU;
    private ParticleSystem PropRRU;
    private ParticleSystem PropLRD;
    private ParticleSystem PropRRD;
    private ParticleSystem HiperSpeedTrail;
    private ParticleSystem SpeedBreakGlow;
    private Rigidbody rb;

    private GameObject CameraMain;
    private GameObject CameraFPP;


    private float _maxSpeed;
    private bool bPropL, bPropR, bPropC, bPropB, bPropU, bPropD, bPropLRU, bPropLRD, bPropRRU, bPropRRD, bHiperSpeedTrail;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = limiteVelocidadAngular;
        rb.maxDepenetrationVelocity = 5;

        dragOriginal = rb.drag;
        angDragOriginal = rb.angularDrag;

        PropL = transform.Find("PropL").GetComponent<ParticleSystem>();
        PropR = transform.Find("PropR").GetComponent<ParticleSystem>();
        PropC = transform.Find("PropC").GetComponent<ParticleSystem>();
        PropU = transform.Find("PropU").GetComponent<ParticleSystem>();
        PropD = transform.Find("PropD").GetComponent<ParticleSystem>();
        PropB = transform.Find("PropB").GetComponent<ParticleSystem>();
        PropLRU = transform.Find("PropLRU").GetComponent<ParticleSystem>();
        PropLRD = transform.Find("PropLRD").GetComponent<ParticleSystem>();
        PropRRU = transform.Find("PropRRU").GetComponent<ParticleSystem>();
        PropRRD = transform.Find("PropRRD").GetComponent<ParticleSystem>();
        HiperSpeedTrail = transform.Find("HiperSpeedTrail").GetComponent<ParticleSystem>();
        SpeedBreakGlow = transform.Find("Glow").GetComponent<ParticleSystem>();
        _maxSpeed = limiteVelocidad;

        CameraMain = transform.Find("Main Camera").gameObject;
        CameraFPP = transform.Find("Camera").gameObject;
    }

    private void Update()
    {
        Debug.Log(rb.angularDrag);
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CameraMain.activeSelf)
            {
                CameraFPP.SetActive(true);
                CameraMain.SetActive(false);
            }
            else
            {
                CameraFPP.SetActive(false);
                CameraMain.SetActive(true);
            }
        }

        //KILLROT
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivarKillRot();
        }

        //SPEEDBREAK
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivarSpeedBreak();
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    { 
        bPropL = bPropR = bPropC = bPropB = bPropU = bPropD = bPropLRU = bPropLRD = bPropRRU = bPropRRD = bHiperSpeedTrail = false;

        
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            ActivarHipervelocidad(true);
        }
        else
        {
            ActivarHipervelocidad(false);
        }

        ControlHipervelocidad();

        if (!hipervelocidad && !apagandoHipervelocidad && !speedBreak)
        {
            if (!killRot)
            {
                rb.angularDrag = angDragOriginal;

                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                {
                    rb.AddTorque(transform.right * fuerzaRotacion);
                    bPropD = true;
                }
                else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
                {
                    rb.AddTorque(transform.right * -fuerzaRotacion);
                    bPropU = true;
                }

                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    rb.AddTorque(transform.forward * fuerzaRotacion * 0.5f);
                    bPropLRD = bPropLRU = true;
                }
                else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                {
                    rb.AddTorque(transform.forward * -fuerzaRotacion * 0.5f);
                    bPropRRD = bPropRRU = true;
                }

                if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                {
                    rb.AddTorque(transform.up * -fuerzaRotacion);
                    bPropR = true;
                }
                else if (!Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                {
                    rb.AddTorque(transform.up * fuerzaRotacion);
                    bPropL = true;
                }
            }
            else
            {
                rb.angularDrag = 5f;
            }

            if (!Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(transform.forward * fuerza);
                bPropL = bPropR = true;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(transform.forward * -fuerzaTrasera);
                bPropB = true;
            }
        }

        

        CambiarEstadoEmitter(PropL, bPropL);
        CambiarEstadoEmitter(PropR, bPropR);
        CambiarEstadoEmitter(PropC, bPropC);
        CambiarEstadoEmitter(PropU, bPropU);
        CambiarEstadoEmitter(PropD, bPropD);
        CambiarEstadoEmitter(PropB, bPropB);
        CambiarEstadoEmitter(PropLRU, bPropLRU);
        CambiarEstadoEmitter(PropLRD, bPropLRD);
        CambiarEstadoEmitter(PropRRU, bPropRRU);
        CambiarEstadoEmitter(PropRRD, bPropRRD);
        CambiarEstadoEmitter(HiperSpeedTrail, bHiperSpeedTrail);
        CambiarEstadoEmitter(SpeedBreakGlow, speedBreak);

        if (CameraMain.activeSelf)
        {
            if (bHiperSpeedTrail)
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.CameraMotionBlur>().enabled = true;
            }
            else
            {
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityStandardAssets.ImageEffects.CameraMotionBlur>().enabled = false;
            }
        }
        


        rb.velocity = Vector3.ClampMagnitude(rb.velocity, _maxSpeed);
        if (rb.angularVelocity.magnitude < 0.01)
        {
            rb.angularVelocity = new Vector3();
        }
    }

    void CambiarEstadoEmitter(ParticleSystem pe, bool state)
    {
        if (state == true)
        {
            if (!pe.isPlaying)
            {
                pe.Play(true);
            }
        }
        else
        {
            if (!pe.isStopped)
            {
                pe.Stop(true);
            }
        }
    }

    void ActivarKillRot()
    {
        if (!killRot)
        {
            killRot = true;
            rb.angularDrag = 5f;
        }
        else
        {
            killRot = false;
            if (!hipervelocidad)
            {
                rb.angularDrag = angDragOriginal;
            }
        }
    }

    Estado EstadoKillRot()
    {
        if (killRot)
        {
            return Estado.Activado;
        }
        return Estado.Disponible;
    }

    void ActivarSpeedBreak()
    {
        //SPEEDBREAK
        if (!hipervelocidad && !apagandoHipervelocidad)
        {
            if (!speedBreak)
            {
                if (Time.time - speedBreakCD > speedBreakCDN)
                {
                    speedBreak = true;
                    rb.drag = 1f;
                }
            }
            else
            {
                speedBreakCD = Time.time;
                speedBreak = false;
                rb.drag = dragOriginal;
            }

        }
    }

    Estado EstadoSpeedBreak()
    {
        if (speedBreak)
        {
            return Estado.Activado;
        }
        else
        {
            if (Time.time - speedBreakCD > speedBreakCDN)
            {
                return Estado.Disponible;
            }
        }

        return Estado.Cooldown;
    }

    void ActivarHipervelocidad(bool press)
    {
        if (press)
        {
            if (!apagandoHipervelocidad && !speedBreak)
            {
                if (Time.time - cdHipervelocidad > cdnHipervelocidad)
                {
                    //INICIO HIPERVEL
                    hipervelocidad = true;
                    rb.angularDrag = 5f;
                    _maxSpeed = limiteHipervelocidad;
                }
            }
        }
        else if (hipervelocidad)
        {
            //FIN HIPERVEL
            hipervelocidad = false;
            apagandoHipervelocidad = true;

            if (!killRot)
            {
                rb.angularDrag = angDragOriginal;
            }

            rb.drag = 0.3f;
        }
    }

    Estado EstadoHipervelocidad()
    {
        if (hipervelocidad)
        {
            return Estado.Activado;
        }
        else
        {
            if (apagandoHipervelocidad)
            {
                return Estado.Apagando;
            }
            else if (Time.time - cdHipervelocidad > cdnHipervelocidad)
            {
                return Estado.Disponible;
            }
        }

        return Estado.Cooldown;
    }

    void ControlHipervelocidad()
    {
        if (hipervelocidad)
        {
            //DURANTE HIPERVEL
            rb.AddForce(transform.forward * fuerzaHipervelocidad);
            bPropL = bPropR = true;
            bPropC = true;
            bHiperSpeedTrail = true;
        }

        if (apagandoHipervelocidad)
        {
            bHiperSpeedTrail = true;
            //DURANTE APAGADO HIPERVEL
            if (rb.velocity.magnitude <= limiteVelocidad)
            {
                cdHipervelocidad = Time.time;
                apagandoHipervelocidad = false;
                _maxSpeed = limiteVelocidad;
                rb.drag = dragOriginal;
            }
        }
    }


    /*private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }*/
    private string EstadoToString(Estado ee)
    {
        switch (ee)
        {
            case Estado.Activado:
                return "Active";
            case Estado.Disponible:
                return "Available";
            case Estado.Apagando:
                return "Shutting down";
            case Estado.Cooldown:
                return "On cooldown";
        }
        return "Not available";
    }

    private void OnGUI()
    {
        GUI.color = Color.green;
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height * 0.1f), "Speed: " + (rb.velocity.magnitude).ToString("#0.00") + " m/s");
        GUI.Label(new Rect(0, 20, Screen.width, Screen.height * 0.1f), "AngularSpeed: " + rb.angularVelocity.magnitude.ToString("#0.00") + " rad/s");
        GUI.Label(new Rect(0, 40, Screen.width, Screen.height * 0.1f), "Hiperspeed: " + EstadoToString(EstadoHipervelocidad()) );
        GUI.Label(new Rect(0, 60, Screen.width, Screen.height * 0.1f), "Kill Rotation: " + EstadoToString(EstadoKillRot()));
        GUI.Label(new Rect(0, 80, Screen.width, Screen.height * 0.1f), "SpeedBreak: " + EstadoToString(EstadoSpeedBreak()));

        GUI.Label(new Rect(0, 150, Screen.width, Screen.height * 0.1f), "(X: " + transform.position.x.ToString("#0.00") + ", Y: " + transform.position.y.ToString("#0.00") + ", Z: " + transform.position.z.ToString("#0.00") + ")");
        GUI.color = Color.white;
    }
}
