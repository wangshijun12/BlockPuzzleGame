﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMgr
{
    public static Object InstantiateObj(Object obj)
    {
//#if _CHECK_OPTIMIZE
//		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
//		stopwatch.Start();
//#endif
        var newobj = Object.Instantiate(obj);
        //#if _CHECK_OPTIMIZE
        //		stopwatch.Stop();
        //		if (stopwatch.ElapsedMilliseconds > 5)
        //			if(DebugMgr.CanLog()) DebugMgr.Log(string.Format("InstantiateObj {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
        //#endif
        if (newobj == null)
        {
            Log.Error("obj为空 " + obj.name);
        }
        return newobj;
    }

    public static GameObject InstantiateGameObj(GameObject obj)
    {
        return Object.Instantiate(obj);
    }
    public static GameObject InstantiateGameObj(string path)
    {
        return Object.Instantiate(LoadMainAssetAtPath(path))as GameObject; 
    }

    public static T LoadResource<T>(string path) where T : Object
    {
        return ResourceMgr.Inst.LoadRes<T>(path);
    }
    public static GameObject InsResource(string path)
    {
       return InstantiateGameObj(LoadResource<GameObject>(path));
    }
    public static Object LoadMainAssetAtPath(string path)
    {
#if UNITY_EDITOR
        //#if _CHECK_OPTIMIZE
        //		System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //		stopwatch.Start();
        //#endif
        var obj = UnityEditor.AssetDatabase.LoadMainAssetAtPath(path);
        //#if _CHECK_OPTIMIZE
        //		stopwatch.Stop();
        //		if (stopwatch.ElapsedMilliseconds > 5)
        //			if (DebugMgr.CanLog()) DebugMgr.Log(string.Format("LoadMainAssetAtPath {0} costtime = {1}", obj.name, stopwatch.ElapsedMilliseconds));
        //#endif
        if (obj==null)
        {
            Log.Error("obj为空 "+ path);
        }
        return obj;
#endif
        return null;
    }
}
