using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUtils : MonoBehaviour
{

    private List<GameObject> _listSlots;
    private RectTransform _rectTransform;
    public GameObject _slotPrefab;


    public int _nbSlots, _nbRow, _nbColumn, _spaceBetweenSlots;

    void GenerateSlotsInventory()
    {
        for (int i = 0; i < _nbRow; i++)
        {
            for (int j = 0; j < _nbColumn; j++)
            {
                GameObject newSlot = (GameObject)Instantiate(_slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.transform.SetParent(this.transform);
                slotRect.localPosition = new Vector3(0, 0, 0) + new Vector3(slotRect.sizeDelta.x / 2 + j * (slotRect.sizeDelta.x + _spaceBetweenSlots) + _spaceBetweenSlots, -slotRect.sizeDelta.y / 2 - i * (slotRect.sizeDelta.y + _spaceBetweenSlots) - _spaceBetweenSlots, 0);
                _listSlots.Add(newSlot);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _listSlots = new List<GameObject>();
        _rectTransform = this.GetComponent<RectTransform>();

        GenerateSlotsInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
