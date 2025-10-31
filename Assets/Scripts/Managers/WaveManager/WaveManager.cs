using UnityEngine;

public partial class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 만약 이걸 사용한다면...
        }
        else
        {
            Destroy(gameObject);
        }

        // 가장 중요한 웨이브 이벤트를 먼저 구독
        onWaveStarted += HandleWaveStarted;
        onWaveEnded += HandleWaveEnded;
    }

    private void OnDestroy()
    {
        // 씬 종료 시, 현재 인스턴스가 자신인 경우에만 null로 설정
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
