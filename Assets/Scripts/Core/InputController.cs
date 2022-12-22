using Cinemachine;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class InputController : Singleton<InputController>, AxisState.IInputAxisProvider
    {
        private Vector3 _moveInput;
        private Vector3 _lookInput;
        private bool _isAiming;
        private bool _isSprinting;
        private bool _jumped;
        private bool _isShooting;
        
#if ENABLE_INPUT_SYSTEM  
        public void OnMove(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            _moveInput.x = input.x;
            _moveInput.z = input.y;
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            var input = context.ReadValue<Vector2>();
            _lookInput.x = input.x;
            _lookInput.z = input.y;
        }
        
        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() && context.performed) _isAiming = true;
            if (context.canceled) _isAiming = false;
        }
    
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton() && context.performed) _isSprinting = true;
            if (context.canceled) _isSprinting = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            _jumped = context.ReadValueAsButton();
        }
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            //TODO how do i shoot based on the two keybinds?
            if (context.ReadValueAsButton() && context.performed) _isShooting = true;
            if (context.canceled) _isShooting = false;
        }
        
        public bool IsSprinting()
        {
            return _isSprinting;
        }
        
        public bool IsAiming()
        {
            //return Mouse.current.rightButton.isPressed;
            return _isAiming;
        }
        
        public bool IsShooting()
        {
            //return Mouse.current.leftButton.isPressed && Mouse.current.rightButton.isPressed;
            return _isShooting;
        }
        
        public bool Jumped()
        {
            return _jumped;
        }

        public Vector3 MoveInput()
        {
            return _moveInput;
        }

        public Vector3 LookInput()
        {
            //var lookInput = new Vector3(Mouse.current.delta.x.ReadValue(),0,Mouse.current.delta.y.ReadValue());
            //return lookInput;
            return _lookInput;
        }
        
#endif
#if ENABLE_LEGACY_INPUT_MANAGER

        [SerializeField] private KeyCode _sprintKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;  
        [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

        public Vector3 MoveInput()
        {
            _moveInput.x = Input.GetAxis("Horizontal");
            _moveInput.y = 0;
            _moveInput.z = Input.GetAxis("Vertical");
            return _moveInput;
        }

        public Vector3 LookInput()
        {
            _lookInput.x = Input.GetAxisRaw("Mouse X");
            _lookInput.y = 0;
            _lookInput.z = Input.GetAxisRaw("Mouse Y");
            return _lookInput;
        }

        public bool IsSprinting()
        {
            return Input.GetKey(_sprintKey);
        }
        
        public bool IsAiming()
        {
            return Input.GetKey(_aimKey);
        }
        
        public bool IsShooting()
        {
           return Input.GetKey(_aimKey) && Input.GetKey(_shootKey);
        }
        public bool Jumped()
        {
            return Input.GetKeyDown(_jumpKey);
        }

#endif

        public float GetAxisValue(int axis)
        {
 
            if (axis == 0) return LookInput().x;
            if (axis == 1) return LookInput().y;
            if (axis == 2) return LookInput().z;
            return 0;
        }
    }
}