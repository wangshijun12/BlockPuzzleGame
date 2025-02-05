﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGroup_MinPrep : GridGroup
{
    public GridGroup_MinPrep()
    {
        G_width = GameStatic._width / 2;
        G_height = GameStatic._height / 2;
        ResName = "Prefab/blockmin";//min的格子
    }

    public override IPoolsType PoolType { get { return IPoolsType.GridGroup_MinPrep; } }
    public override IPoolsType GridType =>  IPoolsType.GridDataMin;
}
