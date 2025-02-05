﻿using System.Collections;
using UnityEngine;

public class CaneraShaker : UIEventListenBase
{
    Vector3 originalV3;
    Vector3 changeV3;
    void Start()
    {
        originalV3 = transform.position;
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            changeV3.x = Random.Range(-1f, 1f) * magnitude;
            changeV3.y = Random.Range(-1f, 1f) * magnitude;
            transform.position = changeV3;
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalV3;
    }
    public void PlayShake()
    {
        Log.Error("shake");
        StartCoroutine(Shake(.15f, 20));
    }
    public override void InitEventListen()
    {
        messageIds = new ushort[]{
            (ushort)CaneraShakeListenID.Shake,
        };
        RegistEventListen(this, messageIds);
        base.InitEventListen();
    }
    public override void ProcessEvent(MessageBase tmpMsg)
    {
        switch (tmpMsg.messageId)
        {
            case (ushort)CaneraShakeListenID.Shake:
                PlayShake();
                break;
            default:
                break;
        }
        base.ProcessEvent(tmpMsg);
    }
}
