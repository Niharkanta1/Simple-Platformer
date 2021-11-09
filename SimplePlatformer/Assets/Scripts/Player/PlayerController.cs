using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private float gravity = -20f;

    [Header("Collision")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayAmount = 4;
    [SerializeField] private int horizontalRayAmount = 4;
    [SerializeField] private float fallMultiplier = 2f;

    public float Gravity => gravity;
    public PlayerConditions Conditions => conditions;
    public Vector2 Force => force;

    #region Internal
    private BoxCollider2D boxCollider2D;
    private PlayerConditions conditions;

    private Vector2 boundBL, boundBR, boundTL, boundTR;
    private float boundsWidth, boundsHeight;

    private float currentGravity;
    private Vector2 force;
    private Vector2 movePosition;

    private float skin = 0.05f;

    private bool isFacingRight { get; set; } = true;
    private float internalFaceDirection = 1f;
    private float faceDirection;

    #endregion

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
        GetFaceDirection();

        CollisionBelow();
        if (!isFacingRight)
        {
            CollisionHorizontal(-1);
        }
        else
        {
            CollisionHorizontal(1);
        }

        transform.Translate(movePosition, Space.Self);

        SetRayOrigins();
        CalculateMovement();

    }

    #region Collision

    #region Collision Below
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
                if (force.y > 0)
                {
                    movePosition.y = force.y * Time.deltaTime;
                    conditions.isCollidingBelow = false;
                }
                else
                {
                    movePosition.y = -hit.distance + boundsHeight / 2f + skin;
                }

                conditions.isCollidingBelow = true;
                conditions.isFalling = false;

                if (Mathf.Abs(movePosition.y) < 0.0001f)
                {
                    movePosition.y = 0f;
                }
            }
        }
    }
    #endregion

    #region Collision Horizontal
    private void CollisionHorizontal(int direction)
    {
        //Calculate Ray Length
        float rayLength = Mathf.Abs(force.x * Time.deltaTime) + boundsWidth / 2f + skin * 2f;

        //Calculate horizontal Ray Origin
        Vector2 topOrigin = (boundBL + boundBR) / 2f;
        Vector2 bottomOrigin = (boundTL + boundTR) / 2f;
        topOrigin += (Vector2)(transform.up * skin);
        bottomOrigin -= (Vector2)(transform.up * skin);

        //Raycast
        for (int i = 0; i < horizontalRayAmount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(topOrigin, bottomOrigin, (float)i / (float)(horizontalRayAmount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLength, collideWith);
            Debug.DrawRay(rayOrigin, direction * transform.right * rayLength, Color.cyan);

            if (hit)
            {
                movePosition.x = direction >= 0 ? hit.distance - boundsWidth / 2f - skin * 2f : -hit.distance + boundsWidth / 2f - skin * 2f;
                force.x = 0f;
            }
        }
    }

    #endregion

    #endregion

    #region Movement

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

    public void SetVerticalForce(float yForce)
    {
        force.y = yForce;
    }

    private void ApplyGravity()
    {
        currentGravity = gravity;
        if (force.y < 0)
        {
            currentGravity *= fallMultiplier;
        }

        force.y += currentGravity * Time.deltaTime;
    }

    #endregion

    #region Direction

    private void GetFaceDirection()
    {
        faceDirection = internalFaceDirection;
        isFacingRight = faceDirection == 1;
        if (force.x > 0.0001f)
        {
            faceDirection = 1f;
            isFacingRight = true;
        }
        else if (force.x < -0.0001f)
        {
            faceDirection = -1f;
            isFacingRight = false;
        }
        internalFaceDirection = faceDirection;
    }


    #endregion

    #region Ray Origins
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
    #endregion


}
