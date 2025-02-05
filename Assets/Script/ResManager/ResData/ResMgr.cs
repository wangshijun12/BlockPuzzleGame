using System;
using System.Collections.Generic;
using System.Diagnostics;

public class ResMgr
{
	public int _Version { get { return _Data._Version; } }
	public ResData _Data;
	public ResMgr()
	{
		_Data = new ResData();
	}
	public ResMgr(byte[] data)
	{
		if (data != null)
		{
			_Data = ProtobufTools.Deserialize<ResData>(data);
		}
	}
	public void PushUnit(ResUnit unit)
	{
		_Data.PushUnit(unit);
	}
	internal void GetObj(IArt art)
	{
		_Data.GetObj(art);
	}
	/// <summary>
	/// Editor模型 调用
	/// </summary>
	/// <param name="_ModelName"></param>
	/// <param name="_AbName"></param>
	/// <param name="ab"></param>
	/// <returns></returns>
	public bool GetAB(string _ModelName, string _AbName, out ResAb ab)
	{
		return _Data.GetAb(_ModelName, _AbName, out ab);
	}
	public bool GetAB(int _ModelID, int _DownLoadId, ref int _SortID, out ResAb ab)
	{
		return _Data.GetAb(_ModelID, _DownLoadId, ref _SortID, out ab);
	}
	internal bool GetObj(int modelID, string _AbName, out ResUnit unit)
	{
		return _Data.GetObj(modelID, _AbName, out unit);
	}

	public bool GetAB(int _ModelID, string _AbName, out ResAb ab)
	{
		return _Data.GetAb(_ModelID, _AbName, out ab);
	}
	public bool GetABForAbID(int _AbID, out ResAb ab)
	{
		return _Data.GetABForAbID(_AbID, out ab);
	}
	public bool FindResUnit(string path, out ResUnit temp)
	{
		return _Data.FindResUnit(path, out temp);
	}

	public void ToFile(string v)
	{
		_Data.ToFile(v);
        SaveToFile(v);
    }
	public void SaveToFile(string v)
	{
        _Data.SaveToFile(v);
	}
	public void CopyAbParams(ResMgr oldMgr)
	{
		if (oldMgr._Data != null)
		{
			VersionAdd(oldMgr._Version);
			_Data.CopyAbParams(oldMgr._Data);
		}
	}
	void VersionAdd(int oldVersion)
	{
		_Data._Version = oldVersion + 1;
	}

	public int[] GetAbs(string[] depens)
	{
		List<int> _list = new List<int>(depens.Length);
		for (int i = 0; i < depens.Length; i++)
		{
			var sp = depens[i].Split('.')[0].Split('/');
			if (GetAB(GetModelName(sp), GetAbName(sp), out ResAb ab))
			{
				_list.Add(ab._ID);
			}
			else
			{
                Log.Error("逻辑上出现问题了，不能查不到数据 ab path=" + depens[i]);
			}
		}
		return _list.ToArray();
	}
	static string GetModelName(string[] sp)
	{
		return sp.Length == 1 ? "" : sp[0];
	}
	static string GetAbName(string[] sp)
	{
		return sp[sp.Length == 1 ? 0 : 1];
	}
	public ResModel GetModel(RES_MODEL_INDEX index)
	{
		return _Data.GetModel(index);
	}
	public void UnloadAbAndObj()
	{
		_Data.UnloadAbAndObj();
	}

}
