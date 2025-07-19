
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.InputSystem;
using DrawXXL;

public class ToonPlayer : MonoBehaviour
{

    struct InputReferences
    {
        public InputAction
        move,
        look_around,
        jump;
    }
    private InputReferences inputReferences;
    public ToonCharacterController Character;
    public ToonCharacterCamera CharacterCamera;


    private void Start()
    {
        // Allocate inputs
        inputReferences = new InputReferences
        {
            move = InputSystem.actions.FindAction("Player/Move"),
            look_around = InputSystem.actions.FindAction("Player/Look"),
            jump = InputSystem.actions.FindAction("Player/jump")
        };

        //Sensitivity - find and modify the delta binding
        for (int i = 0; i < inputReferences.look_around.bindings.Count; i++)
        {
            Debug.Log($"{inputReferences.look_around.bindings[i].path}, {inputReferences.look_around.bindings[i].action}, {inputReferences.look_around.bindings[i].id}", this);

            // if (inputReferences.look_around.bindings[i].path.Contains("delta"))
            // {
            //     inputReferences.look_around.ChangeBinding(i)
            //         .WithProcessor("scaleVector2(x=0.1,y=0.1)");
            //     break;
            // }
        }
        InputBinding binding = new("<Mouse>/delta", "Look", null, "scaleVector2(x=20.0,y=20.0)");
        inputReferences.look_around.ApplyBindingOverride(binding);

        // Get and log the current screen resolution
        Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        CharacterCamera.IgnoredColliders.Clear();
        CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        HandleCharacterInput();
        DrawLogs.LogsAtGameObjectScreenspace(Character.gameObject);
    }

    private void LateUpdate()
    {
        // Handle rotating the camera along with physics movers
        if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
        {
            CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
            CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
        }

        HandleCameraInput();
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        Vector2 rotating_inputs = inputReferences.look_around.ReadValue<Vector2>();
        Vector3 lookInputVector = new(rotating_inputs.x, rotating_inputs.y, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        float scrollInput = 0;
#if UNITY_WEBGL
    scrollInput = 0f;
#endif

        // Apply inputs to the camera
        CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

    }

    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
        Vector2 moveInput = inputReferences.move.ReadValue<Vector2>();
        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = moveInput.y;
        characterInputs.MoveAxisRight = moveInput.x;
        characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
        characterInputs.JumpDown = inputReferences.jump.ReadValue<float>() > 0.5;

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}
