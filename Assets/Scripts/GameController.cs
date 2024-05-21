using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextAsset chartFile;
    int[] noteQuantity;
    float[] timeStamps;
    List<float> noteType = new List<float>(), notePosition = new List<float>(), noteHoldTime = new List<float>();
    public AudioSource music;
    float timer = -1f - 0.3429f - 0.07f;
    int indexOfNote = 0;
    public GameObject tapNote, dragNote, flickNote, holdNote;
    private TempGoDown tempGoDownScript;
    private float generateFactor;
    int calculateAbsScore = 0;
    bool calculated = false;
    private EffectAndScore effectAndScore;

    // Start is called before the first frame update
    void Start()
    {
        
        tempGoDownScript = tapNote.GetComponent<TempGoDown>();
        effectAndScore = GameObject.Find("GameController").GetComponent<EffectAndScore>();
        generateFactor = (tempGoDownScript.noteSpeed - 1) * 10;
        LoadChart();
        music.Stop();
        StartCoroutine("GameStart");
    }

    // Update is called once per frame
    void Update()
    {
        GenerateChart();
    }


    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2f);
        music.Play();
    }

    void LoadChart()
    {
        //split the chart file according to line
        string[] eachLine = chartFile.text.Split('\n');

        //leave enough space of eachLine.Length for the three arrays
        timeStamps = new float[eachLine.Length];
        noteQuantity = new int[eachLine.Length];

        //calculate absolute score in effects and score
        

        for (int i = 0; i < eachLine.Length; i++)
        {

            //Split each line into a two-indexed array [timestamps, position]
            //new: [timestamps, noteCount; note data \n note data]
            string[] eachLineSplit = eachLine[i].Split(';');
            Debug.Log(eachLineSplit.Length);
            string[] eachLineSplitTimePart = eachLineSplit[0].Split(",");
            string[] eachLineSplitNotePart = eachLineSplit[1].Split("/");

            if (float.TryParse(eachLineSplitTimePart[0], out float timeStValue))
            {
                timeStamps[i] = timeStValue;
            }

            if (int.TryParse(eachLineSplitTimePart[1], out int NoteQuValue))
            {
                noteQuantity[i] = (NoteQuValue);
            }

            for(int n = 0; n < eachLineSplitNotePart.Length; n++)
            {
                string[] singleNoteData = eachLineSplitNotePart[n].Split(",");
                if (singleNoteData.Length == 2)
                {
                    noteType.Add(System.Convert.ToSingle(singleNoteData[0]));
                    notePosition.Add(System.Convert.ToSingle(singleNoteData[1]));
                }
                else if (singleNoteData.Length == 3)
                {
                    noteType.Add(System.Convert.ToSingle(singleNoteData[0]));
                    notePosition.Add(System.Convert.ToSingle(singleNoteData[1]));
                    noteHoldTime.Add(System.Convert.ToSingle(singleNoteData[2]));
                }
            }
        }

        if (!calculated)
        {
            for (int s = 0; s < noteQuantity.Length; s++)
            {
                calculateAbsScore += noteQuantity[s];
                Debug.Log("*1*1*1*1*1*1*1*1*    " + noteQuantity[s]);
            }
            Debug.Log("*2*2*2*2*2*2*2*2*    " + calculateAbsScore);
            effectAndScore.absoluteScore = calculateAbsScore;
            calculated = true;
        }


    }

    void GenerateChart()
    {
        //refresh timer 
        timer += Time.deltaTime;

        
        if(indexOfNote < timeStamps.Length)
        {
            if (timer >= timeStamps[indexOfNote])
            {
                for (int i = noteQuantity[indexOfNote] - 1; i >= 0; i--)
                {
                    switch (noteType[0])
                    {
                        case 0:
                            Instantiate(tapNote, new Vector3(notePosition[0], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            break;
                        case 1:
                            Instantiate(dragNote, new Vector3(notePosition[0], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            break;
                        case 2:
                            Instantiate(flickNote, new Vector3(notePosition[0], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            break;
                        case 3:
                            DataTransfer.holdTime = noteHoldTime[0];
                            Instantiate(holdNote, new Vector3(notePosition[0], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            noteHoldTime.RemoveAt(0);
                            break;
                    }
                    
                    noteType.RemoveAt(0);
                    notePosition.RemoveAt(0);

                }

                indexOfNote++;
            }
            
        }
        //if timer is larger than equal to the timestamp for index-th note

    }
}


/*
68.886, 1; 0,0
68.907, 1; 0,0
69.229, 1; 0,0
69.250, 1; 0,0
69.572, 1; 0,0
69.593, 1; 0,0
69.915, 1; 0,0
69.936, 1; 0,0
70.258, 1; 0,0
70.279, 1; 0,0
70.601, 1; 0,0
70.622, 1; 0,0
70.944, 1; 0,0
70.965, 1; 0,0
71.286, 1; 0,0
71.307, 1; 0,0
71.629, 1; 0,0
71.650, 1; 0,0
71.972, 1; 0,0
71.993, 1; 0,0
72.315, 1; 0,0
72.336, 1; 0,0
72.658, 1; 0,0
72.679, 1; 0,0
73.001, 1; 0,0
73.022, 1; 0,0
73.344, 1; 0,0
73.365, 1; 0,0
73.687, 1; 0,0
73.708, 1; 0,0
74.030, 1; 0,0
74.051, 1; 0,0
74.373, 1; 0,0
74.394, 1; 0,0
74.715, 1; 0,0
74.736, 1; 0,0
75.058, 1; 0,0
75.079, 1; 0,0
75.401, 1; 0,0
75.422, 1; 0,0
75.744, 1; 0,0
75.765, 1; 0,0
76.087, 1; 0,0
76.108, 1; 0,0
76.430, 1; 0,0
76.451, 1; 0,0
76.773, 1; 0,0
76.794, 1; 0,0
77.116, 1; 0,0
77.137, 1; 0,0
77.459, 1; 0,0
77.480, 1; 0,0
77.802, 1; 0,0
77.823, 1; 0,0
78.144, 1; 0,0
78.165, 1; 0,0
78.487, 1; 0,0
78.508, 1; 0,0
78.830, 1; 0,0
78.851, 1; 0,0
79.173, 1; 0,0
79.194, 1; 0,0
79.516, 1; 0,0
79.537, 1; 0,0
79.859, 1; 0,0
79.880, 1; 0,0
80.202, 1; 0,0
80.223, 1; 0,0
80.545, 1; 0,0
80.566, 1; 0,0
80.888, 1; 0,0
80.909, 1; 0,0
81.231, 1; 0,0
81.252, 1; 0,0
81.573, 1; 0,0
81.594, 1; 0,0
81.916, 1; 0,0
81.937, 1; 0,0
82.259, 1; 0,0
82.280, 1; 0,0
82.602, 1; 0,0
82.623, 1; 0,0
82.945, 1; 0,0
82.966, 1; 0,0
83.288, 1; 0,0
83.309, 1; 0,0
83.631, 1; 0,0
83.652, 1; 0,0
83.974, 1; 0,0
83.995, 1; 0,0
84.317, 1; 0,0
84.338, 1; 0,0
84.659, 1; 0,0
84.681, 1; 0,0
85.002, 1; 0,0
85.023, 1; 0,0
85.345, 1; 0,0
85.366, 1; 0,0
85.688, 1; 0,0
85.709, 1; 0,0
86.031, 1; 0,0
86.052, 1; 0,0
86.374, 1; 0,0
86.395, 1; 0,0
86.717, 1; 0,0
86.738, 1; 0,0
87.060, 1; 0,0
87.081, 1; 0,0
87.403, 1; 0,0
87.424, 1; 0,0*/