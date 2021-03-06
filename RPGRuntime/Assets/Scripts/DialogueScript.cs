using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    public GameObject speaker;
    public string text;
    public double speed;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(speaker.transform.position.x +0.0f, speaker.transform.position.y+0.25f, transform.position.z);
        if(speed == 0)
            speed = 2f;
        this.GetComponent<TextMeshProUGUI>().text = text;
    }

    // FiexedUpdate is called once every 60th of a second
    void FixedUpdate()
    {
        speed = speed - (1f/60f);
        if(speed < 0){
            Destroy(this.gameObject);
        }
    }

    //Make the dialogue script proceed to the next script
    void OnDestroy(){
       
    }
}
