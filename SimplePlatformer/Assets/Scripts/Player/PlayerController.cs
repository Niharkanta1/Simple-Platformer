using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float gravity = -20f;

    [Header("Collision")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayAmount;

    private BoxCollider2D boxCollider2D;
    private PlayerConditions conditions;

    private Vector2 boundBL, boundBR, boundTL, boundTR;
    private float boundsWidth, boundsHeight;

    private float currentGravity;
    private Vector2 force;
    private Vector2 movePosition;

    private float skin = 0.05f;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        conditions = new PlayerConditions();
        conditions.Reset();
    }

    void Update()
    {
        ApplyGravity();
        StartMovement();

        SetRayOrigins();
        CollisionBelow();

        transform.Translate(movePosition, Space.Self);

        SetRayOrigins();
        CalculateMovement();
        Debug.Log("Force x and y " + force.x + " " + force.y);

        Debug.DrawRay(boundBL, Vector2.left, Color.green);
        Debug.DrawRay(boundTL, Vector2.left, Color.green);
        Debug.DrawRay(boundTR, Vector2.right, Color.green);
        Debug.DrawRay(boundBR, Vector2.right, Color.green);
    }

    private void CollisionBelow()
    {
        if (movePosition.y < 0.0001f)
        {
            conditions.isFalling = true;
        }
        else
        {
            conditions.isFalling = false;
        }

        if (!conditions.isFalling)
        {
            conditions.isCollidingBelow = false;
            return;
        }

        //Calculate Ray Length
        float rayLength = boundsHeight / 2f + skin;
        if (movePosition.y < 0)
        {
            rayLength += Mathf.Abs(movePosition.y);
        }

        //Calculate Ray Origin
        Vector2 leftOrigin = (boundBL + boundTL) / 2f;
        Vector2 rightOrigin = (boundBR + boundTR) / 2f;
        leftOrigin += (Vector2)(transform.up * skin) + (Vector2)(transform.right * movePosition.x);
        rightOrigin += (Vector2)(transform.up * skin) + (Vector2)(transform.right * movePosition.x);

        //RayCast
        for (int i = 0; i < verticalRayAmount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(leftOrigin, rightOrigin, (float)i / (float)(verticalRayAmount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -transform.up, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, -transform.up * rayLength, Color.red);

            if (hit)
            {
                movePosition.y = -hit.distance + boundsHeight / 2f + skin;
                conditions.isCollidingBelow = true;
                conditions.isFalling = false;

                if (Mathf.Abs(movePosition.y) < 0.0001f)
                {
                    movePosition.y = 0f;
                }
            }
            else
            {
                conditions.isCollidingBelow = false;
            }
        }
    }

    private void CalculateMovement()
    {
        if (Time.deltaTime > 0)
        {
            force = movePosition / Time.deltaTime;
        }
    }

    private void StartMovement()
    {
        movePosition = force * Time.deltaTime;
        conditions.Reset();
    }

    public void SetHorizontalForce(float xForce)
    {
        force.x = xForce;
    }

    private void ApplyGravity()
    {
        currentGravity = gravity;
        force.y += currentGravity * Time.deltaTime;
    }

    private void SetRayOrigins()
    {
        Bounds playerBounds = boxCollider2D.bounds;
        boundBL = new Vector2(playerBounds.min.x, playerBounds.min.y);
        boundTL = new Vector2(playerBounds.min.x, playerBounds.max.y);
        boundTR = new Vector2(playerBounds.max.x, playerBounds.max.y);
        boundBR = new Vector2(playerBounds.max.x, playerBounds.min.y);

        boundsHeight = Vector2.Distance(boundBL, boundTL);
        boundsWidth = Vector2.Distance(boundTL, boundTR);
    }
}
