using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGObject : MonoBehaviour
{
    protected bool locked = false;
    public void Lock(bool tf){
        Debug.Log(tf);
        this.locked = tf;
        GameObject.FindGameObjectWithTag("Manager").GetComponent<CommandRouter>().Nextstep();
    }
}
