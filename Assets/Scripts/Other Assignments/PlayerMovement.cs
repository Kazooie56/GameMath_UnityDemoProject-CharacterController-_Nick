using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    // Should move towards the target only if it's in range

    [SerializeField] Transform target;
    [SerializeField] float followRange;

    [SerializeField] float baseSpeed; // if we want the enemy to stand still at the beginning, this should be 0
    // baseSpeed is sort of like a minimum speed

    float speed;

    [SerializeField] float maxSpeed;

    [SerializeField] float acceleration;
    [SerializeField] float deceleration;

    // Update is called once per frame

    private void Start()
    {
        speed = baseSpeed;
    }
    void Update()
    {
        float distToTarget = Vector3.Distance(transform.position, target.position);

        if (distToTarget <= followRange)
        {
            transform.LookAt(target);

            speed = Mathf.MoveTowards(speed, maxSpeed, acceleration);

            Debug.Log("In range");
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);


        }
        else
        {
            speed = Mathf.MoveTowards(speed, baseSpeed, deceleration * Time.deltaTime);

            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

            transform.LookAt(target);
            Debug.Log("Out of range");
            // do something else
        }
    }
}
