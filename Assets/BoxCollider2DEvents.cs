using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider2DEvents : MonoBehaviour
{
    public GameObject _objectToFollow;

    public float _speed = 0.5f;
    private bool _follow = false;
    private bool _isReadyToAttack = false;

    private bool _animationLock = false;
    private float _current_speed = 0.5f;
    private Rigidbody2D _rigidBody2D;
    private Vector2 _mouvement;

    public CircleCollider2D _attackCollider;
    public CircleCollider2D _detectionCollider;

    private Animator _animator;

    void Start()
    {
        _current_speed = _speed;
        _animator = GetComponent<Animator>();
        _rigidBody2D = this.GetComponent<Rigidbody2D>();
        _rigidBody2D.isKinematic = true;
        _detectionCollider.isTrigger = true;
        _attackCollider.isTrigger = true;
        _follow = false;
    }

    void OnTriggerEnter2D(Collider2D otherObject)
    {

        if (otherObject.transform == _objectToFollow.transform)
        {
            if (_detectionCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                // Debug.Log("Entrée dans la zone définie par le box collider");
                _follow = true;
            }

            if (_attackCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                // _isReadyToAttack = true;
                _current_speed = 0f;
                _follow = false;
                _isReadyToAttack = true;

                // Debug.Log("Ready to attack!!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D otherObject)
    {

        if (otherObject.transform == _objectToFollow.transform)
        {
            if (!_detectionCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                // Debug.Log("Sortie de la zone définie par le box collider");
                _follow = false;
            }

            else if (!_attackCollider.IsTouching(_objectToFollow.GetComponent<BoxCollider2D>()))
            {
                // _isReadyToAttack = true;
                _follow = true;
                _current_speed = _speed;
                _isReadyToAttack = false;
                // Debug.Log("NOT ready to attack!!");
            }
        }
    }

    Vector2 getDirectionVector()
    {
        Vector3 direction = _objectToFollow.transform.position - transform.position;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // _rigidBody2D.rotation = angle;
        direction.Normalize();
        return direction;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0); // 0 est l'indice du layer d'animation par défaut
        AnimatorClipInfo[] m_CurrentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        string current_animationName = m_CurrentClipInfo[0].clip.name;
        float animationDuration = stateInfo.length;
        if (current_animationName == "skeleton_attack")
        {
            _animationLock = stateInfo.normalizedTime < 1;
            _animator.Play("skeleton_attack");
        }

        if (!_animationLock)
        {
            if (_follow)
            {
                _animator.Play("skeleton_walk");
                _mouvement = getDirectionVector();
            }
            else if (_isReadyToAttack)
            {
                _animator.Play("skeleton_attack");
            }
            else
            {
                _animator.Play("skeleton_idle");
            }
        }
    }

    private void FixedUpdate()
    {
        if (_follow && !_animationLock)
        {
            MoveCharacter();
        }
    }

    private void MoveCharacter()
    {
        Debug.Log(_mouvement.x);
        _rigidBody2D.MovePosition((Vector2)transform.position + (_mouvement * _current_speed * Time.deltaTime));
        if (_mouvement.x > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
