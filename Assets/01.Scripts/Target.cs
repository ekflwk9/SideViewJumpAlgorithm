using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
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
}
