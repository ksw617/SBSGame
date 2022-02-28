using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour, InputController.IInputActions
{
    [SerializeField] private PathFinding pathFinding;

    private InputController inputController;

    public PlayerController playerController;

    void Awake()
    {
        pathFinding = GetComponent<PathFinding>();

        inputController = new InputController();
        inputController.Input.SetCallbacks(this);
    }

    void OnEnable()
    {
        inputController.Input.Enable();
    }

    void OnDisable()
    {
        inputController.Input.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                playerController.FollowPath(hit.point);
            }

        }
    }
}
