using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TestButtonEvent : StandaloneInputModule{
    public delegate  void HandleVG(GameObject obj);
    public event HandleVG OnMouseEnter;
    public event HandleVG OnMOuseExit;

    protected override void ProcessMove(PointerEventData pointerEvent)
    {
        GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
        HandlePointerExitAndEnter(pointerEvent, gameObject);
        base.ProcessMove(pointerEvent);
    }

    void HandleEnterAndExit(PointerEventData currentPointerData, GameObject newEnterTarget)
    {
        GameObject currentEnter = currentPointerData.pointerEnter;
        if ((newEnterTarget == null) || (currentEnter == null))
        {
            for (int i = 0; i < currentPointerData.hovered.Count; i++)
            {
                //ExecuteEvents.Execute<IPointerExitHandler>(currentPointerData.hovered[i], currentPointerData, ExecuteEvents.pointerExitHandler);
            }
            if (newEnterTarget == null)
            {
                return;
            }
        }

        if ((currentEnter != newEnterTarget) || (newEnterTarget == null))
        {
            GameObject obj2 = FindCommonRoot(currentEnter, newEnterTarget);
            if (currentEnter != null)
            {
                for (Transform transform = currentEnter.transform; transform != null; transform = transform.parent)
                {
                    if ((obj2 != null) && (obj2.transform == transform))
                    {
                        break;
                    }
                    if (transform.GetComponent<Button>() != null)
                        Debug.Log("exit:" + transform.name);
                    //ExecuteEvents.Execute<IPointerExitHandler>(transform.gameObject, currentPointerData, ExecuteEvents.pointerExitHandler);
                    //currentPointerData.hovered.Remove(transform.gameObject);
                }
            }
            if (newEnterTarget != null)
            {
                for (Transform transform2 = newEnterTarget.transform; (transform2 != null) && (transform2.gameObject != obj2); transform2 = transform2.parent)
                {
                    if(transform2.GetComponent<Button>() != null)
                    Debug.Log("enter:" + transform2.name);
                    //ExecuteEvents.Execute<IPointerEnterHandler>(transform2.gameObject, currentPointerData, ExecuteEvents.pointerEnterHandler);
                    //currentPointerData.hovered.Add(transform2.gameObject);
                }
            }
        }
    }

}
