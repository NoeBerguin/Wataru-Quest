using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { NONE, MAIN, ENNEMY }

public class Character : MonoBehaviour
{
    public int _life;
    // Start is called before the first frame update
    public CanvasDetector _canvas;
    public GameObject _lootCanvas;
    private float _timeSinceLastDamage = 0f;
    public CharacterType type;

    public GameObject _inventoryUI_prefab;
    private GameObject _inventoryUI_ref;

    private Rect _lifeBarRect;
    private Color _lifeBarColor;
    private int _initialLife;

    private Vector2 _worldPosition;
    private Vector2 _screenPosition;
    private Vector2 _lifeBarPosition;
    void Start()
    {
        _initialLife = _life;
    }

    public void increaseLife(int lifePoints)
    {
        _life += lifePoints;
    }

    public void reciveDamage(int lifePoints)
    {
        if (_timeSinceLastDamage <= 0f)
        {
            _life -= lifePoints;
            _timeSinceLastDamage = 2f;
            Debug.Log("life" + _life.ToString());
            if (_canvas)
            {
                _canvas.decreaseLifePoints(lifePoints);
            }
        }
    }


    void OnCollisionEnter2D(Collision2D otherObject)
    {
        Debug.Log("le coffre est ouvert !!");
        if (_life <= 0 && _inventoryUI_ref)
        {
            Debug.Log("le coffre est ouvert !!");
            // Code à exécuter lorsqu'il y a une collision
            _inventoryUI_ref.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D otherObject)
    {
        if (_life <= 0 && _inventoryUI_ref)
        {
            // Code à exécuter lorsqu'il y a une collision      
            _inventoryUI_ref.SetActive(false);
        }
    }

    void createLoot()
    {
        _inventoryUI_ref = Instantiate(_inventoryUI_prefab, transform.position, Quaternion.identity);
        _inventoryUI_ref.GetComponent<uiFollowObject>().lookAt = transform;
        _inventoryUI_ref.transform.SetParent(_lootCanvas.transform);

    }
    void Update()
    {

        _worldPosition = transform.position;
        _screenPosition = Camera.main.WorldToScreenPoint(_worldPosition);
        _lifeBarPosition = new Vector2(_screenPosition.x - 50, Screen.height - _screenPosition.y - 70);

        _timeSinceLastDamage -= Time.deltaTime;
        if (_life <= 0 && _inventoryUI_ref == null)
        {
            createLoot();
            StartCoroutine(DestroyAfterTime(150f));
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(_inventoryUI_ref);
        Destroy(gameObject);
    }

    void OnGUI()
    {
        if (!_canvas && _life > 0)
        {
            float lifePercent = (float)_life / (float)_initialLife;

            _lifeBarRect = new Rect(_lifeBarPosition.x, _lifeBarPosition.y - 15, 100 * lifePercent, 20);

            if (lifePercent > 0.75f)
            {
                _lifeBarColor = Color.green;
            }
            else if (lifePercent > 0.5f)
            {
                _lifeBarColor = Color.yellow;
            }
            else if (lifePercent > 0.25f)
            {
                _lifeBarColor = Color.red;
            }
            else
            {
                _lifeBarColor = Color.red;
            }

            GUI.Box(_lifeBarRect, "", new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    background = MakeTexture(2, 2, _lifeBarColor),
                }
            });
        }
    }

    Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }


}
