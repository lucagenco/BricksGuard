using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public int BallCount;
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Ball")
        {
            BallCount++;
            Destroy(collider.gameObject);
        }
    }
}
