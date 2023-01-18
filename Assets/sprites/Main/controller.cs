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

    // public
    public float _speed = 1;
    public float _gravityEffect;
    public GameObject _user_inventory;


    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("main_idle");
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
            Move(Vector2.left, "main_walk", 0);
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2.right, "main_walk", 180);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector2.up, "main_walk", 0);
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2.down, "main_walk", 0);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            Move(new Vector2(0, 0), "main_idle", 0);
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
    }
}
