using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{
    float timer = -1f;
    public EffectAndScore effectAndScore;

    //Make sure "add" and "remove" happens once for each note
    bool notAdded = true, notRemoved = true, notRight = true;

    void Start()
    {
        effectAndScore = GameObject.Find("GameController").GetComponent<EffectAndScore>();
    }


    void Update()
    {
        timer += Time.deltaTime;

        //if in the judgement range (+-50ms), add yourself to judge list for judgement
        if (notAdded & timer > -0.08f)
        {
            DataTransfer.dragJudgeList.Add(this);
            notAdded = false;
        }
       
        else if (notRemoved && timer > 0.08f)
        {
            DataTransfer.dragJudgeList.Remove(this);
            notRemoved = false;
            Miss();
        }
    }

    public bool judgeDrag(float xTouchedPosition)
    {
        //get the abs value of the x-difference between where the player tapped and the center of the dragNote
        float x = System.Math.Abs(xTouchedPosition - transform.position.x);

        //if the difference "x" is too large
        if (x <= 2.7)
        {
            //generate effects & calculate score
            effectAndScore.relativeScore++;
            effectAndScore.comboCount++;
            Vector3 particleTransform = effectAndScore.effect.transform.position;
            particleTransform.x = transform.position.x;
            effectAndScore.effect.transform.position = particleTransform;
            effectAndScore.effect.Play();

            //remove from judgeList and playing screen since it's finished
            DataTransfer.dragJudgeList.Remove(this);
            effectAndScore.perfectCounts++;
            Destroy(gameObject);
            return true;
        }
        return false;

    }

    void Miss()
    {
        //generate effects & calculate combo
        effectAndScore.comboCount = 0;
        effectAndScore.missCounts++;
        Debug.Log("Miss Drag! ");
        Destroy(gameObject);
    }
}
