using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementJoystick : MonoBehaviour
{
    [SerializeField] private MoveJoystick movementJoystick;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button attackButton;

    private IMovementController movementController;
    private IAttackController attackController;

    public void Start()
    {
        movementController = GetComponent<IMovementController>();
        attackController = GetComponent<IAttackController>();
        movementJoystick = GameObject.Find("JoystickArea").GetComponent<MoveJoystick>();
        jumpButton = GameObject.Find("Jump").GetComponent<Button>();
        attackButton = GameObject.Find("Attack").GetComponent<Button>();

        jumpButton.onClick.AddListener(HandleJumpButtonPress);
        attackButton.onClick.AddListener(HandleAttackButtonPress);
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

    private void HandleAttackButtonPress()
    {
        attackController.Attack();
    }
}
