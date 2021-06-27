using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    //Gold variable will be kept in here
    public int gold = 50;
    [SerializeField] private Text goldText;
    void Start()
    {
        gold = 50; //Just for practice
    }

    // Update is called once per frame
    void Update()
    {
        //Change gold text
        goldText.text = gold.ToString();
    }
}
