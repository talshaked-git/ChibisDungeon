using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementJoystick : MonoBehaviour
{
    [SerializeField] private MoveJoystick movementJoystick;
    [SerializeField] private Button jumpButton;

    private IMovementController movementController;

    private void Awake()
    {
        this.enabled = false;
    }

    public void Init()
    {
        movementController = GetComponent<IMovementController>();
        GameObject.Find("Jump").GetComponent<Button>();
        movementJoystick = GameObject.Find("JoystickArea").GetComponent<MoveJoystick>();
        jumpButton.onClick.AddListener(HandleJumpButtonPress);
    }

    private void Update()
    {
        HandleJoystickMove();
    }

    private void HandleJoystickMove()
    {
        float horizontalMove = movementJoystick.joystickVec.x;
        movementController.Move(horizontalMove);
    }

    private void HandleJumpButtonPress()
    {
        movementController.Jump();
    }
}
