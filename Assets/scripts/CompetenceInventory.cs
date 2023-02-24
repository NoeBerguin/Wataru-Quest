using System;
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

    public GameObject[] _listSkill = new GameObject[10];

    private void Start()
    {
        // Récupérer la hauteur de l'écran en pixels
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // Récupérer la taille du panneau UI en pixels
        RectTransform rectTransform = GetComponent<RectTransform>();
        float panelHeight = rectTransform.rect.height;
        float panelWidth = rectTransform.rect.width;

        // Calculer la position Y pour placer le panneau UI au milieu en bas
        float yPosition = -(screenHeight) + (panelHeight);
        float xPosition = (screenWidth / 2) - (panelWidth / 2);

        // Définir la position du panneau UI
        rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
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

    public bool isFocus()
    {
        if (currentSlot != null)
        {
            return currentSlot.GetComponent<CompetenceSlot>().isFocus();
        }
        return true;
    }


    private void GenerateCompetenceSlots()
    {
        competenceSlots = new List<GameObject>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < 10; i++)
        {


            if (_listSkill[i])
            {
                GameObject slot = Instantiate(competenceSlotPrefab, transform);
                slot.GetComponent<CompetenceSlot>()._skill_Ref = Instantiate(_listSkill[i], slot.transform);
                RectTransform rectTransformSkill = slot.GetComponent<CompetenceSlot>()._skill_Ref.GetComponent<RectTransform>();
                rectTransformSkill.localScale = new Vector3(0.83f, 0.79f, 1);
                slot.GetComponent<RectTransform>().localPosition = new Vector3(startX + i * (slot.GetComponent<RectTransform>().rect.width + spaceBetweenSlots), startY, 0);
                competenceSlots.Add(slot);
            }
            // else
            // {
            //     GameObject slot = Instantiate(competenceSlotPrefab, transform);
            //     slot.transform.SetParent(this.transform);
            //     var tempColor = slot.GetComponent<CompetenceSlot>()._skill_Ref.GetComponent<Image>().color;
            //     tempColor.a = 0f;
            //     slot.GetComponent<CompetenceSlot>()._skill_Ref.GetComponent<Image>().color = tempColor;
            //     slot.GetComponent<RectTransform>().localPosition = new Vector3(startX + i * (slot.GetComponent<RectTransform>().rect.width + spaceBetweenSlots), startY, 0);
            //     competenceSlots.Add(slot);
            // }
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
