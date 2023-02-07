using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class controller : MonoBehaviour
{
    // private
    private Rigidbody2D _rigidBody;
    private float _moveInput;
    private Animator _animator;

    private bool _isInventory = false;
    private bool _animationLock = false;
    private bool _damageTaken = false;

    private GameObject _currentEnnemy;
    private bool _currentEnnemyIsInAttackRange = false;

    // public
    public float _speed = 1;
    public float _gravityEffect;
    public GameObject _user_inventory;
    public CircleCollider2D _attackCollider;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("idle");
    }

    private void Move(Vector2 velocity, string animationName, float angle)
    {
        _rigidBody.velocity = velocity;
        _animator.Play(animationName);
        transform.eulerAngles = new Vector3(0, angle, 0);
    }

    void MoveDirectionControl()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2.left, "walk", 180);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2.right, "walk", 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector2.up, "walk", 0);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2.down, "walk", 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            Move(new Vector2(0, 0), "idle", 0);
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.Play("Attacking");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _isInventory = !_isInventory;
            // Ouvrez l'inventaire ici
            _user_inventory.SetActive(_isInventory);
        }

    }

    void Update()
    {
        MoveDirectionControl();
        checkGiveDammageToEnemy();
    }


    void checkGiveDammageToEnemy()
    {
        if (_currentEnnemy != null && _currentEnnemyIsInAttackRange)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(AnimationEnum.Attacking))
            {
                _animationLock = stateInfo.normalizedTime < 1;
                _animator.Play(AnimationEnum.Attacking);
                if (stateInfo.normalizedTime % 1 >= 0.9f && !_damageTaken)
                {
                    _damageTaken = true;
                    _currentEnnemy.GetComponent<Character>().reciveDamage(1);
                }
                if (stateInfo.normalizedTime % 1 < 0.9f)
                {
                    _damageTaken = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_attackCollider.IsTouching(collision))
        {
            // Check if the collided object is of type "Character"
            if (collision.gameObject.GetComponent<Character>() != null)
            {
                Debug.Log("Spectrum enter ");
                // Call the "receiveDamage" function on the "Character" object
                _currentEnnemyIsInAttackRange = true;
                _currentEnnemy = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the exiting object is the current enemy
        if (collision.gameObject == _currentEnnemy)
        {
            Debug.Log("Spectrum exit ");
            // Set the current enemy is in attack range variable to false
            _currentEnnemyIsInAttackRange = false;
        }
    }
}
