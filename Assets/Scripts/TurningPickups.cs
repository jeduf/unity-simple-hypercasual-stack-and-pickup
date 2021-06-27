using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPickups : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float turningSpeed = 50;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (0,turningSpeed*Time.deltaTime,0);
        
    }
}
