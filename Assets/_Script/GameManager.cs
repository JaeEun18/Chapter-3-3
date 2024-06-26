using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Animator playerAnimator;

    public bool isAttack = false; // 공격 시 모션에 맞게 눌러지도록 조절
    private float attackTime = 0;  // 진행되는 시간
    private float hitTime = 0.4f;  // 공격 타이밍 조절

    private Setting setting;

    [Header("UI")]
    public Text textScore;
    public Text textGold; // Gold UI 텍스트 필드 추가
    public Button itemButton; // 아이템 버튼 추가
    public float itemEffectDuration = 10f; // 아이템 효과 지속 시간
    private bool isItemEffectActive = false; // 아이템 효과 활성화 여부

    private float itemEffectTimeLeft = 0f; // 아이템 효과 남은 시간
    private float itemEffectInterval = 1f; // 아이템 효과 간격 시간
    private float itemEffectTimer = 0f; // 아이템 효과 타이머

    private int itemEffectScoreIncrement = 100; // 아이템 효과로 증가되는 점수

    private int score = 0; // 현재 점수
    private int gold = 0; // 현재 골드


    // 아이템 효과로 증가되는 점수와 골드 증가량
    private int scoreInc = 100; // 아이템 효과로 증가되는 점수
    private int goldInc = 10; // 아이템 효과로 증가되는 골드

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
        StartAutoClick();
        setting = GetComponent<Setting>();

        if (setting == null)
        {
            Debug.LogError("확인용 11");
            return;
        }

        textScore.text = setting.StringScore();
        textGold.text = gold.ToString(); // 초기 Gold UI 텍스트 설정

        // 아이템 버튼 클릭 이벤트 등록
        itemButton.onClick.AddListener(ActivateItemEffect);

    }

    // Update is called once per frame
    void Update()
    {
        MouseOnClick();

        // 아이템 효과가 활성화되어 있으면 처리
        if (isItemEffectActive)
        {
            HandleItemEffect();
        }
    }

    private void MouseOnClick()
    {
        if (isAttack) // isAttack이 true라면 attackTime에 Time.deltaTime를 더함
        {
            attackTime += Time.deltaTime;

            if (attackTime > hitTime) // 다시 클릭하기 위해 세팅
            {
                attackTime = 0f;
                isAttack = false;

                // 아이템 효과가 활성화 중이면 점수 증가 처리하지 않음
                if (!isItemEffectActive)
                {
                    setting.AddScore(); // 점수 증가
                    UpdateGoldUI(); // Gold UI 업데이트
                    textScore.text = setting.StringScore(); // UI 업데이트
                }
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 마우스로 클릭한 위치값을 카메라에서 화면을 보는 월드 좌표로 해서 pos 변수에 넣는다.
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 클릭한 위치값 맞는지 확인
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null && hit.collider.tag == "Enemy") // 만약 맞은 게 Collider가 있다면 함수 안으로 들어올 것이고 아니면 나감
            {
                // 그 Collider가 Enemy 스크립트를 가지고 있다면
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

    // 아이템 버튼 클릭 시 호출될 함수
    private void ActivateItemEffect()
    {
        if (!isItemEffectActive)
        {
            isItemEffectActive = true;
            itemEffectTimeLeft = itemEffectDuration;
            itemEffectTimer = 0f;
        }
    }

    // 아이템 효과가 활성화된 경우 호출될 함수
    private void HandleItemEffect()
    {
        itemEffectTimer += Time.deltaTime;

        // 일정 간격으로 점수 증가
        if (itemEffectTimer >= itemEffectInterval)
        {
            itemEffectTimer = 0f;
            setting.AddScore(scoreInc); // 점수 증가
            score += scoreInc; // 현재 점수에도 반영
            textScore.text = setting.StringScore(); // UI 업데이트

            if (!isItemEffectActive)
            {
                int goldIncrement = scoreInc / 10 * goldInc;
                gold += goldIncrement;
                textGold.text = gold.ToString(); // Gold UI 업데이트
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
        gold = (int)(setting.GetGold()); // setting.GetGold() 반환값을 형변환하여 gold에 저장
        textGold.text = gold.ToString(); // Gold UI 업데이트
    }
}
