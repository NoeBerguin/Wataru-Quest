using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Vector3 targetPosition;  // La position de la souris où l'objet doit se déplacer

    private bool isMoving;  // Indique si l'objet doit se déplacer ou non


    // public
    public float _speed = 1;
    public float _gravityEffect;
    public GameObject _user_inventory;
    public CircleCollider2D _attackCollider;

    public GameObject _competenceInventory;

    public bool isMovingItem = false;


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
        if (Input.GetMouseButtonDown(0) && !_competenceInventory.GetComponent<CompetenceInventory>().isFocus() && !isMovingItem)  // Si l'utilisateur clique avec la souris
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // Récupère la position de la souris
            targetPosition.z = transform.position.z;  // La position Z est la même que celle de l'objet pour éviter les problèmes de profondeur
            isMoving = true;  // Définit le booléen à true pour démarrer le déplacement
        }

        if (isMoving)
        {
            if (transform.position != targetPosition)  // Si l'objet n'est pas encore à la position de la souris
            {
                if (transform.position.x > targetPosition.x)
                {
                    transform.eulerAngles = new Vector3(0, 180f, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);  // Déplace l'objet vers la position de la souris avec une vitesse définie
                _animator.Play("walk");
                if (transform.position == targetPosition)  // Vérifie si l'objet a atteint sa destination
                {
                    _animator.Play("idle");
                    isMoving = false;  // Définit le booléen à false pour arrêter le déplacement
                }                                                                                                       // Si la direction est vers la droite, faire une rotation de 180 degrés
            }
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        targetPosition = transform.position;
        isMoving = false;
        _animator.Play("idle");
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
