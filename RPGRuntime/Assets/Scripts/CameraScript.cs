using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AssignTarget(GameObject targ)
    {
        target = targ;
    }

    // Update is called once per frame. Get the target and move the camera there.
    void FixedUpdate()
    {
        GameObject trans = this.gameObject;
        if(target != null)
            trans.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
    }
}
