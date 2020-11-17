using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject PrefabBall;
    public GameObject FxPrefab;

    public float BallSpeed;
    public GameManager GameManager;

    public int BallShoot;
    public List<GameObject> Balls;

    void Start()
    {
        Balls = new List<GameObject>();
    }

    public void StartShoot()
    {
        StartCoroutine(Shoot(BallShoot));
    }

    IEnumerator Shoot(int number)
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < number; i++)
        {
            GameObject ball = Instantiate(PrefabBall, transform.position, Quaternion.identity);
            Balls.Add(ball);
            Instantiate(FxPrefab, transform.position, Quaternion.identity);
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * BallSpeed);
        }
    }

    public void destroyBall()
    {
        foreach (GameObject b in Balls)
        {
            Destroy(b);
        }
    }
}
