using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject circle;
    public float horizontal,vertical;

    void Start(){
        border.SetActive(false);
        circle.SetActive(false);
    }

    public void DragHandler(BaseEventData data){
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
        );
        float distance = Vector3.Distance(canvas.transform.TransformPoint(position), border.GetComponent<Transform>().position);
        Vector3 tempPosition=new Vector3(canvas.transform.TransformPoint(position).x, canvas.transform.TransformPoint(position).y, 0f);
        if(distance<100){
            circle.GetComponent<Transform>().position = canvas.transform.TransformPoint(position);
        }
        else
        {
            Vector3 newTempPosition = tempPosition;
            newTempPosition=(newTempPosition-border.GetComponent<Transform>().position).normalized*100;
            newTempPosition=newTempPosition+border.GetComponent<Transform>().position;

            circle.GetComponent<Transform>().position = newTempPosition;
        }
        tempPosition = tempPosition-border.GetComponent<Transform>().position;
        horizontal = tempPosition.normalized.x;
        vertical = tempPosition.normalized.y;
        Debug.Log(horizontal);
        Debug.Log(vertical);
    }
    public void Selected(BaseEventData data){
        border.SetActive(true);
        circle.SetActive(true);
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position
        );

        border.GetComponent<Transform>().position = canvas.transform.TransformPoint(position);
        circle.GetComponent<Transform>().position = canvas.transform.TransformPoint(position);
        Debug.Log("mouse click down");
    }
    public void Deselected(){
        border.SetActive(false);
        circle.SetActive(false);
        Debug.Log("mouse click up");
    }
}
