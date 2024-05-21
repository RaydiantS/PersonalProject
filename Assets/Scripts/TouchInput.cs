using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    List<float> tap = new List<float>();
    List<float> touch = new List<float>();
    List<float> flick = new List<float>();
    List<float> lastTouch = new List<float>();

    public GameObject lineDisplay;
    List<GameObject> touchPositions = new List<GameObject>();
    


    void Start()
    {
        
    }


    void Update()
    {
        bool removed;
        copyToLastTouch();

        getTouchInput();
        //for every tapNote to be judged in the judgelist...
        for(int i = 0; i < DataTransfer.tapJudgeList.Count; i++)
        {
            removed = false;
            Debug.Log("*****Before FOR, i-index: " + i);
            if (tap.Count != 0)
            {
                
                //for every tapInput in the taplist...
                for (int n = 0; n < tap.Count;)
                {
                    
                    Debug.Log("*****Before FOR, n-index: " + n);


                    //if the tapInput is judged true by the function
                    if (DataTransfer.tapJudgeList[i].judgeTap(tap[n]) == true)
                    {
                        //Debug.Log("********Before Removed, n " + n + "tap.count is " + tap.Count);

                        tap.Remove(tap[n]);
                        removed = true;



                        //Problem: Removing the tap judge might lead to wrong index references
                        //Possible solution: Manually increase index

                        //Debug.Log("********After Removed, n " + n + "tap.count is " + tap.Count);
                        //make a clone, iterate over clone, remove from original
                    }
                    else
                    {
                        n++;
                        
                    }
                    Debug.Log("*****After FOR, n-index: " + n);
                }
                Debug.Log("*****After FOR, i-index: " + i);

                if (removed)
                {
                    i--;
                }
            }

            
            else
            {
                break;
            }
            

        }

        for (int i = 0; i < DataTransfer.dragJudgeList.Count; i++)
        {
            //for every touchInput in the taplist...
            for (int n = 0; n < touch.Count; n++)
            {
                //if the touchInput is judged true by the function
                if (DataTransfer.dragJudgeList[i].judgeDrag(touch[n]) == true)
                {
                    touch.Remove(touch[n]);
                }
            }
        }

        for (int i = 0; i < DataTransfer.flickJudgeList.Count; i++)
        {
            //for every flickInput in the taplist...
            for (int n = 0; n < flick.Count; n++)
            {
                //if the flickInput is judged true by the function
                if (DataTransfer.flickJudgeList[i].judgeFlick(flick[n]) == true)
                {
                    flick.Remove(flick[n]);
                }
            }
        }

        for (int i = 0; i < DataTransfer.holdHeadJudgeList.Count; i++)
        {
            //for every tapInput in the taplist...
            for (int n = 0; n < tap.Count; n++)
            {
                //if the tapInput is judged true by the hold head function
                if (DataTransfer.holdHeadJudgeList[i].judgeHoldHead(tap[n]) == true)
                {
                    tap.Remove(tap[n]);
                }
            }
        }

        for (int i = 0; i < DataTransfer.holdMiddleJudgeList.Count; i++)
        {
            //for every tapInput in the taplist...
            for (int n = 0; n < touch.Count; n++)
            {
                //if the tapInput is judged true by the hold head function
                if (DataTransfer.holdMiddleJudgeList[i].judgeHoldMiddle(touch[n]) == true)
                {
                    touch.Remove(touch[n]);
                }
            }
        }

        //Display judging lines

        addDisplayLines();

        destroyExtraDisplayLines();
    }



    private void copyToLastTouch()
    {
        //clear data from last lastTouch before storing this lastTouch
        lastTouch.Clear();

        //store data in lastTouch from touch list
        foreach (float a in touch)
        {
            lastTouch.Add(a);
        }

        //clear data from touch, tap and flick lists to make space for new ones
        touch.Clear();
        tap.Clear();
        flick.Clear();
    }

    private void getTouchInput()
    {
        //get data for this touch
        foreach (Touch finger in Input.touches)
        {
            //send ray from where the finger is touching and detect collisions
            Ray ray = Camera.main.ScreenPointToRay(finger.position);
            RaycastHit hit; //returns a bool

            //Same with: Physics.Raycast(ray) == true
            if (Physics.Raycast(ray, out hit))
            {
                touch.Add(hit.point.x);

                if (finger.phase == TouchPhase.Began)
                {
                    tap.Add(hit.point.x);
                    //Debug.Log("Tap added! ");
                }

                //Add flick if the finger move faster than Flick Threshold
                Vector2 touchDeltaPosition = finger.deltaPosition;
                float touchSpeed = touchDeltaPosition.magnitude / finger.deltaTime;
                float flickThres = 2000f;
                if(touchSpeed > flickThres)
                {
                    flick.Add(hit.point.x);
                    //Debug.Log("Flick added! ");
                }
            }

        }
    }

    private void getMouseInput()
    {
        //send ray from where the mouse is pointing and detect collisions
        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;

        //Same with: Physics.Raycast(ray) == true
        if (Physics.Raycast(ray1, out hit1))
        {
            if (Input.GetMouseButton(0))
            {
                touch.Add(hit1.point.x);
                //Debug.Log("Touched");
            }


            if (Input.GetMouseButtonDown(0))
            {
                tap.Add(hit1.point.x);
                //Debug.Log("Tapped");
            }

            //if lastTouch has the same hit as this touch, and if the input moved
            if (lastTouch.Contains(hit1.point.x) && Input.GetMouseButton(0))
            {
                flick.Add(hit1.point.x);
                //Debug.Log("Flicked");
            }
        }
    }

    private void addDisplayLines()
    {
        //for loop going through all the existing touches
        for (int i = 0; i < touch.Count; i++)
        {
            if (i == touchPositions.Count)
            {
                /*
                 * About the condition: 
                 * if the amount of touches (i + 1) is more than the lines stored in touchPositions...
                 * then add a line to touchPositions. 
                 * when i == touchPositions.Count == 2, the condition is fulfilled
                 * Note that i starts at 0, so if "i" caps at 2 there are actually 3 touches
                 */
                GameObject line = Instantiate(lineDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                touchPositions.Add(line);
                //Debug.Log("Line Added! ");
            }

            //get the position of the line using its number
            Vector3 pos = touchPositions[i].transform.position;
            //change the x value of the line using the float value of the touch input with the same number as the line
            pos.x = touch[i];
            touchPositions[i].transform.position = pos;
        }
    }

    private void destroyExtraDisplayLines()
    {
        //if the number of touches decreased, delete extra lines
        //Since this is a list, the "indexes" are immediately adjusted afterwards and the existing lines are renumbered
        for (int i = touch.Count; i < touchPositions.Count;)
        {
            Destroy(touchPositions[i]);
            touchPositions.RemoveAt(i);
            //Debug.Log("Line Deleted! ");
        }
    }
}

