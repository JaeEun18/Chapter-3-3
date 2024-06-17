using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Animator playerAnimator;

    public bool isAttack = false; // ���� �� ��ǿ� �°� ���������� ����
    private float attackTime = 0;  // ����Ǵ� �ð�
    private float hitTime = 0.4f;  // ���� Ÿ�̹� ����

    private Setting setting;

    [Header("UI")]
    public Text textScore;
    public Text textGold; // Gold UI �ؽ�Ʈ �ʵ� �߰�
    public Button itemButton; // ������ ��ư �߰�
    public float itemEffectDuration = 10f; // ������ ȿ�� ���� �ð�
    private bool isItemEffectActive = false; // ������ ȿ�� Ȱ��ȭ ����

    private float itemEffectTimeLeft = 0f; // ������ ȿ�� ���� �ð�
    private float itemEffectInterval = 1f; // ������ ȿ�� ���� �ð�
    private float itemEffectTimer = 0f; // ������ ȿ�� Ÿ�̸�

    private int itemEffectScoreIncrement = 100; // ������ ȿ���� �����Ǵ� ����

    private int score = 0; // ���� ����
    private int gold = 0; // ���� ���


    // ������ ȿ���� �����Ǵ� ������ ��� ������
    private int scoreInc = 100; // ������ ȿ���� �����Ǵ� ����
    private int goldInc = 10; // ������ ȿ���� �����Ǵ� ���

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
        StartAutoClick();
        setting = GetComponent<Setting>();

        if (setting == null)
        {
            Debug.LogError("Ȯ�ο� 11");
            return;
        }

        textScore.text = setting.StringScore();
        textGold.text = gold.ToString(); // �ʱ� Gold UI �ؽ�Ʈ ����

        // ������ ��ư Ŭ�� �̺�Ʈ ���
        itemButton.onClick.AddListener(ActivateItemEffect);

    }

    // Update is called once per frame
    void Update()
    {
        MouseOnClick();

        // ������ ȿ���� Ȱ��ȭ�Ǿ� ������ ó��
        if (isItemEffectActive)
        {
            HandleItemEffect();
        }
    }

    private void MouseOnClick()
    {
        if (isAttack) // isAttack�� true��� attackTime�� Time.deltaTime�� ����
        {
            attackTime += Time.deltaTime;

            if (attackTime > hitTime) // �ٽ� Ŭ���ϱ� ���� ����
            {
                attackTime = 0f;
                isAttack = false;

                // ������ ȿ���� Ȱ��ȭ ���̸� ���� ���� ó������ ����
                if (!isItemEffectActive)
                {
                    setting.AddScore(); // ���� ����
                    UpdateGoldUI(); // Gold UI ������Ʈ
                    textScore.text = setting.StringScore(); // UI ������Ʈ
                }
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // ���콺�� Ŭ���� ��ġ���� ī�޶󿡼� ȭ���� ���� ���� ��ǥ�� �ؼ� pos ������ �ִ´�.
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Ŭ���� ��ġ�� �´��� Ȯ��
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Enemy") // ���� ���� �� Collider�� �ִٸ� �Լ� ������ ���� ���̰� �ƴϸ� ����
            {
                // �� Collider�� Enemy ��ũ��Ʈ�� ������ �ִٸ�
                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.EnemyOnClick();

                    playerAnimator.SetTrigger("Attack");

                    isAttack = true;
                }
            }
        }
    }

    private void StartAutoClick()
    {
        AutoClick autoClickScript = gameObject.AddComponent<AutoClick>();
        autoClickScript.enabled = true;
    }

    // ������ ��ư Ŭ�� �� ȣ��� �Լ�
    private void ActivateItemEffect()
    {
        if (!isItemEffectActive)
        {
            isItemEffectActive = true;
            itemEffectTimeLeft = itemEffectDuration;
            itemEffectTimer = 0f;
        }
    }

    // ������ ȿ���� Ȱ��ȭ�� ��� ȣ��� �Լ�
    private void HandleItemEffect()
    {
        itemEffectTimer += Time.deltaTime;

        // ���� �������� ���� ����
        if (itemEffectTimer >= itemEffectInterval)
        {
            itemEffectTimer = 0f;
            setting.AddScore(scoreInc); // ���� ����
            score += scoreInc; // ���� �������� �ݿ�
            textScore.text = setting.StringScore(); // UI ������Ʈ

            if (!isItemEffectActive)
            {
                int goldIncrement = scoreInc / 10 * goldInc;
                gold += goldIncrement;
                textGold.text = gold.ToString(); // Gold UI ������Ʈ
            }
        }

        itemEffectTimeLeft -= Time.deltaTime;
        if (itemEffectTimeLeft <= 0)
        {
            isItemEffectActive = false;
        }
    }

    private void UpdateGoldUI()
    {
        gold = (int)(setting.GetGold()); // setting.GetGold() ��ȯ���� ����ȯ�Ͽ� gold�� ����
        textGold.text = gold.ToString(); // Gold UI ������Ʈ
    }
}
