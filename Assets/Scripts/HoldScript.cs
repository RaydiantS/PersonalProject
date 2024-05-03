using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldScript : MonoBehaviour
{
    float timer = -1f;

    //Make sure "add" and "remove" happens once for each note
    float holdTime, holdingTime = 0f;
    bool notAdded = true, notRemoved = true, notRight = true, holding;
    SpriteRenderer holdSprite;

    void OnEnable()
    {
        holdSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        holdTime = DataTransfer.holdTime;

        //locate the hold
        transform.position = new Vector3(1, 1, holdTime * 18.25f);

        //drop the hold
        transform.Translate(0, 0, holdTime * -18.25f);
    }


    void Update()
    {
        timer += Time.deltaTime;

        //if in the judgement range (+-50ms), add yourself to judge list for judgement
        if (notAdded & timer > -0.05f)
        {
            DataTransfer.holdHeadJudgeList.Add(this);
            notAdded = false;
        }
        //else if(notRight && timer >= 0f)
        //{
        //    Debug.Log("Just right timing at " + transform.position.z);
        //    notRight = false;
        //}
        else if (notRemoved && timer > 0.05f)
        {
            DataTransfer.holdHeadJudgeList.Remove(this);
            notRemoved = false;
            Miss();
        }
    }

    public bool judgeHoldHead(float xHoldHeadPosition)
    {
        //get the abs value of the x-difference between where the player tapped and the center of the tapNote
        float x = System.Math.Abs(xHoldHeadPosition - transform.position.x);

        //if the difference "x" is too large
        if (x <= 2.7)
        {
            //generate effects & calculate score

            //remove from judgeList and playing screen since it's finished
            //to prevent removal after head judge
            notRemoved = false;
            Debug.Log("Perfect Hold Head! ");
            DataTransfer.holdHeadJudgeList.Remove(this);
            DataTransfer.holdMiddleJudgeList.Add(this);
            holding = true;
            StartCoroutine("HoldingTimer");
            return true;
        }
        return false;

    }

    public bool judgeHoldMiddle(float xHoldMiddlePosition)
    {
        //get the abs value of the x-difference between where the player tapped and the center of the tapNote
        float x = System.Math.Abs(xHoldMiddlePosition - transform.position.x);

        //if the difference "x" is too large
        if (x <= 2.7)
        {
            holding = true;
            return true;
        }
        return false;
    }

    void Miss()
    {
        //generate effects & calculate score
        holdSprite.color = new Color(1, 1, 1, 0.5f);
        DataTransfer.holdHeadJudgeList.Remove(this);
        DataTransfer.holdMiddleJudgeList.Remove(this);
        Debug.Log("Miss Hold! ");
        Destroy(gameObject);
    }

    IEnumerator HoldingTimer()
    {
        while (holding)
        {
            holdingTime += Time.deltaTime;
            holding = false;
            transform.position = new Vector3(1, 1, (holdTime - holdingTime) * 18.25f);
            transform.Translate(0, 0, -18.25f * Time.deltaTime);
            if(holdingTime > holdTime)
            {
                DataTransfer.holdMiddleJudgeList.Remove(this);
                Debug.Log("Perfect Hold! ");
                //generate effects and calculate score

                Destroy(gameObject);
                break;
            }
            yield return 0; //stop running the coroutine for this frame, and start again next frame
        }

        if(holdingTime < holdTime)
        {
            Miss();
        }
    }
}
