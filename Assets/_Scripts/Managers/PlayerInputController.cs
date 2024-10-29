using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Windows;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance;
    [SerializeField] private PlayerInput _playerInput;
    public event Action SummonUnitPressed;
    public event Action CastSpellPressed;
    public event Action CheatPressed;
    public event Action PausePressed;
    public event Action<int> UnitSelectionPressed;
    public event Action<int> SpellSelectionPressed;
    public static event Action<bool> ChangedOnGamepad;
    public static event Action GameSpeedPressed;

    private const string mouseScheme = "Keyboard";
    private const string gamepadScheme = "Gamepad";
    private string _previousControlSheme = "";

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Cursor.visible = false;
    }
    public void Check()
    {
        
        if (_playerInput.currentControlScheme == mouseScheme)
        {
            Cursor.visible = true;
            _previousControlSheme = mouseScheme;
            ChangedOnGamepad?.Invoke(false);
        }
        else if (_playerInput.currentControlScheme == gamepadScheme)
        {
            Cursor.visible = false;
            _previousControlSheme = gamepadScheme;
            ChangedOnGamepad?.Invoke(true);
        }

    }

    public void SpellSelection_performed(InputAction.CallbackContext obj)
    {
        if(obj.performed)
        SpellSelectionPressed?.Invoke(obj.ReadValue<float>() > 0f ? 1 : -1);
    }

    public void UnitSelection_performed(InputAction.CallbackContext obj)
    {
        if (obj.performed)
            UnitSelectionPressed?.Invoke(obj.ReadValue<float>() > 0f ? 1:-1);
    }

    public void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.performed)
            PausePressed?.Invoke();
    }

    public void Cheat_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.performed)
            if (Time.timeScale != 0)
            CheatPressed?.Invoke();
    }

    public void CastSpell_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (obj.performed)
            if (Time.timeScale != 0)
            CastSpellPressed?.Invoke();
    }

    public void SummonUnit_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
            if (Time.timeScale != 0)
            SummonUnitPressed?.Invoke();
    }
    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == mouseScheme && _previousControlSheme != mouseScheme)
        {
            Cursor.visible = true;
            _previousControlSheme = mouseScheme;
            ChangedOnGamepad?.Invoke(false);
        }
        else if (input.currentControlScheme == gamepadScheme && _previousControlSheme != gamepadScheme)
        {
            Cursor.visible = false;
            _previousControlSheme = gamepadScheme;
            ChangedOnGamepad?.Invoke(true);
        }
    }
    public void GameSpeed_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
                GameSpeedPressed?.Invoke();
    }

    private void OnDestroy()
    {

        Instance = null;
    }
}
