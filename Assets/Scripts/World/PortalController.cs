using UnityEngine;
using UnityEngine.UI;

public class PortalController : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string m_sceneName;
    private Button m_telleportBtn; // Add a reference to the button
    private bool isInRange = false;

    private void Start()
    {
        // add if button pressed, teleport to targetPortal
        m_telleportBtn = GameObject.Find("Attack").GetComponent<Button>(); // Get the button component
        m_telleportBtn.onClick.AddListener(TeleportPlayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other.gameObject, true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CheckCollision(other.gameObject, false);
    }

    private void CheckCollision(GameObject gameObject, bool state)
    {
        if (gameObject.CompareTag(playerTag))
        {
            isInRange = state;
        }
    }

    private void TeleportPlayer()
    {
        if (isInRange)
        {
            PlayerManager.instance.CurrentPlayer.LastLocation = m_sceneName;
            GameManager.instance.ChangeScene(m_sceneName);
        }
    }

    private void OnDestroy()
    {
        m_telleportBtn.onClick.RemoveListener(TeleportPlayer);
    }
}
