using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Inventory Inventory;

    //Double tap
    private int TapCount;
    public float MaxDubbleTapTime;
    private float NewTime;

    void Start()
    {
        TapCount = 0;
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void Update()
    {
        DoubleTabControl();
    }

    public void DoubleTabControl()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TapCount += 1;
            }

            if (TapCount == 1)
            {

                NewTime = Time.time + MaxDubbleTapTime;
            }
            else if (TapCount == 2 && Time.time <= NewTime)
            {
                Vector3 mousePos = Input.mousePosition;
                Debug.Log("Mouse pos : " + mousePos);
                Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
                
                objectPos.z = -3;
                Debug.Log("Object pos : " + objectPos);
                if (Inventory.currentBrick != null)
                {
                    Instantiate(Inventory.currentBrick.prefab, objectPos, Quaternion.identity);
                }   
                
                TapCount = 0;
            }

        }
        if (Time.time > NewTime)
        {
            TapCount = 0;
        }
    }
}
