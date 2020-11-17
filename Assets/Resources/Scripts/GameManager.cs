using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    //Data
    public int TargetFps;
    public Inventory Inventory;
    public House house;
    public GameObject[] particles;
    private int Depth;
    public int Level;
    private bool AnimationPlay;

    private List<GameObject> gameObjectsDepth1;
    private List<GameObject> gameObjectsDepth2;
    private List<GameObject> gameObjectsDepth3;

    public Vector3 defaultCameraPositionStart;
    public Vector3 defaultCameraPositionEnd;
    public Vector3 defaultCameraPositionLevelMap;
    public float speedCamera;
    public float timeCamera;
    public float timeEndCamera;
    public float distanceCameraBuild;

    //Swipe
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    public float SWIPE_THRESHOLD = 20f;
    public Camera cam;

    //Canons
    public GameObject canonLeftGameObject;
    public GameObject canonMiddleGameObject;
    public GameObject canonRightGameObject;
    public Canon canonLeft;
    public Canon canonMiddle;
    public Canon canonRight;

    //UI
    public GameObject PanelGeneral;
    public GameObject PanelLevelMap;
    public GameObject ButtonGo;
    public GameObject ButtonMap;
    public GameObject PanelEndGame;
    public GameObject TitleWin;
    public GameObject TitleLose;
    public GameObject Star1;
    public GameObject Star2;
    public GameObject Star3;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!AnimationPlay)
        {
            SwipeControl();
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

    public void SwipeControl()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }
    }

    void SpawnCube()
    {
        Vector3 mousePos = Input.mousePosition;
        if(mousePos.y >= 895)
        {
            mousePos.z = distanceCameraBuild;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (Inventory.currentBrick != null)
            {
                if (Inventory.currentBrick.number > 0)
                {
                    GameObject obj = Instantiate(Inventory.currentBrick.prefab, objectPos, Quaternion.identity);
                    AddObjectDepthList(obj);
                    Inventory.currentBrick.number--;
                    //Update UI Stack Text
                    Inventory.renderStackUi(Inventory.currentBrick.number);
                }
            }
        }
        
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
                HideButton();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
                HideButton();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            checkEventOnUi();
            SpawnCube();
        }
    }

    void HideButton()
    {
        if (Depth == 0)
        {
            ButtonGo.SetActive(true);
            ButtonMap.SetActive(true);
        }
        else
        {
            ButtonGo.SetActive(false);
            ButtonMap.SetActive(false);
        }
    }

    void OnSwipeUp()
    {
        BackwardDepth();
    }

    void OnSwipeDown()
    {
        ForwardDepth();
    }

    void OnSwipeLeft()
    {
        
    }

    void OnSwipeRight()
    {
        
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    private void checkEventOnUi()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (IsPointerOverUIObject())
            return;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void Init()
    {
        Depth = 0;
        AnimationPlay = true;
        Application.targetFrameRate = TargetFps;

        gameObjectsDepth1 = new List<GameObject>();
        gameObjectsDepth2 = new List<GameObject>();
        gameObjectsDepth3 = new List<GameObject>();
    }

    private void AddObjectDepthList(GameObject obj)
    {
        switch (Depth)
        {
            case 0:
                gameObjectsDepth1.Add(obj);
                break;
            case 1:
                gameObjectsDepth2.Add(obj);
                break;
            case 2:
                gameObjectsDepth3.Add(obj);
                break;
        }
    }

    private void MakeTransparentObject()
    {
        switch (Depth)
        {
            case 0:
                foreach(GameObject obj in gameObjectsDepth2)
                {
                    
                }
                foreach (GameObject obj in gameObjectsDepth3)
                {
                    
                }
                break;
            case 1:
                foreach (GameObject obj in gameObjectsDepth1)
                {
                    
                }
                foreach (GameObject obj in gameObjectsDepth3)
                {
                    
                }
                break;
            case 2:
                foreach (GameObject obj in gameObjectsDepth1)
                {
                    
                }
                foreach (GameObject obj in gameObjectsDepth2)
                {
                    
                }
                break;
        }
    }

    private void ForwardDepth()
    {
        if (Depth < 2)
        {
            StartCoroutine(Kiwert.Moving(cam.gameObject, cam.gameObject.transform.position + (Vector3.forward * speedCamera), timeCamera));
            Depth++;
        }
        else
        {
            //StartCoroutine(Kiwert.Moving(cam.gameObject, defaultCameraPositionStart, timeEndCamera));
            //Depth = 0;
        }
    }

    private void BackwardDepth()
    {
        if (Depth < 1)
        {
            //StartCoroutine(Kiwert.Moving(cam.gameObject, defaultCameraPositionEnd, timeEndCamera));
            //Depth = 2;
        }
        else
        {
            StartCoroutine(Kiwert.Moving(cam.gameObject, cam.gameObject.transform.position + -(Vector3.forward * speedCamera), timeCamera));
            Depth--;
        }
    }

    public void Play()
    {
        StartCoroutine(StartLevelManager());
        AnimationPlay = true;
    }

    IEnumerator StartLevelManager()
    {
        //Custom Level
        switch (Level)
        {
            case 1:
                StartCoroutine(StartBaseAnimation());
                yield return new WaitForSeconds(0.5f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(0.5f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(0.5f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(2);
                StartCoroutine(EndBaseAnimation());
                StartCoroutine(EndLevel());
                break;
            case 2:
                StartCoroutine(StartBaseAnimation());
                yield return new WaitForSeconds(0.5f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(0.5f);
                canonLeft.StartShoot();
                yield return new WaitForSeconds(0.5f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(2);
                StartCoroutine(EndBaseAnimation());
                StartCoroutine(EndLevel());
                break;
            case 3:
                StartCoroutine(StartBaseAnimation());
                yield return new WaitForSeconds(0.5f);
                canonLeft.StartShoot();
                yield return new WaitForSeconds(0.3f);
                canonMiddle.StartShoot();
                yield return new WaitForSeconds(0.3f);
                canonRight.StartShoot();
                yield return new WaitForSeconds(2);
                StartCoroutine(EndBaseAnimation());
                StartCoroutine(EndLevel());
                break;
        }
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(5f);
        DestroyAllBalls();
        DestroyAllParticles();
        DestroyAllBricks();
        DisplayResult();
        //RESET Level
        house.BallCount = 0;
    }

    IEnumerator StartBaseAnimation()
    {
        PanelGeneral.SetActive(false);
        StartCoroutine(Kiwert.Moving(cam.gameObject, new Vector3(0f, 7.5f, -25f), 1f));
        StartCoroutine(Kiwert.Rotating(cam.gameObject, new Vector3(26f, 0f, 0f), 1f));
        yield return new WaitForSeconds(1f);
    }

    IEnumerator EndBaseAnimation()
    {
        StartCoroutine(Kiwert.Rotating(cam.gameObject, new Vector3(0f, 0f, 0f), 0.6f));
        StartCoroutine(Kiwert.Moving(cam.gameObject, new Vector3(0f, 2.5f, -25f), 0.8f));
        yield return new WaitForSeconds(1);
        AnimationPlay = false;
    }

    public void DestroyAllBalls()
    {
        foreach (GameObject ball in canonLeft.Balls)
        {
            Destroy(ball);
        }
        foreach (GameObject ball in canonMiddle.Balls)
        {
            Destroy(ball);
        }
        foreach (GameObject ball in canonRight.Balls)
        {
            Destroy(ball);
        }
    }

    public void DestroyAllParticles()
    {
        particles = GameObject.FindGameObjectsWithTag("particles");
        for (int i = 0; i < particles.Length; i++)
        {
            Destroy(particles[i]);
        }
    }

    public void DestroyAllBricks()
    {
        foreach(GameObject obj in gameObjectsDepth1)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in gameObjectsDepth2)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in gameObjectsDepth3)
        {
            Destroy(obj);
        }
    }

    public void InitLevel()
    {
        canonLeftGameObject.SetActive(true);
        canonRightGameObject.SetActive(true);
        canonMiddleGameObject.SetActive(true);
        switch (Level)
        {
            case 1:
                //SET STACK BRICKS
                Inventory.bricks[0].number = 10; //Basic
                Inventory.bricks[1].number = 5; //Horizontal
                Inventory.bricks[2].number = 0; //Vertical
                InitUI();
                canonLeftGameObject.SetActive(false);
                canonRightGameObject.SetActive(false);
                canonMiddle.BallSpeed = 600;
                break;
            case 2:
                //SET STACK BRICKS
                Inventory.bricks[0].number = 5; //Basic
                Inventory.bricks[1].number = 6; //Horizontal
                Inventory.bricks[2].number = 3; //Vertical
                InitUI();
                canonRightGameObject.SetActive(false);
                canonMiddle.BallSpeed = 700;
                canonLeft.BallSpeed = 700;
                break;
            case 3:
                //SET STACK BRICKS
                Inventory.bricks[0].number = 0; //Basic
                Inventory.bricks[1].number = 5; //Horizontal
                Inventory.bricks[2].number = 5; //Vertical
                InitUI();
                canonMiddle.BallSpeed = 800;
                canonLeft.BallSpeed = 800;
                canonRight.BallSpeed = 800;
                break;
        }
    }

    public void GoToLevelMap()
    {
        AnimationPlay = true;
        cam.gameObject.transform.position = defaultCameraPositionLevelMap;
        PanelLevelMap.SetActive(true);
        PanelGeneral.SetActive(false);
        PanelEndGame.SetActive(false);
        TitleWin.SetActive(false);
        TitleLose.SetActive(false);
    }

    public void DisplayResult()
    {
        PanelEndGame.SetActive(true);
        switch (house.BallCount)
        {
            case 0:
                TitleWin.SetActive(true);
                Star1.SetActive(true);
                Star2.SetActive(true);
                Star3.SetActive(true);
                break;
            case 1:
                TitleWin.SetActive(true);
                Star1.SetActive(true);
                Star2.SetActive(true);
                break;
            case 2:
                TitleWin.SetActive(true);
                Star1.SetActive(true);
                break;
            case 3:
                TitleLose.SetActive(true);
                break;
            default:
                TitleLose.SetActive(true);
                break;
        }
    }

    public void InitUI()
    {
        cam.gameObject.transform.position = defaultCameraPositionStart;
        Inventory.UpdateInventoryBricksAndUI();
        PanelGeneral.SetActive(true);
        PanelLevelMap.SetActive(false);
        AnimationPlay = false;
    }
    
    //LEVEL SELECTION

    public void SelectLevel1()
    {
        Level = 1;
        InitLevel();
    }

    public void SelectLevel2()
    {
        Level = 2;
        InitLevel();
    }

    public void SelectLevel3()
    {
        Level = 3;
        InitLevel();
    }
}
