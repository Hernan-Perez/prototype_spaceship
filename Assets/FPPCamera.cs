using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPPCamera : MonoBehaviour {

    private Texture2D mira;

    // Use this for initialization
    void Start () {
        mira = Resources.Load("Images/mira") as Texture2D;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        GUI.color = new Color(0.2f, 1f, 0.2f, 0.5f);
        GUI.DrawTexture(new Rect(Screen.width * 0.5f - 8, Screen.height * 0.5f - 8, 16, 16), mira);
        GUI.color = Color.white;
    }
}
