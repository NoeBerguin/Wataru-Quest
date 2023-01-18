using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item _item;
    private CanvasDetector _canvasDetector;

    private Sprite _image;
    private Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransformItem = _item.GetComponent<RectTransform>();
        _canvasDetector = (CanvasDetector)GameObject.Find("Canvas").GetComponent("CanvasDetector");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onClick()
    {
        // Debug.Log("Apple");
        // _canvasDetector.setItemSelected(this);
    }

    public void onPress()
    {
        Debug.Log("Press Down");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasDetector.setItemSelected(this);
        Debug.Log("Press Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasDetector.unselectItem();
    }
}
