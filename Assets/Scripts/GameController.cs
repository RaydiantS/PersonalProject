using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int notec = 0;
    public TextAsset chartFile;
    int[] noteQuantity;
    float[] timeStamps;
    List<float> noteType = new List<float>(), notePosition = new List<float>(), noteHoldTime = new List<float>();
    public AudioSource music;
    float timer = -1f;
    int indexOfNote = 0;
    public GameObject tapNote, dragNote, flickNote;
    private TempGoDown tempGoDownScript;
    private float generateFactor;

    // Start is called before the first frame update
    void Start()
    {
        tempGoDownScript = tapNote.GetComponent<TempGoDown>();
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
                //Debug.Log(timeStamps[i]);
            }

            if (int.TryParse(eachLineSplitTimePart[1], out int NoteQuValue))
            {
                noteQuantity[i] = NoteQuValue;
                //Debug.Log(noteQuantity[i]);
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
            
            ////Convert eachLineSplit[1] from string to float and store it in timeStamps[i]
            //if (float.TryParse(eachLineSplit[0], out float noteTypeValue))
            //{
            //    noteType[i] = noteTypeValue;
            //    //Debug.Log(timeStamps[i]);
            //}

            ////Convert eachLineSplit[1] from string to float and store it in timeStamps[i]
            //if (float.TryParse(eachLineSplit[1], out float timeStValue))
            //{
            //    timeStamps[i] = timeStValue;
            //    //Debug.Log(timeStamps[i]);
            //}

            ////Convert eachLineSplit[2] from string to float and store it in timeStamps[i]
            //if (float.TryParse(eachLineSplit[2], out float posValue))
            //{
            //    notePositions[i] = posValue;
            //    //Debug.Log(notePositions[i]);
            //}
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
                    Debug.Log("index of note: " + indexOfNote);
                    Debug.Log("note quantity: " + noteQuantity[indexOfNote]);
                    switch (noteType[indexOfNote])
                    {
                        case 0:
                            Instantiate(tapNote, new Vector3(notePosition[indexOfNote], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            indexOfNote++;
                            break;
                        case 1:
                            Instantiate(dragNote, new Vector3(notePosition[indexOfNote], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            indexOfNote++;
                            break;
                        case 2:
                            Instantiate(flickNote, new Vector3(notePosition[indexOfNote], 0, 17.5f + 3.75f * generateFactor), Quaternion.identity);
                            indexOfNote++;
                            break;
                        case 3:
                            DataTransfer.holdTime = noteHoldTime[indexOfNote];
                            noteHoldTime.RemoveAt(indexOfNote);
                            indexOfNote++;
                            break;
                    }
                    noteType.RemoveAt(indexOfNote);
                    notePosition.RemoveAt(indexOfNote);
                }

            }
        }
        //if timer is larger than equal to the timestamp for index-th note

    }
}
