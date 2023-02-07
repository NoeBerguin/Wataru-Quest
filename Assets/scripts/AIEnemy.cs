using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationEnum
{
    public static string idle { get { return "idle"; } }
    public static string Attacking { get { return "Attacking"; } }
    public static string walk { get { return "walk"; } }
    public static string Dying { get { return "Dying"; } }
}

public class AIEnemy : MonoBehaviour
{
    public GameObject _objectToFollow;
    public float radius = 5f; // the radius of the circle
    public float moveDuration = 1f; // how long the enemy should move in one direction before changing direction
    public float stopDurationMin = 2f; // minimum amount of time the enemy should stop before changing direction again
    public float stopDurationMax = 3f; // maximum amount of time the enemy should stop before changing direction again
    public float speed = 1f; // the speed of the enemy's movement
    public bool _invertion;
    public float _initial_speed = 0.5f;
    public GameObject iceSpellPrefab;
    public float range = 3f;
    public float spellCreateInterval = 3f;
    public CircleCollider2D _attackCollider;
    public CircleCollider2D _detectionCollider;

    private bool _follow = false;
    private bool _isReadyToAttack = false;
    private bool _animationLock = false;
    private float _current_speed = 1f;
    private Rigidbody2D _rigidBody2D;
    private Vector2 _mouvement;
    private Animator _animator;
    private bool _damageTaken = false;
    private Character _character;
    private Vector3 originalPosition; // the initial position of the enemy
    private Vector3 targetPosition; // the current target position
    private float moveStartTime; // the time when the enemy started moving
    private bool stopped = false; // whether the enemy is currently stopped
    private float stopStartTime; // the time when the enemy started stopping
    private float nextSpellCreateTime;

    void Start()
    {
        originalPosition = transform.position; // store the initial position
        _current_speed = _initial_speed;
        _animator = GetComponent<Animator>();
        _rigidBody2D = this.GetComponent<Rigidbody2D>();
        _rigidBody2D.isKinematic = true;
        _detectionCollider.isTrigger = true;
        _attackCollider.isTrigger = true;
        _follow = false;
        _character = GetComponent<Character>();
        nextSpellCreateTime = Time.time + spellCreateInterval;
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {

        if (otherObject.transform == _objectToFollow.transform)
        {
            if (_detectionCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                _follow = true;
            }

            if (_attackCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                _current_speed = 0f;
                _follow = false;
                _isReadyToAttack = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D otherObject)
    {

        if (otherObject.transform == _objectToFollow.transform)
        {
            if (!_detectionCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                _follow = false;
            }

            else if (!_attackCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                _follow = true;
                _current_speed = _initial_speed;
                _isReadyToAttack = false;
            }
        }
    }

    Vector2 getDirectionVector()
    {
        Vector3 direction = _objectToFollow.transform.position - transform.position;
        direction.Normalize();
        return direction;
    }

    Vector3 GetRandomPosition()
    {
        float angle = Random.Range(0f, 360f); // generate a random angle
        float x = originalPosition.x + radius * Mathf.Sin(angle); // calculate the x position
        float y = originalPosition.y + radius * Mathf.Cos(angle); // calculate the y position
        return new Vector3(x, y, 0); // return the new position
    }

    void randomMovement()
    {
        if (stopped) // if the enemy is currently stopped
        {
            if (Time.time - stopStartTime >= Random.Range(stopDurationMin, stopDurationMax)) // if the stop duration has passed
            {
                stopped = false; // the enemy is no longer stopped
                moveStartTime = Time.time; // update the move start time
                targetPosition = GetRandomPosition(); // get a new random position within the circle
            }

            _animator.Play(AnimationEnum.idle);
        }
        else // if the enemy is currently moving
        {
            if (Time.time - moveStartTime >= moveDuration) // if the move duration has passed
            {
                stopped = true; // the enemy is now stopped
                stopStartTime = Time.time; // update the stop start time
            }
            else
            {
                _mouvement = (targetPosition - transform.position).normalized;
                rotateCharacter();
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime); // move towards the target position at the specified speed
                _animator.Play(AnimationEnum.walk);
            }
        }
    }
    private void CreateSpell()
    {
        GameObject iceSpell = Instantiate(iceSpellPrefab, transform.position, Quaternion.identity);
        iceSpell.GetComponent<IceSpell>().direction = _objectToFollow.transform.position - transform.position;
        iceSpell.GetComponent<IceSpell>().invocator = gameObject;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpellCreateTime)
        {
            float distance = Vector3.Distance(transform.position, _objectToFollow.transform.position);
            if (distance <= range)
            {
                CreateSpell();
                nextSpellCreateTime = Time.time + spellCreateInterval;
            }
        }
        if (_character._life <= 0)
        {
            _animator.Play(AnimationEnum.Dying);
        }
        else
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName(AnimationEnum.Attacking))
            {
                _animationLock = stateInfo.normalizedTime < 1;
                _animator.Play(AnimationEnum.Attacking);
                if (stateInfo.normalizedTime % 1 >= 0.9f && !_damageTaken)
                {
                    _damageTaken = true;
                    _objectToFollow.GetComponent<Character>().reciveDamage(1);
                }
                if (stateInfo.normalizedTime % 1 < 0.9f)
                {
                    _damageTaken = false;
                }
            }
            if (!_animationLock)
            {
                if (_follow)
                {
                    _animator.Play(AnimationEnum.walk);
                    _mouvement = getDirectionVector();
                }
                else if (_isReadyToAttack)
                {
                    _animator.Play(AnimationEnum.Attacking);
                }
                else
                {
                    randomMovement();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (_follow && !_animationLock && _character._life > 0)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        _rigidBody2D.MovePosition((Vector2)transform.position + (_mouvement * _current_speed * Time.deltaTime));
        rotateCharacter();
    }

    private void rotateCharacter()
    {
        if (_invertion)
        {
            if (_mouvement.x > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (_mouvement.x < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
    }
}


