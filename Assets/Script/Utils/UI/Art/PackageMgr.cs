using System.Collections.Generic;
using UnityEngine;

public class PackageMgr:Singleton<PackageMgr>
{
	static readonly Dictionary<string, Object> PackageLoad = new Dictionary<string, Object>();
	static readonly Dictionary<string, UiWndArt> _ResourceDatas = new Dictionary<string, UiWndArt>();
	public PackageMgr()
	{
		TimeMgr.Instance.AddIntervelEvent(TimerClear, 10000, -1);
	}
    public static void LoadObjectCallBack(string packageName, System.Action<string> cbv, bool unClear = false)
    {
        LoadObject(packageName, () => { cbv(packageName); }, unClear);
    }
    /// <summary>
    /// 回调CallBack
    /// </summary>
    /// <param name="packageName"></param>
    /// <param name="cbv"></param>
    /// <param name="unClear"></param>
    static void LoadObject(string packageName, System.Action cbv, bool unClear = false)
	{
		if (string.IsNullOrEmpty(packageName))
		{
			Log.Error("资源包名BUG");
			return;
		}
		if (IsLoaded(packageName))
		{
			MarkResource(packageName);
			cbv();
		}
		else
		{
			if (_ResourceDatas.TryGetValue(packageName, out UiWndArt art))
			{
				art.AddCb(cbv);
			}
			else
			{
				_ResourceDatas.Add(packageName, new UiWndArt(packageName, cbv, unClear));
                if (!AppParam.LoadArtIsAb)
                {
                    cbv();
                }
            }
		}
	}
    public static Object GetObject(string packageName, string resName)
    {
        if (IsLoaded(packageName))
        {
            if (_ResourceDatas.TryGetValue(packageName, out UiWndArt temp))
            {
                return temp.GetRes(resName);
            }
            return null;
        }
        else
        {
            Log.Error("资源包未加载 + " + packageName);
            return null;
        }
    }
    public static Object CreateObject(string packageName, string resName)
    {
        if (IsLoaded(packageName))
        {
            //生成ui
            if (_ResourceDatas.TryGetValue(packageName, out UiWndArt temp))
            {
                return ObjectMgr.InstantiateObj(temp.GetRes(resName));
            }
            foreach (var item in _ResourceDatas)
            {
                Log.Error(item.Key);
            }
            Log.Error("没有资源 " + packageName + "    " + resName);
            return null;
        }
        else
        {
            Log.Error("资源包未加载 + " + packageName);
            return null;
        }
    }
    public static void AddLoadPackage(string packageName,Object obj)
    {
        PackageLoad[packageName] = obj;
    }
    public static void RemoveLoadPackage(string packageName)
    {
        if (PackageLoad.TryGetValue(packageName,out Object obj))
        {
            PackageLoad.Remove(packageName);
        }
    }
    public static void RemoveAll()
	{
		var e = _ResourceDatas.GetEnumerator();
		while (e.MoveNext())
		{
			if (IsLoaded(e.Current.Key))
                RemoveLoadPackage(e.Current.Key);
		}
		_ResourceDatas.Clear();
		e.Dispose();
	}
    static bool IsLoaded(string wnd)
	{
        return PackageLoad.ContainsKey(wnd);
	}
	public static void RemovePackage(string packageName)
	{
		Inst.ClearMark(packageName);
	}
	/// <summary>
	/// 加载一次标记
	/// </summary>
	/// <param name="packageName"></param>
	/// <param name="onlyLoad"></param>
	static void MarkResource(string packageName)
	{
		if (_ResourceDatas.TryGetValue(packageName, out UiWndArt temp))
			temp.Mark();
	}
	/// <summary>
	/// 卸载一次标记
	/// </summary>
	/// <param name="packageName"></param>
	void ClearMark(string packageName)
	{
		if (_ResourceDatas.TryGetValue(packageName, out UiWndArt temp))
			temp.ClearMark();
	}

	/// <summary>
	/// 计时器卸载闲置资源
	/// </summary>
	/// <param name="t1"></param>
	/// <param name="t2"></param>
	void TimerClear(int t1, float t2)
	{
		var e = _ResourceDatas.GetEnumerator();
		while (e.MoveNext())
		{
			var value = e.Current.Value;
			if (!value.NeedClear()) continue;
			var key = e.Current.Key;
			if (IsLoaded(key))
			{
				value.RemoveData();
				_ResourceDatas.Remove(key);
				break;
			}
		}
		e.Dispose();
	}
}
