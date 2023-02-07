using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetenceSlot : MonoBehaviour
{


    public Sprite notSelectedSprite;
    public Sprite selectedSprite;

    // Update is called once per frame
    void Update()
    {

    }
    private void Start()
    {

    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            GetComponent<Image>().sprite = selectedSprite;
        }
        else
        {
            GetComponent<Image>().sprite = notSelectedSprite;
        }
    }
}
