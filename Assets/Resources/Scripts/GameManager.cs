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

    private Vector3 touchStart;
    public Camera cam;


    void Start()
    {
        TapCount = 0;
        Application.targetFrameRate = 300;
    }

    // Update is called once per frame
    void Update()
    {
        DoubleTabControl();
        PanningControl();
    }

    private void PanningControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(0);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(0);
            cam.transform.position += direction;
        }
    }

    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
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
                mousePos.z = 20;
                Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
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
