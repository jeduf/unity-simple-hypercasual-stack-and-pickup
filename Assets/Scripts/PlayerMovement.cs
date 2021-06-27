using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Speed Variables")]
    [SerializeField] private float runSpeed = 3;
    [SerializeField] private float changeLaneSpeed = 3;
    void Start()
    {
        //Start running
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,runSpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        //Clamp x position to prevent leaving running path
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2, 2), transform.position.y, transform.position.z);

        //A&D to change lanes
        if(Input.GetKey("a"))
        {
            if(transform.position.x > -2)
                transform.Translate((Time.deltaTime * -changeLaneSpeed),0,0);
        }
        if(Input.GetKey("d"))
        {
            if(transform.position.x < 2)
                transform.Translate((Time.deltaTime * changeLaneSpeed),0,0);
        }
        
    }
}
