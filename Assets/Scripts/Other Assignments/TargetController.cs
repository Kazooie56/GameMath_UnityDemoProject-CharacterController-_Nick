using UnityEngine;

public class TargetController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    [SerializeField]
    float _speed;

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;

        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;

        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        inputVector.Normalize();

        transform.Translate(new Vector3(inputVector.x, inputVector.y, 0) * _speed * Time.deltaTime);

    }
}
