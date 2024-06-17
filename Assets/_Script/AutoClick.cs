using System.Collections;
using UnityEngine;

public class AutoClick : MonoBehaviour
{
    private GameManager gameManager;

    private int clickCount = 0;
    private const int autoAttackInterval = 5; // 5번째 클릭마다 자동 공격
    private const float autoAttackDelay = 0.3f; // 자동 공격 후 대기 시간

    private bool canAutoAttack = false; // 자동 공격 허용 여부를 나타내는 플래그

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(EnableAutoAttack());
    }

    private IEnumerator EnableAutoAttack()
    {
        clickCount = 0; // 게임 시작 시 클릭 횟수 초기화

        // 게임 시작 후 처음 5번의 클릭을 대기
        while (clickCount < autoAttackInterval)
        {
            yield return null; // 대기
        }

        // 5번 클릭 이후 자동 공격 허용
        canAutoAttack = true;

        // 자동 공격 실행
        StartCoroutine(AutoAttackCoroutine());
    }

    void Update()
    {
        // 마우스 클릭을 감지하여 클릭 횟수 증가
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            // 5번째 클릭일 때 자동으로 공격
            if (clickCount % autoAttackInterval == 0 && canAutoAttack)
            {
                StartCoroutine(AutoAttackCoroutine());
            }
        }
    }

    private IEnumerator AutoAttackCoroutine()
    {
        // 임의의 화면 좌표를 생성하여 월드 좌표로 변환
        Vector2 pos = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
        pos = Camera.main.ScreenToWorldPoint(pos);

        // 플레이어를 중심으로 Ray를 쏴서 적을 탐지
        RaycastHit2D hit = Physics2D.Raycast(gameManager.player.transform.position, pos - (Vector2)gameManager.player.transform.position);

        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            // 적을 공격
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyOnClick();
                gameManager.playerAnimator.SetTrigger("Attack"); // 플레이어의 공격 애니메이션 재생
                gameManager.isAttack = true; // GameManager에 공격 중임을 알림
            }
        }

        // 자동 공격 후 대기 시간
        yield return new WaitForSeconds(autoAttackDelay);

        // 다음 자동 공격 준비
        if (canAutoAttack)
        {
            StartCoroutine(AutoAttackCoroutine());
        }
    }
}
