using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpell : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 direction;
    private Animator animator;
    public float distanceToTravel = 5f;
    public bool isTuch = false;
    public bool isExplode = false;
    private Vector3 startPos;

    public GameObject invocator;

    private CircleCollider2D _detectionCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
        animator.Play("move");

        //Calculate angle between direction and forward vector
        float angle = Vector3.SignedAngle(direction, transform.right, Vector3.forward);
        angle *= -1;
        // Apply rotation to the object
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        _detectionCollider = GetComponent<CircleCollider2D>();

    }

    void Update()
    {
        if (!isTuch && !isExplode)
        {
            transform.position += direction * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, startPos) >= distanceToTravel)
            {
                isExplode = true;
                animator.Play("explode");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        BoxCollider2D boxCollider2D = other.gameObject.GetComponent<BoxCollider2D>();
        if (_detectionCollider.IsTouching(boxCollider2D))
        {
            if (boxCollider2D != invocator.GetComponent<BoxCollider2D>())
            {
                Debug.Log("tutch 2 ");
                Character character = other.gameObject.GetComponent<Character>();
                if (character != null)
                {
                    isTuch = true;
                    isExplode = true;
                    animator.Play("explode");
                    character.reciveDamage(1);
                }
            }
        }
    }

    void ExplodeAnimationEvent()
    {
        Destroy(gameObject);
    }
}
