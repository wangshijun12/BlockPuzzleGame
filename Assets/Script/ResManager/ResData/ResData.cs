using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[ProtoBuf.ProtoContract]
public class ResData
{
	[ProtoBuf.ProtoMember(1)]
	public int _Version;
	[ProtoBuf.ProtoMember(2)]
	public ResModel[] _Data;

	public int _Count { get { return _Data != null ? _Data.Length : 0; } }
	public ResModel this[int i]
	{
		get
		{
			return _Data != null ? _Data[i] : null;
		}
	}
	public void PushUnit(ResUnit unit)
	{
		ResModel temp;
		for (int i = 0; i < _Count; i++)
		{
			temp = _Data[i];
			if (temp._ModelID == unit._ModelID)
			{
				temp.PushUnit(unit);
				return;
			}
		}
		temp = new ResModel(unit._ModelID, unit._ModelName);
		temp.PushUnit(unit);
		ResModel[] _TempDatas = new ResModel[_Count + 1];
		for (int i = 0; i < _Count; i++)
		{
			_TempDatas[i] = this[i];
		}
		_TempDatas[_Count] = temp;
		_Data = _TempDatas;
	}

	internal void CopyAbParams(ResData oldMgr)
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			for (int j = 0; j < oldMgr._Count; j++)
			{
				var old = oldMgr[j];
				if (temp._ModelID == old._ModelID)
				{
					temp.CopyAbParams(old);
					break;
				}
			}
		}
	}

	internal bool GetObj(int modelID, string artName, out ResUnit unit)
	{
		unit = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = _Data[i];
			if (temp._ModelID == modelID)
			{
				if (temp.GetObj(artName, out unit))
				{
					return true;
				}
			}
		}
		return false;
	}


	internal bool GetABForAbID(int abID, out ResAb ab)
	{
		ab = null;
		for (int i = 0; i < _Data.Length; i++)
		{
			if (_Data[i].GetABForAbID(abID, out ab))
			{
				return true;
			}
		}
		return false;
	}

	internal void ToFile(string v)
	{
		File.WriteAllText(Application.dataPath + "/reslog.txt", ToDebug());
		SaveToFile(v);
	}
    internal void SaveToFile(string v)
    {
        ProtobufTools.SerializeToFile(v, this);
    }

    public string ToDebug()
	{
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < _Count; i++)
		{
			sb.Append(this[i].ToDebug());
			sb.Append("---------------------\n");
		}
		return sb.ToString();
	}
	internal void GetObj(IArt art)
	{
		Log.Error(art.ArtName());
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];

			if ((temp._ModelID == art._ModelID))
			{
				temp.GetObj(art);
					return;
			}
			else if (art._ModelID == 0 && temp.GetObj(art))
			{
				return;
			}
		}
		Log.Error(art.AbSingleName() + "=>" + art.ArtName());
	}
	internal bool GetAb(int modelID, int downloadId, ref int _SortID, out ResAb res)
	{
		res = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (modelID == 0)
			{
				if (temp.GetAbForDownloadID(downloadId, ref _SortID, out res))
				{
					return true;
				}
			}
			else if ((temp._ModelID == modelID))
			{
				return temp.GetAbForDownloadID(downloadId, ref _SortID, out res);
			}
		}
		return false;
	}
	internal bool GetAb(int modelID, string abName, out ResAb res)
	{
		res = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			if (modelID == 0)
			{
				if (temp.GetAb(abName, out res))
				{
					return true;
				}
			}
			else if ((temp._ModelID == modelID))
			{
				return temp.GetAb(abName, out res);
			}
		}
		return false;
	}

	internal void Switch(int _ModelIndex, ResModel newResModel)
	{
		for (int i = 0; i < _Count; i++)
		{
			if (this[i]._ModelID == _ModelIndex)
			{
				_Data[i] = newResModel;
			}
		}
	}

	internal bool GetAb(string modelName, string abName, out ResAb res)
	{
		res = null;
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];

			if ((temp._ModelName == modelName))
			{
				return temp.GetAb(abName, out res);
			}
		}
		return false;
	}

	internal bool FindResUnit(string path, out ResUnit temp)
	{
		temp = null;
		for (int i = 0; i < _Count; i++)
		{
			if (this[i].FindResUnit(path, out temp))
			{
				return true;
			}
		}
		return false;
	}
	public ResModel GetModel(RES_MODEL_INDEX index)
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];

			if (temp._ModelID == (int)index)
			{
				return temp;
			}
		}
		return null;
	}
	public void UnloadAbAndObj()
	{
		for (int i = 0; i < _Count; i++)
		{
			var temp = this[i];
			temp.UnloadAbAndObj();
		}
	}
}
