using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    private Animator playerAnimator;

    private bool isAttack = false; //���� �� ��ǿ� �°� ���������� ����
    private float attackTime = 0;  //����Ǵ� �ð�
    [SerializeField]
    private float hitTime = 0;     //���� Ÿ�̹� ����

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>(); 
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

            if(attackTime > hitTime)
            {
                attackTime = 0f;
                isAttack = false;
            }
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
                }
            }

        }
    }
}
