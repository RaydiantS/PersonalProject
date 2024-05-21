using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectAndScore : MonoBehaviour
{
    public TapScript tapScript;
    public GameController gameController;
    public TextMeshProUGUI score, combo, missCount, perfectCount, lazyText;
    public float relativeScore;
    public float absoluteScore;
    public int comboCount, missCounts, perfectCounts;
    float currentScore;
    public ParticleSystem effect;

    // Start is called before the first frame update
    void Start()
    {
        missCounts = 0;
        perfectCounts = 0;
        lazyText.enabled = false;
        DataTransfer.controller = GetComponent<GameController>();
        tapScript = GetComponent<TapScript>();
        gameController = GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(relativeScore + " / " + absoluteScore);
        currentScore = (relativeScore / absoluteScore) * 10000000;
        //Debug.Log("CURRENTSCORE" + currentScore);
        int currentScoreInt = Convert.ToInt32(currentScore);
        score.text = Convert.ToString(currentScoreInt);
        combo.text = Convert.ToString(comboCount);
        missCount.text = "Miss: " + missCounts;
        perfectCount.text = "Perfect: " + perfectCounts;

        if(missCounts + perfectCounts >= 872)
        {
            lazyText.enabled = true;
        }
    }
}
