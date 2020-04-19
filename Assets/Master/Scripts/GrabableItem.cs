using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableItem : Interactable
{

    public float WeightDefault;
    public float WeightCarry;
    public float GrabDistance;
    public float ResetRotSpeed;

    public Transform[] grabpoints;

    public Rigidbody2D rb;
    public bool isCarried;
    public bool isReleased;
    Quaternion StartRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartRot = transform.rotation;
        isReleased = true;
    }

    void CarryBox()
    {
        StopAllCoroutines();
        isCarried = true;
        rb.mass = WeightCarry;
        isReleased = false;

        transform.localScale = new Vector3(startsize.x + .13f, startsize.x + .13f, startsize.x + .13f);

    }

    void ReleaseBox()
    {
        if (isReleased) return;

        isCarried = false;
        rb.mass = WeightDefault;
        transform.localScale = startsize;

        ResetRotation();

    }

    public override void Interact()
    {
        if (isCarried) ReleaseBox();
        else CarryBox();
    }

    public Vector3 FindClosestPoint(Vector3 pos)
    {
        var distance = 0f;
        var memoryDistance = 0f;
        Transform pt = null;

       foreach(Transform go in grabpoints)
        {
            distance = Vector3.Distance(go.position, pos);

            if(distance < memoryDistance || memoryDistance == 0f)
            {
                memoryDistance = distance;
                pt = go;
            }
        }

        if (pt == null)
        {
            Debug.Log("No points found");
            return new Vector3(0,0,0);
        }
        Debug.Log( pt.position);
        return  pt.localPosition;
    }

    public override void InteractibleFdbck()
    {
        if (isCarried) return;

        if (isInRange)
            transform.localScale = new Vector3(startsize.x + .07f, startsize.x + .07f, startsize.x + .07f);
        else
            transform.localScale = startsize;
    }

    public void ResetRotation()
    {
        Debug.Log("StarCoco");
        StopAllCoroutines();
        StartCoroutine(RotateTo());
    }

    public IEnumerator RotateTo()
    {
        Vector2 lookAt = (Vector2)transform.position + Vector2.right;
        var TgAngle = transform.eulerAngles.z;
        Debug.Log("Set" + TgAngle);
        float AngleRad = Mathf.Atan2(lookAt.y - transform.position.y, lookAt.x - transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;

        var t = 5f;

        while (t > 0)
        {
            t -= Time.deltaTime;
            TgAngle = Mathf.MoveTowardsAngle(TgAngle, AngleDeg, ResetRotSpeed);
            Debug.Log("current" +TgAngle);
            Debug.Log("tg" + AngleDeg);

            transform.rotation = Quaternion.Euler(0, 0, TgAngle);

            yield return new WaitForEndOfFrame();
        }

        isReleased = true;
    }
}
