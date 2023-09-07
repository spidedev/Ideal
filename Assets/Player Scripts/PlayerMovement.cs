using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    
    // Physics
    
    [SerializeField] float _speed;
    private Rigidbody2D _rigidbody2D;
    
    // Input
    
    private Vector2 _movementInput;
    private MainInput _input;
    
    // Events (Broadcasters)
    
    public static event Action _confirmed ;
    public static event Action _canceledconfirmed;
    public static event Action _inventoryOpen;
    public static event Action _upInv;
    public static event Action _downInv;
    
    // Visuals

    private Animator _animator;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = new MainInput();

    }

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Fire.performed += ConfirmedFunction;
        _input.Player.Fire.canceled += CanceledConfirmedFunction;
        _input.Player.Inventory.performed += InventoryFunction;
        
        // Inventory
        
        _input.Player.Up_Inv.performed += Up_i;
        _input.Player.Down_Inv.performed += Down_i;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Fire.performed -= ConfirmedFunction;
        _input.Player.Fire.canceled -= CanceledConfirmedFunction;
        _input.Player.Inventory.performed -= InventoryFunction;
        
        // Inventory

        _input.Player.Up_Inv.performed -= Up_i;
        _input.Player.Down_Inv.performed -= Down_i;
    }

    private void Update()
    {
        if (DialogueManager.GetInstance()._dialogueIsPlaying)
        {
            _animator.SetFloat("Speed", 0.00f);
            return;
        }
        
        if (InventoryManagement_GUI.DisplayingGUI)
        {
            _animator.SetFloat("Speed", 0.00f);
            return;
        }
        
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Vertical", _movementInput.y);
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance()._dialogueIsPlaying)
        {
            return;
        }
        
        if (InventoryManagement_GUI.DisplayingGUI)
        {
            return;
        }
        
        _rigidbody2D.velocity = _movementInput * _speed;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void ConfirmedFunction(InputAction.CallbackContext value)
    {
        _confirmed?.Invoke();
    }
    
    private void CanceledConfirmedFunction(InputAction.CallbackContext value)
    {
        _canceledconfirmed?.Invoke();
    }
    
    private void InventoryFunction(InputAction.CallbackContext value)
    {
        _inventoryOpen?.Invoke();
    }
    
    private void Up_i(InputAction.CallbackContext value)
    {
        _upInv?.Invoke();
    }
    
    private void Down_i(InputAction.CallbackContext value)
    {
        _downInv?.Invoke();
    }
}
