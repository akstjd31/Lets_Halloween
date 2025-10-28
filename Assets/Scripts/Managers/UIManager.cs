using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button preparingButton;

    private void Start()
    {
        preparingButton.onClick.AddListener(GameManager.Instance.OnClickReadyButton);
    }
}
