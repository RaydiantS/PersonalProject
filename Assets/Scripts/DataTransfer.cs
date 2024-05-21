using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//every script can access this script
public static class DataTransfer
{
    //list for tap judgement
    public static float holdTime;
    public static GameController controller;
    public static List<TapScript> tapJudgeList = new List<TapScript>();
    public static List<DragScript> dragJudgeList = new List<DragScript>();
    public static List<FlickScript> flickJudgeList = new List<FlickScript>();
    public static List<HoldScript> holdHeadJudgeList = new List<HoldScript>();
    public static List<HoldScript> holdMiddleJudgeList = new List<HoldScript>()
;
}
