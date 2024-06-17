using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Animator playerAnimator;

    public bool isAttack = false; //���� �� ��ǿ� �°� ���������� ����
    private float attackTime = 0;  //����Ǵ� �ð�
    private float hitTime = 0.4f;     //���� Ÿ�̹� ����

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
            Debug.LogError("Setting ������Ʈ�� ã�� �� �����ϴ�. GameObject�� Setting ��ũ��Ʈ�� �پ� �ִ��� Ȯ���ϼ���.");
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
        if(isAttack) //isAttack true��� attackTime�� Time.deltaTime�� ����
        {
            attackTime += Time.deltaTime;

            if(attackTime > hitTime) //�ٽ� Ŭ���ϱ� ���� ����
            {
                attackTime = 0f;
                isAttack = false;

                setting.GetGold(); // ���� ����
                textScore.text = setting.StringScore(); // UI ������Ʈ
            }

            return;
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            //���콺�� Ŭ���� ��ġ���� ī�޶󿡼� ȭ���� ���� ���� ��ǥ���ؼ� pos������ �ִ´�.
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Ŭ���� ��ġ�� �´��� Ȯ��
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if(hit.collider != null && hit.collider.tag == "Enemy") //���� ������ Collider�� �ִٸ� �Լ� ������ ���ð��̰� �ƴϸ� ����
            {
                //�� Collider�� Enemy ��ũ��Ʈ�� ������ �ִٸ�
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
