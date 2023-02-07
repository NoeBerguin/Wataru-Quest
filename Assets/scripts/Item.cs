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
        Use();
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
                Debug.Log("#HEALTH");
                setTransparent(false);
                break;

            case ItemType.SWORD:
                Debug.Log("#SWORD");
                setTransparent(false);
                break;

            case ItemType.NONE:
                Debug.Log("#NONE");
                setSprite(null);
                setTransparent(true);
                break;

            default:
                break;
        }
    }

    public void setTransparent(bool isTransparent)
    {
        var tempColor = _current_image.color;
        var output = (isTransparent == true) ? tempColor.a = 0f : tempColor.a = 1f;
        _current_image.color = tempColor;
    }
}
