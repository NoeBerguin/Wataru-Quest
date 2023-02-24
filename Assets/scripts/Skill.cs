using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType { NONE, MAGIC, CAC }
public enum InvocatorType { NONE, MAIN, ENEMY }


public class Skill : MonoBehaviour
{
    public Sprite _sprite;
    public GameObject _skillPrefab;

    public SkillType _skillType = SkillType.NONE;

    public GameObject _invocator;

    public bool isFocus = false;
    public GameObject circlePrefab;
    private GameObject _circle;

    public InvocatorType _invocatorType;

    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Image>().sprite = _sprite;
        _invocator = GameObject.Find("Main");
        _invocatorType = InvocatorType.MAIN;
    }

    public void Copy(Skill skill)
    {
        _sprite = skill._sprite;
        _skillPrefab = skill._skillPrefab;
        _skillType = skill._skillType;
        _invocator = skill._invocator;
        isFocus = skill.isFocus;
        circlePrefab = skill.circlePrefab;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (_skillType != SkillType.NONE && isFocus)
        {
            if (_invocatorType == InvocatorType.MAIN)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0;

                if (isFocus && _circle)
                {
                    _circle.transform.position = mousePosition;

                    if (Input.GetMouseButtonDown(0))
                    {
                        // Supprimez le cercle
                        Destroy(_circle);

                        // Utilisez le sort
                        GameObject iceSpell = Instantiate(_skillPrefab, _invocator.transform.position, Quaternion.identity);
                        iceSpell.GetComponent<IceSpell>().direction = mousePosition - _invocator.transform.position;
                        iceSpell.GetComponent<IceSpell>().invocator = _invocator;
                        isFocus = false;
                    }
                    else if (Input.GetKeyUp(""))
                    {
                        Debug.Log("error cicrle");
                        isFocus = false;
                    }
                }
            }
        }

    }

    public virtual void Use()
    {
        if (_skillType == SkillType.MAGIC)
        {
            if (_invocatorType == InvocatorType.MAIN)
            {
                Debug.Log("spell 1");
                Vector3 mousePosition = Input.mousePosition;
                mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                mousePosition.z = 0;

                _circle = Instantiate(circlePrefab, mousePosition, Quaternion.identity);
                isFocus = true;
            }
            else if (_invocatorType == InvocatorType.ENEMY)
            {
                GameObject iceSpell = Instantiate(_skillPrefab, _invocator.transform.position, Quaternion.identity);
                iceSpell.GetComponent<IceSpell>().direction = GameObject.Find("Main").transform.position - _invocator.transform.position;
                iceSpell.GetComponent<IceSpell>().invocator = _invocator;
            }
        }
        else
        {
            GameObject iceSpell = Instantiate(_skillPrefab, _invocator.transform.position, Quaternion.identity);
            iceSpell.GetComponent<IceSpell>().direction = _invocator.transform.position;
            iceSpell.GetComponent<IceSpell>().invocator = _invocator;
        }
    }
}
