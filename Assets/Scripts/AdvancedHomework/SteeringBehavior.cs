using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior
{
    public static float Speed = 0.0f;
    public static float ArriveRange = 10.0f;
    public static float MaxSpeed = 2.5f;
    public static float MaxRotate = 5.0f;

    /*private static SteeringBehavior _singleton = null;
    public static SteeringBehavior GetSingleton { get { return _singleton ??= new(); } }*/

    public Vector3 TargetPosition { get; set; } = Vector3.zero;

    public void GetPositionFromTargetNode(Vector3 target)
    {
        TargetPosition = target;
    }

    public void Seek(GameObject character)
    {
        Transform t = character.transform;
        Vector3 cPos = t.position;
        Vector3 vFor = t.forward;
        Vector3 vec = TargetPosition - cPos;
        vec.y = 0.0f;
        float fDist = vec.magnitude;
        vec.Normalize();
        float fLimit = Speed * Time.deltaTime + 0.01f;
        if (fDist < fLimit)
        {
            Vector3 tPos = TargetPosition;
            tPos.y = cPos.y;
            t.position = tPos;
            //t.forward = vec;
            return;
        }

        float fDot = Vector3.Dot(vec, vFor);
        if (fDot > 1.0f)
        {
            fDot = 1.0f;
            t.forward = vec;
        }
        else
        {
            float fSinLen = Mathf.Sqrt(1 - fDot * fDot);
            //float fRad = Mathf.Acos(fDot);
            //fSinLen = Mathf.Sin(fRad);
            float fDot2 = Vector3.Dot(t.right, vec);

            float fMag = 0.1f;
            if (fDot < 0)
            {
                fMag = 1.0f;
            }
            if (fDot2 < 0)
            {
                fMag = -fMag;
            }

            vFor += fMag * fSinLen * t.right;
            vFor.Normalize();
            t.forward = vFor;
        }


        // Move.
        float fForForce = fDot;
        float fRatio = 1.0f;
        float fAcc = fForForce * fRatio;
        float fAcc2 = fDist / ArriveRange;
        if (fAcc2 > 1.0f)
        {
            fAcc2 = 1.0f;
        }
        else
        {
            fAcc2 = -(1.0f - fAcc2);
        }

        Speed = Speed + fAcc * Time.deltaTime + fAcc2 * Time.deltaTime;
        if (Speed > MaxSpeed)
        {
            Speed = MaxSpeed;
        }
        else if (Speed < 0.01f)
        {
            Speed = 0.01f;
        }
        t.position = cPos + Speed * Time.deltaTime * t.forward;
    }
}
