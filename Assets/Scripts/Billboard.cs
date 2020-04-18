using UnityEngine;

public class Billboard: MonoBehaviour {

    public Transform cameraTransform;
    private Transform myTransform;
    
    public bool alignNotLook = true;

    // Use this for initialization
    void Start () {
        myTransform = this.transform;
        cameraTransform = Camera.main.transform;
    }
	
    // Update is called once per frame
    void LateUpdate () {
        if (alignNotLook)
            myTransform.forward = cameraTransform.forward;
        else
            myTransform.LookAt (cameraTransform, Vector3.up);
    }
}