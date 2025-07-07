using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("이동 속도")] private float moveSpeed;
    [SerializeField] private Rigidbody2D rigid;

    private void Reset()
    {
        rigid = this.GetComponent<Rigidbody2D>();

        if (rigid == null)
        {
            rigid = this.AddComponent<Rigidbody2D>();
            rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //test
            var jumpPos = new Vector2(3, 8);
            Jump(jumpPos); //3 //size
        }
    }

    private void Jump(Vector2 _jumpPos)
    {
        var gravity = Mathf.Abs(Physics2D.gravity.y);           //중력 가속
        _jumpPos.y = Mathf.Sqrt(_jumpPos.y * gravity * 2.5f);   //거리 * 속도 * 2 (*2.5f)

        var sideMoveTime = (_jumpPos.y / gravity) * 2f;         //x축으로 이동 시간
        _jumpPos.x = (_jumpPos.x / sideMoveTime) * 1.25f;       //속도 = 거리 / 시간 (*추가 이동)

        rigid.linearVelocity = _jumpPos;
    }
}
