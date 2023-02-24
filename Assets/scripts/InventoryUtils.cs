using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUtils : MonoBehaviour
{

    private List<GameObject> _listSlots;
    private RectTransform _rectTransform;
    public GameObject _slotPrefab;

    public Vector3 initialPading;

    public List<Item> _listOfInitialItems;


    public int _nbSlots, _nbRow, _nbColumn, _spaceBetweenSlots;

    void GenerateSlotsInventory()
    {
        int k = 0;
        for (int i = 0; i < _nbRow; i++)
        {
            for (int j = 0; j < _nbColumn; j++)
            {
                GameObject newSlot = (GameObject)Instantiate(_slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                if (k < _listOfInitialItems.Count)
                {
                    Slot slotObject = newSlot.GetComponent<Slot>();

                    cloneItem(slotObject, _listOfInitialItems[k]);
                    k++;
                }
                newSlot.transform.SetParent(this.transform);
                slotRect.localPosition = new Vector3(0, 0, 0) + initialPading + new Vector3(slotRect.sizeDelta.x / 2 + j * (slotRect.sizeDelta.x + _spaceBetweenSlots) + _spaceBetweenSlots, -slotRect.sizeDelta.y / 2 - i * (slotRect.sizeDelta.y + _spaceBetweenSlots) - _spaceBetweenSlots, 0);
                _listSlots.Add(newSlot);
            }
        }



    }

    void cloneItem(Slot slot, Item item)
    {
        slot._item._itemType = item._itemType;
        //slot._item.setSprite(item._spriteNeutral);
        slot._item._spriteNeutral = item._spriteNeutral;
        slot._item._maxSizeStack = item._maxSizeStack;
        //slot._item.Use();
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
