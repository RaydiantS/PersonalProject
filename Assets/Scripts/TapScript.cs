using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MonoBehaviour
{
    float timer = -1f;

    //Make sure "add" and "remove" happens once for each note
    bool notAdded = true, notRemoved = true, notRight = true;

    void Start()
    {
        
    }


    void Update()
    {
        timer += Time.deltaTime;

        //if in the judgement range (+-100ms), add yourself to judge list for judgement
        if(notAdded & timer > -0.15f)
        {
            DataTransfer.tapJudgeList.Add(this);
            //Debug.Log(" ******* tap add/remove " + this + "added, count = " + DataTransfer.tapJudgeList.Count);
            notAdded = false;
        }
        //else if(notRight && timer >= 0f)
        //{
        //    Debug.Log("Just right timing at " + transform.position.z);
        //    notRight = false;
        //}
        else if (notRemoved && timer > 0.15f)
        {
            DataTransfer.tapJudgeList.Remove(this);
           // Debug.Log(" ******* tap add/remove " + this + "removed, count = " + DataTransfer.tapJudgeList.Count);
            notRemoved = false;
            Miss();
        }
    }

    public bool judgeTap(float xTapPosition)
    {
        //Debug.Log("*****1***** judgeTap this = " + this);
        //get the abs value of the x-difference between where the player tapped and the center of the tapNote
       // Debug.Log(" ********** 1.5 ********* xPos = " + xTapPosition);
        float x = System.Math.Abs(xTapPosition - transform.position.x);
        //Debug.Log("*****2*****");
        //if the difference "x" is too large
        if (x <= 2.7)
        {
            //generate effects & calculate score

            //remove from judgeList and playing screen since it's finished
            DataTransfer.tapJudgeList.Remove(this);
            Debug.Log("Perfect Tap! At" + transform.position.x);
            Destroy(gameObject);
            //Debug.Log("*****3*****");
            return true;
        }
        //Debug.Log("*****4*****");
        return false;

    }

    void Miss()
    {
        //generate effects & calculate score
        Debug.Log("Miss Tap! At " + transform.position.x);
        Destroy(gameObject);
    }
}
