using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset m_PlayerController;

    [Header("Action Map Name Asset")]
    [SerializeField] private string m_ActionMapName = "Player";

    [Header("Action name reference")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string dash = "Dash";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    public Vector2 MoveInput {  get; private set; }
    public Vector2 lookInput { get; private set; }
    public bool jumpInput { get; private set; }
    public bool dashInput { get; private set; }

    public static PlayerInputHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = m_PlayerController.FindActionMap(m_ActionMapName).FindAction(move);
        lookAction = m_PlayerController.FindActionMap(m_ActionMapName).FindAction(look);
        jumpAction = m_PlayerController.FindActionMap(m_ActionMapName).FindAction(jump);
        dashAction = m_PlayerController.FindActionMap(m_ActionMapName).FindAction(dash);
        RegisterInputAction();
    }

    void RegisterInputAction()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => lookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => lookInput = Vector2.zero;

        jumpAction.performed += context => jumpInput = true;
        jumpAction.canceled += context => jumpInput = false;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable(); 
        jumpAction.Enable();
        dashAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        dashAction.Disable();
    }
}
