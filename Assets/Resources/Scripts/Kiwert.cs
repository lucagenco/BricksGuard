using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kiwert : MonoBehaviour
{
    static public IEnumerator Moving(GameObject GO, Vector3 targetPos, float totalTime)
    {

        Vector3 goPos;
        goPos = GO.GetComponent<Image>() == null ? GO.transform.localPosition : GO.GetComponent<RectTransform>().anchoredPosition3D;


        float curTime = totalTime;

        Vector3 totalDist = (targetPos - goPos);
        while (curTime > 0)
        {
            var timePassed = Time.fixedDeltaTime;

            goPos += timePassed * totalDist / totalTime;
            curTime -= timePassed;

            if (GO.GetComponent<Image>() == null)
                GO.transform.localPosition = goPos;

            else
                GO.GetComponent<RectTransform>().anchoredPosition3D = goPos;

            yield return null;
        }
        if (GO.GetComponent<Image>() == null)
            GO.transform.localPosition = targetPos;
        else
            GO.GetComponent<RectTransform>().anchoredPosition3D = targetPos;

        yield return null;
    }

    static public IEnumerator Rotating(GameObject GO, Vector3 targetRot, float totalTime)
    {
        float timeAdd = 0;
        Vector3 startRot = GO.transform.localEulerAngles;
        while (timeAdd < totalTime)
        {
            timeAdd += Time.deltaTime;
            GO.transform.localEulerAngles = AngleLerp(startRot, targetRot, timeAdd / totalTime);

            yield return null;
        }

        GO.transform.localEulerAngles = targetRot;
        yield return null;
    }

    private static Vector3 AngleLerp(Vector3 StartAngle, Vector3 FinishAngle, float t)
    {
        float xLerp = Mathf.LerpAngle(StartAngle.x, FinishAngle.x, t);
        float yLerp = Mathf.LerpAngle(StartAngle.y, FinishAngle.y, t);
        float zLerp = Mathf.LerpAngle(StartAngle.z, FinishAngle.z, t);
        Vector3 Lerped = new Vector3(xLerp, yLerp, zLerp);
        return Lerped;
    }
}
