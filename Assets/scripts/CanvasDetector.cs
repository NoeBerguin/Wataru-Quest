using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasDetector : MonoBehaviour
{
    public GameObject _lifeObject;
    public float _spaceBetweenLifeUI;
    public GameObject _player;
    private Slot _slot_ref;
    private Vector3 _itemPreviewPosition;
    private List<GameObject> _listLife;

    public bool _isSelected = false;
    private Vector3 mousePosition;

    private Transform _copySlotParent;



    // Start is called before the first frame update
    void Start()
    {
        _listLife = new List<GameObject>();

        float minX = GetComponent<RectTransform>().position.x + 2 * GetComponent<RectTransform>().rect.xMin;
        float maxY = GetComponent<RectTransform>().position.y;
        float z = GetComponent<RectTransform>().position.z;

        Vector3 topLeft = new Vector3(minX, maxY, z);

        for (int i = 0; i < _player.GetComponent<Character>()._life; i++)
        {
            GameObject newLife = (GameObject)Instantiate(_lifeObject);
            RectTransform lifeRect = newLife.GetComponent<RectTransform>();
            newLife.transform.SetParent(this.transform);
            lifeRect.localPosition = topLeft + new Vector3(20 + i * (lifeRect.sizeDelta.x + _spaceBetweenLifeUI), -20, 0);
            _listLife.Add(newLife);
        }

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
                _slot_ref.transform.SetParent(_copySlotParent);

            }
        }
        catch (InvalidCastException e)
        {
            Debug.Log(e);
        }


    }

    public void setItemSelected(Slot slot)
    {
        _player.GetComponent<controller>().isMovingItem = true;
        _slot_ref = slot;
        _copySlotParent = _slot_ref.transform.parent;
        _slot_ref.transform.SetParent(this.transform);
        _itemPreviewPosition = _slot_ref._item.transform.position;
        _isSelected = true;
    }

    public void unselectItem()
    {
        _player.GetComponent<controller>().isMovingItem = false;
        foreach (RaycastResult element in RaycastMouse())
        {
            try
            {
                if (element.gameObject.GetComponent("Slot"))
                {
                    //Debug.Log("It's a slot");
                    Slot slot = (Slot)element.gameObject.GetComponent("Slot");
                    slot._item.setSprite(_slot_ref._item._spriteNeutral);
                    slot._item._itemType = _slot_ref._item._itemType;
                    slot._item.setTransparent(false);
                    _slot_ref._item._itemType = ItemType.NONE;
                    _slot_ref._item.Use();
                }
                else
                {
                    _slot_ref._item.transform.position = _itemPreviewPosition;
                    _slot_ref.transform.SetParent(_copySlotParent);
                }

                if (element.gameObject.GetComponent("CompetenceInventory"))
                {
                    Debug.Log("CompetenceInventory");
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

    public void decreaseLifePoints(int amount)
    {
        if (_listLife.Count > 0 && amount <= _listLife.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject life = _listLife[_listLife.Count - 1];
                _listLife.RemoveAt(_listLife.Count - 1);
                Destroy(life);
            }
        }
    }

}
