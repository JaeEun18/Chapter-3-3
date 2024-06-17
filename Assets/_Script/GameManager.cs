using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Animator playerAnimator;

    public bool isAttack = false; //공격 시 모션에 맞게 눌러지도록 조절
    private float attackTime = 0;  //진행되는 시간
    private float hitTime = 0.4f;     //공격 타이밍 조절

    private Setting setting;

    [Header("UI")]
    public Text textScore;


    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
        StartAutoClick();
        setting = GetComponent<Setting>();

        if (setting == null)
        {
            Debug.LogError("Setting 컴포넌트를 찾을 수 없습니다. GameObject에 Setting 스크립트가 붙어 있는지 확인하세요.");
            return;
        }

        textScore.text = setting.StringScore();

    }


    // Update is called once per frame
    void Update()
    {
        MouseOnClick();
    }

    private void MouseOnClick()
    {
        if(isAttack) //isAttack true라면 attackTime에 Time.deltaTime를 더함
        {
            attackTime += Time.deltaTime;

            if(attackTime > hitTime) //다시 클릭하기 위해 세팅
            {
                attackTime = 0f;
                isAttack = false;

                setting.GetGold(); // 점수 증가
                textScore.text = setting.StringScore(); // UI 업데이트
            }

            return;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            //마우스로 클릭한 위치값을 카메라에서 화면을 보는 월드 좌표로해서 pos변수에 넣는다.
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //클릭한 위치값 맞는지 확인
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if(hit.collider != null && hit.collider.tag == "Enemy") //만약 맞은게 Collider가 있다면 함수 안으로 들어올것이고 아니면 나감
            {
                //그 Collider가 Enemy 스크립트를 가지고 있다면
                Enemy enemy = hit.collider.GetComponent<Enemy>();

                if(enemy != null)
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
}
