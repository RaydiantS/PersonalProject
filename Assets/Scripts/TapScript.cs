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

        //if in the judgement range (+-50ms), add yourself to judge list for judgement
        if(notAdded & timer > -0.05f)
        {
            DataTransfer.tapJudgeList.Add(this);
            notAdded = false;
        }
        //else if(notRight && timer >= 0f)
        //{
        //    Debug.Log("Just right timing at " + transform.position.z);
        //    notRight = false;
        //}
        else if (notRemoved && timer > 0.05f)
        {
            DataTransfer.tapJudgeList.Remove(this);
            notRemoved = false;
            Miss();
        }
    }

    public bool judgeTap(float xTapPosition)
    {
        //get the abs value of the x-difference between where the player tapped and the center of the tapNote
        float x = System.Math.Abs(xTapPosition - transform.position.x);

        //if the difference "x" is too large
        if(x <= 2.7)
        {
            //generate effects & calculate score

            //remove from judgeList and playing screen since it's finished
            DataTransfer.tapJudgeList.Remove(this);
            Debug.Log("Perfect Tap! ");
            Destroy(gameObject);
            return true;
        }
            return false;

    }

    void Miss()
    {
        //generate effects & calculate score
        Debug.Log("Miss Tap! ");
        Destroy(gameObject);
    }
}
