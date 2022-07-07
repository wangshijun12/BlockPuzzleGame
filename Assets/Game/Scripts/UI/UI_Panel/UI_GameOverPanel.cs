﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WUtils;

public class UI_GameOverPanel : UIBase, IPoolable
{
    public override string WndName => IPoolsType.UI_GameOverPanel.ToString();

    public override UIHideType hideType => UIHideType.WaitDestroy;

    public override UIHideFunc hideFunc => UIHideFunc.MoveOutOfScreen;

    public override int layer { get => (int)UILayer.Panel; set => layer = value; }

    public override bool isFull => false;

    public IPoolsType GroupType => IPoolsType.UI_GameOverPanel;
    public override int orderInLayer { get => 10; set => orderInLayer = value; }
    public bool IsRecycled { get ; set; }

    public void OnRecycled()
    {
       
    }
    UI_GameOverPanelJob paneljob;
    public override void OnCreate()
    {
        paneljob = WndRoot.AddMissingComponent<UI_GameOverPanelJob>();

    }
    public override void OnShow()
    {
        paneljob.ShowGameOver();
    }
    public override void UnRegistEvents()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
