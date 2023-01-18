using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Slot _slot_ref;
    private Vector3 _itemPreviewPosition;

    public bool _isSelected = false;
    private Vector3 mousePosition;

    private Transform _copySlotParent;

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isSelected = false;
        _slot_ref._item.transform.position = _itemPreviewPosition;
        Debug.Log("Return on position");

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Input.GetMouseButton(0) && _isSelected)
            {
                mousePosition = Input.mousePosition;
                //mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);
                _slot_ref._item.transform.position = Vector2.Lerp(transform.position, mousePosition, 1f);

            }
        }
        catch (InvalidCastException e)
        {
            Debug.Log(e);
        }


    }

    public void setItemSelected(Slot slot)
    {
        _slot_ref = slot;
        _copySlotParent = _slot_ref.transform.parent;
        _slot_ref.transform.SetParent(this.transform);
        _itemPreviewPosition = _slot_ref._item.transform.position;
        _isSelected = true;
    }

    public void unselectItem()
    {
        foreach (RaycastResult element in RaycastMouse())
        {
            try
            {
                if (element.gameObject.GetComponent("Slot"))
                {
                    Debug.Log("It's a slot");
                    Slot slot = (Slot)element.gameObject.GetComponent("Slot");
                    slot._item.setSprite(_slot_ref._item._spriteNeutral);
                    _slot_ref._item._itemType =  ItemType.NONE;
                    _slot_ref._item.Use();
                }
                else
                {
                    _slot_ref._item.transform.position = _itemPreviewPosition;
                    _slot_ref.transform.SetParent(_copySlotParent);
                }
            }
            catch (InvalidCastException e)
            {
                Debug.Log(e);
            }
        }

        _isSelected = false;
        _slot_ref = null;
    }

    public List<RaycastResult> RaycastMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }
}
