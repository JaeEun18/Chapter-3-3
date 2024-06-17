using System.Collections;
using UnityEngine;

public class AutoClick : MonoBehaviour
{
    private GameManager gameManager;

    private int clickCount = 0;
    private const int autoAttackInterval = 5; // 5��° Ŭ������ �ڵ� ����
    private const float autoAttackDelay = 0.3f; // �ڵ� ���� �� ��� �ð�

    private bool canAutoAttack = false; // �ڵ� ���� ��� ���θ� ��Ÿ���� �÷���

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(EnableAutoAttack());
    }

    private IEnumerator EnableAutoAttack()
    {
        clickCount = 0; // ���� ���� �� Ŭ�� Ƚ�� �ʱ�ȭ

        // ���� ���� �� ó�� 5���� Ŭ���� ���
        while (clickCount < autoAttackInterval)
        {
            yield return null; // ���
        }

        // 5�� Ŭ�� ���� �ڵ� ���� ���
        canAutoAttack = true;

        // �ڵ� ���� ����
        StartCoroutine(AutoAttackCoroutine());
    }

    void Update()
    {
        // ���콺 Ŭ���� �����Ͽ� Ŭ�� Ƚ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;

            // 5��° Ŭ���� �� �ڵ����� ����
            if (clickCount % autoAttackInterval == 0 && canAutoAttack)
            {
                StartCoroutine(AutoAttackCoroutine());
            }
        }
    }

    private IEnumerator AutoAttackCoroutine()
    {
        // ������ ȭ�� ��ǥ�� �����Ͽ� ���� ��ǥ�� ��ȯ
        Vector2 pos = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
        pos = Camera.main.ScreenToWorldPoint(pos);

        // �÷��̾ �߽����� Ray�� ���� ���� Ž��
        RaycastHit2D hit = Physics2D.Raycast(gameManager.player.transform.position, pos - (Vector2)gameManager.player.transform.position);

        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            // ���� ����
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyOnClick();
                gameManager.playerAnimator.SetTrigger("Attack"); // �÷��̾��� ���� �ִϸ��̼� ���
                gameManager.isAttack = true; // GameManager�� ���� ������ �˸�
            }
        }

        // �ڵ� ���� �� ��� �ð�
        yield return new WaitForSeconds(autoAttackDelay);

        // ���� �ڵ� ���� �غ�
        if (canAutoAttack)
        {
            StartCoroutine(AutoAttackCoroutine());
        }
    }
}
