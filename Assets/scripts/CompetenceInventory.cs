using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetenceInventory : MonoBehaviour
{
    public GameObject competenceSlotPrefab;
    private GameObject currentSlot;
    public float spaceBetweenSlots = 1f;
    public float startX = 0f;
    public float startY = 0f;
    private List<GameObject> competenceSlots;

    private void Start()
    {
        GenerateCompetenceSlots();
        currentSlot = competenceSlots[0];
    }

    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectSlot(i);
                break;
            }
        }
    }


    private void GenerateCompetenceSlots()
    {
        competenceSlots = new List<GameObject>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < 10; i++)
        {
            GameObject slot = Instantiate(competenceSlotPrefab, transform);
            slot.transform.SetParent(this.transform);
            slot.GetComponent<RectTransform>().localPosition = new Vector3(startX + i * (slot.GetComponent<RectTransform>().rect.width + spaceBetweenSlots), startY, 0);
            competenceSlots.Add(slot);
        }
    }

    public void SelectSlot(int index)
    {
        Debug.Log(index);
        currentSlot.GetComponent<CompetenceSlot>().SetSelected(false);
        currentSlot = competenceSlots[index];
        currentSlot.GetComponent<CompetenceSlot>().SetSelected(true);
    }
}
