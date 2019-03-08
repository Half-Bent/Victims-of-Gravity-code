using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/// <summary>
/// Pelaajan värin valinnalle
/// </summary>
public class MenuPlayerColorSelect : MonoBehaviour {

    public XboxController CurrentController = XboxController.Any;
    public int MenyPlayerID;
    public bool Locked
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        Locked = false;
    }

    public void SetControllerID(XboxController controllerID)
    {
        this.CurrentController = controllerID;
        Debug.Log(controllerID.ToString());
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Locked && XCI.GetButton(XboxButton.B, CurrentController))
        {
            Locked = false;
        }

        if(XCI.GetButton(XboxButton.A, CurrentController))
        {
            Locked = true;
        }

	}
}
