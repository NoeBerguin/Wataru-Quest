using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ItemType { NONE, HEALTH, SWORD }

public class Item : MonoBehaviour
{
    public ItemType _itemType;
    public Sprite _spriteNeutral;
    public int _maxSizeStack;

    private Image _current_image;


    public void Start()
    {
        _current_image = GetComponent<Image>();
        _current_image.sprite = _spriteNeutral;
    }

    public void setSprite(Sprite sprite)
    {
        _spriteNeutral = sprite;
        _current_image.sprite = _spriteNeutral;
    }

    public void Update()
    {

    }


    public void Use()
    {
        switch (_itemType)
        {
            case ItemType.HEALTH:
                Debug.Log("#MANA");
                break;

            case ItemType.SWORD:
                Debug.Log("#SWORD");
                break;

            case ItemType.NONE:
                Debug.Log("#NONE");
                setSprite(null);
                break;

            default:
                break;
        }
    }
}
