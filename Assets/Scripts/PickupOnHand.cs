using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupOnHand : MonoBehaviour
{
    [Header("Pickup Variables")]
    [SerializeField] private int goldPickup = 5; //How much gold will come from coin pickups
    [SerializeField] private int blockHitGold = 5; //How much will be gone if player hit blocks
    private Gold goldComp; //Gold component
    [Header("Positions")]
    [SerializeField] private GameObject pickupPos; //Stack position to keep objects on hand
    [Header("Stacked Object Counter")]
    public int PickedupObjectNum = 0; //Number of stacked objects (no use yet)
    [Header("Destroy Stacked Object")]
    [SerializeField] private int numberOfPickupsToDestroy = 1; //Basic variable to destroy objects when hit a block (might be implemented on block objects to change variable individually)
    private List<GameObject> PickedupObjectNumList = new List<GameObject>(); //List to add all stacked objects
    public bool isGrounded = true;
    [Header("Lose Game Screen")]
    [SerializeField] private Text LoseText;
    private bool disableWaitFirstTime = true;
    void Start()
    {
        goldComp = GameObject.FindGameObjectWithTag("GameController").GetComponent<Gold>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goldComp.gold <= 0)
        {
            LoseGame();
        }
    }
    void OnTriggerEnter(Collider col)
    {
        //Pickup gold
        if(col.gameObject.tag == "Gold")
        {
            goldComp.gold += goldPickup;
            Destroy(col.gameObject);
        }
        //Gold block (will drop gold)
        if(col.gameObject.tag  == "Block")
        {
            goldComp.gold = goldComp.gold - blockHitGold;
            Destroy(col.gameObject);
        }
        //Stack either hand or under character based on stackOrPickup bool
        if(col.gameObject.tag  == "Pickup")
        {
            PickedupObjectNum++;
            //Add object to list
            PickedupObjectNumList.Add(col.gameObject);
            //Set parent to predefined position
            col.gameObject.transform.SetParent(pickupPos.transform);
            //Change tag to prevent unwanted collisions
            col.gameObject.tag = "Untagged";
            //Different positioning based on stack or pickup
            col.gameObject.transform.localPosition = new Vector3 (0,(col.gameObject.transform.localScale.y + 0.02f) * PickedupObjectNum,0);
            
        }
        //Stack block, will destroy predefined number of objects if pickup. If stack, its gonna take the height of the block and will make the calculation to find how many object to destroy.
        if(col.gameObject.tag  == "PickupBlock")
        {
            col.gameObject.tag = "Untagged";
            int i = 0;
            i = numberOfPickupsToDestroy;
            PickedupObjectNum -= numberOfPickupsToDestroy;
            while(0 < i)
            {
                i--;
                try 
                {
                    Destroy(PickedupObjectNumList[PickedupObjectNumList.Count - 1]);
                    PickedupObjectNumList.RemoveAt(PickedupObjectNumList.Count - 1);
                }       
                catch (ArgumentOutOfRangeException ex) 
                {
                    LoseGame();
                }
            }
                
            
        }

    }
    void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Floor")
            isGrounded = true;
            disableWaitFirstTime = true;
    }
    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.tag == "Floor")
        {
            isGrounded = false;
            StartCoroutine(PutDownObjects());
        }
            
    }
    IEnumerator PutDownObjects()
    {
        if(!disableWaitFirstTime)
            yield return new WaitForSeconds(0.2f);
        disableWaitFirstTime = false;
        if(!isGrounded)
        {
            try 
            {
                GameObject obj = PickedupObjectNumList[PickedupObjectNumList.Count - 1];
                obj.transform.parent = null;
                obj.transform.localPosition = new Vector3 (obj.transform.localPosition.x,0,pickupPos.transform.position.z);
                PickedupObjectNumList.RemoveAt(PickedupObjectNumList.Count - 1);
                StartCoroutine(PutDownObjects());
            }       
            catch (ArgumentOutOfRangeException ex) 
            {
                LoseGame();
            }
            
        } 
    }
    void LoseGame()
    {
        LoseText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
