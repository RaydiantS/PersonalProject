using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickScript : MonoBehaviour
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
        if (notAdded & timer > -0.05f)
        {
            DataTransfer.flickJudgeList.Add(this);
            notAdded = false;
        }

        else if (notRemoved && timer > 0.05f)
        {
            DataTransfer.flickJudgeList.Remove(this);
            notRemoved = false;
            Miss();
        }
    }

    public bool judgeFlick(float xFlickedPosition)
    {
        //get the abs value of the x-difference between where the player tapped and the center of the flickNote
        float x = System.Math.Abs(xFlickedPosition - transform.position.x);

        //if the difference "x" is too large
        if (x <= 2.7)
        {
            //generate effects & calculate score

            //remove from judgeList and playing screen since it's finished
            DataTransfer.flickJudgeList.Remove(this);
            Debug.Log("Perfect Flick! ");
            Destroy(gameObject);
            return true;
        }
        return false;

    }

    void Miss()
    {
        //generate effects & calculate score
        Debug.Log("Miss Flick! ");
        Destroy(gameObject);
    }
}
