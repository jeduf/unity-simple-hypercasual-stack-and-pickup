using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackUnderChar : MonoBehaviour
{

    [Header("Positions")]    
    [SerializeField] private GameObject stackPos; //Stack position to keep objects under character

    [Header("Stacked Object Counter")]
    public int StackedObjectNum = 0; //Number of stacked objects (no use yet)
    [Header("Destroy Stacked Object")]
    private List<GameObject> stackedObjectList = new List<GameObject>(); //List to add all stacked objects
    [Header("Lose Game Screen")]
    [SerializeField] private Text LoseText;
    private int CharPutDownNum;

    void OnTriggerEnter(Collider col)
    {
        //Stack either hand or under character based on stackOrPickup bool
        if(col.gameObject.tag  == "Stack")
        {
            StackedObjectNum++;
            //Add object to list
            stackedObjectList.Add(col.gameObject);
            //Set parent to predefined position
            col.gameObject.transform.SetParent(stackPos.transform);
            //Change tag to prevent unwanted collisions
            col.gameObject.tag = "Untagged";
            //Different positioning based on stack or pickup
            transform.position = new Vector3(transform.position.x, transform.position.y + col.gameObject.transform.localScale.y, transform.position.z);
            col.gameObject.transform.localPosition = new Vector3 (0,-col.gameObject.transform.localScale.y * StackedObjectNum,0);
            
        }
        //Stack block, will destroy predefined number of objects if pickup. If stack, its gonna take the height of the block and will make the calculation to find how many object to destroy.
        if(col.gameObject.tag  == "StackBlock")
        {
            col.gameObject.tag = "Untagged";
            int i = 0;
            try
            {
                int a = Mathf.RoundToInt(1 / stackedObjectList[0].gameObject.transform.localScale.y);
                i = Mathf.RoundToInt(col.gameObject.transform.localScale.y * a);
            }
            catch (ArgumentOutOfRangeException ex) 
            {
                LoseGame();
            }
            CharPutDownNum = i;
            StartCoroutine(PutDownChar());

            while(0 < i)
            {
                try
                {
                    i--;
                    Destroy(stackedObjectList[stackedObjectList.Count - 1]);
                    stackedObjectList.RemoveAt(stackedObjectList.Count - 1);
                }
                catch (ArgumentOutOfRangeException ex) 
                {
                    LoseGame();
                }
            }
                
            
        }

    }
    IEnumerator PutDownChar()
    {
        yield return new WaitForSeconds(0.4f);
        try 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - (stackedObjectList[0].gameObject.transform.localScale.y) * CharPutDownNum, transform.position.z);
        }       
        catch (ArgumentOutOfRangeException ex) 
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
    void LoseGame()
    {
        LoseText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
