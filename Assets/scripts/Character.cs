using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int _life;
    // Start is called before the first frame update
    public CanvasDetector _canvas;
    private float _timeSinceLastDamage = 0f;
    void Start()
    {
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
    void Update()
    {
        _timeSinceLastDamage -= Time.deltaTime;
        if (_life <= 0)
        {
            StartCoroutine(DestroyAfterTime(15f));
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
