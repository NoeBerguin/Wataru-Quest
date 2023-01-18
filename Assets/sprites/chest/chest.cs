using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closeSprite;

    public GameObject _inventoryUI;

    private SpriteRenderer _currentSprite;
    // Start is called before the first frame update
    void Start()
    {
        _currentSprite = this.GetComponent<SpriteRenderer>();
        _inventoryUI.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Code à exécuter lorsqu'il y a une collision
        Debug.Log("le coffre est ouvert !!");
        _currentSprite.sprite = openSprite;
        _inventoryUI.SetActive(true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Code à exécuter lorsqu'il y a une collision
        Debug.Log("le coffre est ferme !!");
        _currentSprite.sprite = closeSprite;
        _inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
