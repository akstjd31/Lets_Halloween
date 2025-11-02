using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // [Header("ī�޶�")]
    [SerializeField] private GameObject inputCamera;
    // [Header("ĳ����")]/
    [SerializeField] private GameObject inputPlayer;
    [SerializeField] private GameObject inputEnemy;
    // [Header("ȸ�� �ӵ�")]
    [SerializeField] private float spinSpeed;
    // [Header("�ִϸ��̼� ��Ʈ��")]
    [SerializeField] private Animator inputPlayerAnimator;
    [SerializeField] private Animator inputEnemyAnimator;
    // [Header("�ִϸ��̼� ������")]
    [SerializeField] private float animationDelay;
    // [Header("�� �� ���� �� �Ŵ���")]
    [SerializeField] private SetUpGameSceneManager inputGameSceneManager;
    private float delayTiemr;
    private float delayTimerForUpdate;
    private int progressNumber;
    private SetUpGameSceneManager tempManager;

    private bool isUsed = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        //Spin();
        delayTimerForUpdate += Time.deltaTime;
        // �ִϸ��̼� ���� ����
        if (delayTimerForUpdate > animationDelay) { UpdateAnimation(); }
        

        // �ż��带 ����ߴٸ� Ÿ�̸� ����
        if (isUsed) {  delayTiemr += Time.deltaTime; }
        // �����̰� ������ Ÿ�̾�� �ִϸ����� ���� �ʱ�ȭ �� ī�޶� ��ȯ �Լ� ȣ��
        if (delayTiemr > animationDelay) 
        { 
            isUsed = false;
            delayTiemr = 0;
            inputEnemyAnimator.SetBool("isClicked", false);
            inputPlayerAnimator.SetBool("isClicked", false);
            inputGameSceneManager.ChangeCamera();
        }
    }

    private void Spin()
    {
        //Debug.Log("ȸ�� ���� ��");
        inputPlayer.transform.Rotate(Vector3.up, spinSpeed *  Time.deltaTime);
        inputEnemy.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
        
    }

    public void EnemyAttack()
    {
        // ĳ���Ͱ� ī�޶� �ٶ󺸵��� ����
        //inputEnemy.transform.LookAt(inputCamera.transform);
        inputEnemyAnimator.SetBool("loop", false);
        inputEnemyAnimator.SetBool("isClicked", true);
        isUsed = true;
    }

    public void PlayerAttack()
    {
        inputPlayerAnimator.SetBool("loop", false);
        inputPlayerAnimator.SetBool("isClicked", true);
        isUsed = true;
    }

    private void Init()
    {
        // �� �� ���� �� �Ŵ��� ����
        tempManager = GetComponent<SetUpGameSceneManager>();
        isUsed = false;
    }

    private void UpdateAnimation()
    {
        // ����� ������Ʈ ��Ȳ�� �´� ���ϸ����� ���� ����
        switch (progressNumber)
        {
            case 0:
                inputEnemyAnimator.SetFloat("progressNumber", 1f);
                inputPlayerAnimator.SetFloat("progressNumber", 1f);
                progressNumber++;
                break;
            case 1:
                inputEnemyAnimator.SetFloat("progressNumber", 2f);
                inputPlayerAnimator.SetFloat("progressNumber", 2f);
                progressNumber++;
                break;
            case 2:
                inputEnemyAnimator.SetFloat("progressNumber", 3f);
                inputPlayerAnimator.SetFloat("progressNumber", 3f);
                progressNumber++;
                break;
            

        }
    }
}
