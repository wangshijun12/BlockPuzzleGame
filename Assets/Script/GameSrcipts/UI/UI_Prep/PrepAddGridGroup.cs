﻿using System;
using UnityEngine;
using DG.Tweening;
public class PrepAddGridGroup : MonoBehaviour
{
    public int Index;
    bool isdrag;//判断是否执行过拖动函数
    bool isdraging;//放手的时候判断是否抓起过,抓起过的不进入点击事件
    public Transform Root;
    public bool IsUse;
    bool canuse = true;
    public bool IsCanUse
    {
        get { return canuse; }
        set
        {
            if (canuse != value)
            {
                Log.Info("cant use   " + transform.name);
                minPrepGroup.SetCanUseStatus(value);//设置要不要变灰的表现
            }
            canuse = value;
        }
    }
    [SerializeField]
    public GridGroup_MinPrep minPrepGroup { get; private set; }
    public int[,] rotatePrep;
    // Start is called before the first frame update
    void Start()
    {
        EventTriggerListener.Get(gameObject).onDown = OnPointerDown;
        EventTriggerListener.Get(gameObject).onUp = OnPointerUp;
        EventTriggerListener.Get(gameObject).onDrag = OnDrag;
        EventTriggerListener.Get(gameObject).onClick = OnClickGroup;
    }
    /// <summary>
    /// 还原旋转
    /// </summary>
    public void BackRotate()
    {
        if (transform.localEulerAngles.z!=0)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        if (minPrepGroup != null)
            rotatePrep = minPrepGroup.DataArray;
    }
    private void OnClickGroup(GameObject obj)
    {
        Log.Info("OnClick   " + transform.name);
        if (!isdraging && GameStatic.IsRotateState) //如果 没有被抓起来 并且当前是 旋转的状态 
        {
            if (M_math.NeedToRotate(minPrepGroup.DataArray))
            {
                transform.DOKill(true);//强制完成旋转
                transform.DOLocalRotate(transform.localEulerAngles - Vector3.forward * 90, .2f);
                rotatePrep = M_math.Rotate_90(rotatePrep);//点击后 执行 旋转rotatePrep数据  之后再执行能不能放置
                if (GridGroupMgr.Inst.IsCanPrepNext())
                { }
            }
            else
            {
                Log.Error("不需要旋转");
            }
        }
        //再放置成功之后判断是否旋转后跟没旋转前是否相同，不相同则减掉一个旋转用的金币
    }

    void UsePrepGridGroup()
    {
        IsUse = true;
        if (GameStatic.IsRotateState)
        {
            MsgSend.ToSend((ushort)UIGroupRotateListenID.HideOne, Index);
        }
        Recycle();
        //三个格子都用完了，刷新三个待放入的格子
        if (GridGroupMgr.Inst.IsCantUseAllPrep())
        {
            GridGroupMgr.Inst.RefreshPrepGridGroup();
        }
        //就算三个用完重新刷新 也需要判断能不能放置
        IsGameOver();
    }
    void IsGameOver()
    {
        if (!GridGroupMgr.Inst.IsCanPrepNext())
        {
            Log.Error("游戏结束");
            AllUIPanelManager.Inst.Show(IPoolsType.UI_GameOverPanel);
        }
    }
    public void SetGridData(GridGroup_MinPrep v)
    {
        minPrepGroup = v;
        rotatePrep = v.DataArray;
        if (GameStatic.IsRotateState)
        {
            MsgSend.ToSend((ushort)UIGroupRotateListenID.SwOne, Index);
        }
    }
    void SetChildActive(bool sw)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(sw);
        }
    }
    public void OnPointerUp(GameObject eventData)
    {
        //CaneraShaker.Inst.PlayShake();测试代码
        Log.Info("OnPointerUp   " + transform.name);
        StopTime();
        if (!isdraging)
        {
            return;
        }
        if (IsUse)//
        {
            return;
        }
        DragingGridMgr.Inst.SetDragUp(this);
        if (GridGroupMgr.Inst.RefreshMainGrid())//如果当前可以放置 刷新主面板显示
        {
            AudioMgr.Inst.PlayPlace();
            //如果是旋转过的状态 处理旋转所需的金币值，当值达到0时，关闭旋转开关
            if (GameStatic.IsRotateState && !M_math.IsSameArrays(rotatePrep, minPrepGroup.DataArray))
            {
                MsgSend.ToSend((ushort)UIGroupRotateListenID.AddRotateGold, -1);
            }
            UsePrepGridGroup();//设置当前待放入的group为使用过了
        }
        else
        {
            AudioMgr.Inst.PlayReturn();
            SetChildActive(true);//使用失败 跑一个回到原始位置的动画
        }
    }
    bool opentime = false;
    float timer = 0.2f;
    private void Update()
    {
        if (opentime)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                UpDoSome();
            }
        }
    }
    void StopTime()
    {
        opentime = false;
        timer = 0.2f;
    }
    void StartTime()
    {
        opentime = true;
        timer = 0.2f;
    }
    void UpDoSome()
    {
        if (opentime)
        {
            DragDown();
        }
        StopTime();
    }
    public void OnPointerDown(GameObject eventData)
    {
        isdrag = false;
        isdraging = false;
        Log.Info("OnPointerDown   " + transform.name);
        //return;
        if (IsUse)//
        {
            return;
        }
        if (!IsCanUse)
        {
            return;
        }
        if (GameStatic.IsRotateState)
        {
            StartTime();  //长按识别 抓起的组
        }
        else
        {
            DragDown();
        }
    }
    void DragDown()
    {
        AudioMgr.Inst.PlayPick();
        DragingGridMgr.Inst.SetDragDown(this);
        SetChildActive(false);
        isdraging = true;
    }
    
    void OnDrag(GameObject eventData)
    {
        if (!isdrag && isdraging)
        {
            Log.Info("OnDrag " + transform.name);
            DragingGridMgr.Inst.SetDrag(true);
            isdrag = true;
        }
        //调整旋转的状态  拖动的时候才开始
        //Debug.Log("OnDrag " + transform.name);
    }
    void Recycle()
    {
        if (minPrepGroup != null && !minPrepGroup.IsRecycled)
        {
            PoolMgr.Recycle(minPrepGroup);
            minPrepGroup = null;
        }
    }
    public void Reset()
    {
        BackRotate();
        Recycle();
        IsUse = false;
        canuse = true;
    }
}
