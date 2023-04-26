using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbitro : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider ball)
    {
        if (ball.gameObject.tag == "Pelota")
        {
            ball.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            ball.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.gameObject.GetComponent<Rigidbody>().transform.localPosition = new Vector3(0, 5.0f, 0);
        }
    }
}
