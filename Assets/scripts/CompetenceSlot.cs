using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetenceSlot : MonoBehaviour
{
    public Sprite notSelectedSprite;
    public Sprite selectedSprite;

    private Skill _skill;

    public GameObject _skill_Ref;

    private Dictionary<Type, Action<Skill>> skillActions = new Dictionary<Type, Action<Skill>>();

    // Update is called once per frame
    void Update()
    {

    }

    public bool isFocus()
    {
        if (_skill)
        {
            return _skill.isFocus;
        }
        return false;
    }
    private void Start()
    {
        _skill = _skill_Ref.GetComponent<Skill>();
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            GetComponent<Image>().sprite = selectedSprite;
            if (_skill._skillType != SkillType.NONE)
            {
                // Type type = _skill.GetType();
                // Debug.Log(type);
                // if (type == typeof(DashSkill1))
                // {
                //     Debug.Log(type);
                //     DashSkill1 skillDash = (DashSkill1)_skill;
                //     skillDash.Use();
                // }
                // else
                // {
                //     _skill.Use();
                // }
                _skill.Use();
            }
        }
        else
        {
            GetComponent<Image>().sprite = notSelectedSprite;
        }
    }
}
