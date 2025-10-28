using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button preparingButton;
    [SerializeField] private TextMeshProUGUI waveText;

    private void Start()
    {
        preparingButton.onClick.AddListener(GameManager.Instance.OnClickReadyButton);
        
        WaveManager.Instance.onWaveStarted += OnWaveStarted;
        WaveManager.Instance.onWaveEnded += OnWaveEnded;
    }

    private void OnWaveStarted()
    {
        Debug.Log("웨이브 시작");
    }

    private void OnWaveEnded()
    {
        Debug.Log("웨이브 종료");
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
        {
            WaveManager.Instance.onWaveStarted -= OnWaveStarted;
            WaveManager.Instance.onWaveEnded -= OnWaveEnded;
        }
    }
}
