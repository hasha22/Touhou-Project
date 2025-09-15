using UnityEngine;
using UnityEngine.SceneManagement;
namespace KH
{
    public class PlayerInputManager : MonoBehaviour
    {
        [HideInInspector] public InputControls inputControls;
        public static PlayerInputManager instance { get; private set; }

        [Header("Player Movement")]
        public Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount = 0;

        [Header("Player Actions")]
        public bool isShooting = false;
        public bool isInPrecision = false;
        public bool isBombing = false;

        #region Setup Methods
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            SceneManager.activeSceneChanged += OnSceneChange;

            if (inputControls != null)
            {
                inputControls.Disable();
            }
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void HandleAllInputs()
        {
            HandleMovementInput();
        }
        private void OnEnable()
        {
            if (inputControls == null)
            {
                inputControls = new InputControls();
                inputControls.Player.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                inputControls.Player.Movement.canceled += i => movementInput = Vector2.zero;

                inputControls.Player.Shoot.performed += i => isShooting = true;
                inputControls.Player.Shoot.canceled += i => isShooting = false;

                inputControls.Player.Precision.performed += i => isInPrecision = true;
                inputControls.Player.Precision.canceled += i => isInPrecision = false;

                inputControls.Player.Bomb.performed += i => isBombing = true;
                inputControls.Player.Bomb.canceled += i => isBombing = false;

                inputControls.Enable();
            }
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                inputControls.Enable();
            }
            else
            {
                inputControls.Disable();
            }
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //Enables controls if loading into Game Scene, disables otherwise
            if (newScene.buildIndex == TitleScreenManager.instance.gameSceneIndex)
            {
                instance.enabled = true;
                if (inputControls != null)
                {
                    inputControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;
                if (inputControls != null)
                {
                    inputControls.Disable();
                }
            }
        }
        #endregion

        #region Input Methods
        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
        }
        #endregion

    }
}

