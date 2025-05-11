using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BirdCircularFlight : MonoBehaviour
{
    [Header("Flight Settings")]
    public float radius = 5f;
    public float speed = 1f;
    public Vector3 centerOffset = Vector3.zero;

    private float angle;
    private Vector3 centerPoint;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        centerPoint = transform.position + centerOffset;

        if (animator != null)
        {
            animator.Play("Scene");
        }
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 newPos = new Vector3(x, 0f, z) + centerPoint;
        transform.position = newPos;

        Vector3 direction = new Vector3(-Mathf.Sin(angle), 0f, Mathf.Cos(angle));
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
