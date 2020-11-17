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
    public GameObject currentSlotObject;
    private Text textCurrentStack;
    private Image imageCurrentObject;

    public List<GameObject> slotsObjects;
    public List<Brick> bricks;
    void Start()
    {
        slotsObjects = new List<GameObject>();
        for(int i = 0; i < scrollItemsPrefabs.Length; i++)
        {
            Brick brick = scrollItemsPrefabs[i].GetComponent<Brick>();
            GenerateItem(scrollItemsPrefabs[i], brick);
        }
        DefaultSelection();
    }

    public void GenerateItem(GameObject prefabSlotItem, Brick brick)
    {
        GameObject slotObject = Instantiate(prefabSlotItem);
        slotsObjects.Add(slotObject);
        bricks.Add(brick);
        //Add slot into scroll content
        slotObject.transform.SetParent(scrollContent.transform, false);
        //Change brick number
        Text stackText = slotObject.transform.Find("Stack").gameObject.GetComponent<Text>();
        stackText.text = brick.defaultNumber.ToString();
        brick.number = brick.defaultNumber;
        //Get Image
        Image imageObjet = slotObject.GetComponent<Image>();
        var tempColor = imageObjet.color;
        tempColor.a = 1;
        imageObjet.color = tempColor;
        //Add listener button
        Button button = slotObject.gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate ()
        {
            currentBrick = brick;
            currentSlotObject = slotObject;
            textCurrentStack = stackText;
            imageCurrentObject = imageObjet;
            selectionObject();
        });
    }

    public void DefaultSelection()
    {
        //Default selection
        GameObject DefaultSlot = GameObject.Find("Basic Cube Item(Clone)");
        if(DefaultSlot != null)
        {
            Brick DefaultBrick = DefaultSlot.GetComponent<Brick>();
            Text TextStack = DefaultSlot.transform.Find("Stack").gameObject.GetComponent<Text>();
            Image ImageObjetDefault = DefaultSlot.GetComponent<Image>();
            currentBrick = DefaultBrick;
            currentSlotObject = DefaultSlot;
            textCurrentStack = TextStack;
            imageCurrentObject = ImageObjetDefault;
            selectionObject();
        }
    }

    public void renderStackUi(int stack)
    {
        textCurrentStack.text = stack.ToString();
    }

    public void selectionObject()
    {
        //Reset selections
        foreach(GameObject slotObj in slotsObjects)
        {
            Image slotObjImage = slotObj.GetComponent<Image>();
            var temp = slotObjImage.color;
            temp.a = 1f;
            slotObjImage.color = temp;
        }
        //Set selection
        var tempColor = imageCurrentObject.color;
        tempColor.a = 0.5f;
        imageCurrentObject.color = tempColor;
    }

    public void UpdateInventoryBricksAndUI()
    {
        int i = 0;
        foreach (GameObject slotObj in slotsObjects)
        {
            Brick brick = slotObj.GetComponent<Brick>();
            brick = bricks[i];
            //Change brick number
            Text stackText = slotObj.transform.Find("Stack").gameObject.GetComponent<Text>();
            stackText.text = brick.number.ToString();
            i++;
        }
    }
}
