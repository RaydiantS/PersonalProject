using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGoDown : MonoBehaviour
{
    public float noteSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0, -37.5f * noteSpeed) * Time.deltaTime;
    }
}
