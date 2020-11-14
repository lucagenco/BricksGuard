using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject scrollContent;
    public GameObject[] scrollItemsPrefabs;
    public Brick currentBrick;
    
    void Start()
    {
        for(int i = 0; i < scrollItemsPrefabs.Length; i++)
        {
            Brick brick = scrollItemsPrefabs[i].GetComponent<Brick>();
            generateItem(scrollItemsPrefabs[i], brick);
        }
    }

    public void generateItem(GameObject prefabSlotItem, Brick brick)
    {
        GameObject slotObject = Instantiate(prefabSlotItem);
        //Add slot into scroll content
        slotObject.transform.SetParent(scrollContent.transform, false);
        //Change brick number
        slotObject.transform.Find("Stack").gameObject.GetComponent<Text>().text = brick.number.ToString();
        //Add listener button
        Button button = slotObject.gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate ()
        {
            currentBrick = brick;
        });
    }
}
