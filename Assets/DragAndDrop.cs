using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 mousePosition;
    private bool insideBasket = false;
    private bool isTouchingEdge = false;
    private bool isDragging = false;

    public GameObject basket;
    public GameObject ball;
    public int requiredScore = 1;
    public int currentScore = 0;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        transform.position = new Vector3(3f, -3f, 25f);
    }

    void SetStartPosition()
    {
        isDragging = false;
        transform.position = new Vector3(3f, -3f, 25f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        insideBasket = false;
        ball.transform.position = new Vector3(3f, -3f, 25f);
        currentScore += 1;
        if (currentScore >= requiredScore)
        {
            currentScore = 0;
            requiredScore += 1;
        }
        Debug.Log("Current Score: " + currentScore + "\nRequired Score: " + requiredScore);
    }

    void CheckScreenBounds()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);

        if (viewPos.x <= 0 || viewPos.x >= 1 || viewPos.y <= 0 || viewPos.y >= 1)
        {
            isTouchingEdge = true;
        }
    }

    public void IsBasket()
    {
        if (!insideBasket)
        {
            var basketPosition = basket.transform.position;
            var ballPosition = ball.transform.position;
            var distance = new Vector3(.2f, .2f, .2f);
            var leftBasketArea = basketPosition - distance;
            var rightBasketArea = basketPosition + distance;

            if (
                ballPosition.x >= leftBasketArea.x && ballPosition.x <= rightBasketArea.x &&
                ballPosition.y >= leftBasketArea.y && ballPosition.y <= rightBasketArea.y
            )
            {
                insideBasket = true;
            }
        }
        else
        {
            var basketPosition = basket.transform.position;
            var ballPosition = ball.transform.position;
            var distance = new Vector3(.2f, .2f, .2f);
            var leftBasketArea = basketPosition - distance;
            var rightBasketArea = basketPosition + distance;

            if (
                ballPosition.x < leftBasketArea.x || ballPosition.x > rightBasketArea.x ||
                ballPosition.y < leftBasketArea.y || ballPosition.y > rightBasketArea.y
            )
            {
                insideBasket = false;
                SetStartPosition();
            }
        }
    }

    private Vector3 GetMousePos()
    {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        mousePosition = Input.mousePosition - GetMousePos();
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            newPosition.z = transform.position.z;

            if (newPosition.y < -3)
            {
                var targetPosition = new Vector3(newPosition.x, -2.81f, newPosition.z);
                transform.position = targetPosition;
            }
            else
            {
                transform.position = newPosition;
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;

        // Reset ball's velocity and angular velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        IsBasket();

        if (Input.GetMouseButtonUp(0))
        {
            if (insideBasket)
            {
                insideBasket = false;
                SetStartPosition();
            }
        }
        CheckScreenBounds();

        if (isTouchingEdge)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; // Reset angular velocity as well
            isTouchingEdge = false;
        }
    }
}
