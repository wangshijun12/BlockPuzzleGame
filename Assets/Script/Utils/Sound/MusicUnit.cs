using UnityEngine;
using System;
using System.Collections.Generic;

public class MusicUnit : ArtBase
{    
    public AudioClip m_clip { get; private set; }
    string m_artName;
    public int m_lastTime { get; private set; }
    int m_curTime;    
    Queue<int> m_delayTime;
    Queue<sbyte> m_delayVolume;
    public SoundType m_type { get; private set; }
    public sbyte volum { get; private set; }


    public MusicUnit(string _artName, SoundType _Type)
    {
        m_artName = _artName;
        m_type = _Type;
        InitDelayParams();
        MsgSend.GetRes(this);
    }
    void InitDelayParams()
    {
        m_delayTime = new Queue<int>();
        m_delayVolume = new Queue<sbyte>();
    }

    public override string AbSingleName()
    {
        return "sound";
    }

    public override string ArtName()
    {
        return m_artName;
    }

    public bool IsWaitArt(ushort key = 0)
    {
        return true;
    }
    public override IEnumerator<float> Loading(AssetBundle ab)
    {
        yield return 0;
    }
    //public IEnumerator<float> Loading(AssetBundle ab, ResData _data)
    //{
    //    if (_data.objs == null)
    //    {
    //        _data.objs = new Dictionary<string, UnityEngine.Object>();
    //    }
    //    _data.DelayClearObjs(120);
    //    var tmp = _data.objNode;
    //    if (tmp != null)
    //    {
    //        do
    //        {
    //            MusicUnit value = tmp.Value as MusicUnit;
    //            if (value != null && value.IsWaitArt(_data.key))
    //            {
    //                var _artName = value.ArtName();
    //                if (_artName != null && !_data.objs.ContainsKey(_artName))
    //                {
    //                    var tmpresult = ab.LoadAssetAsync(_artName);
    //                    yield return MEC.Timing.WaitUntilDone(tmpresult);
    //                    if (tmpresult.asset != null)
    //                    {
    //                        AudioClip clip = tmpresult.asset as AudioClip;
    //                        if (clip.loadState == AudioDataLoadState.Unloaded)
    //                        {
    //                            clip.LoadAudioData();
    //                        }
    //                        while (clip.loadState == AudioDataLoadState.Loading)
    //                        {
    //                            yield return 0;
    //                        }
    //                        _data.objs.Add(_artName, clip);
    //                    }
    //                }
    //            }
    //            tmp = tmp.next;
    //        } while (tmp != null);
    //    }
    //    _data.FeedBack();
    //    ab.Unload(false);
    //    _data.DelayClearObjs(120);
    //}


    public override void UseArt(object obj)
    {
        ////UnityEngine.Object o = null;
        ////if (((ResData)obj).objs.TryGetValue(ArtName(), out o))
        ////{
        ////    m_clip = o as AudioClip;            
        ////}
        
    }

    internal void Play(int v,sbyte volume=10)
    {
        if (m_delayTime.Count<3)//��������̫��
        {
            m_delayTime.Enqueue(v);
            m_delayVolume.Enqueue(volume);
        }        
        m_lastTime = TimeMgr.Instance._MsTime;
    }
    public int GetTop()
    {
        if (m_curTime == 0)
        {
            if (m_delayTime.Count > 0)
            {
                m_curTime = m_delayTime.Dequeue();
                volum=m_delayVolume.Dequeue();
            }
            else if(m_lastTime==0)
            {
                m_lastTime = TimeMgr.Instance._MsTime;
            }
        }        
        return m_curTime;
    }   
    public void SetTip(int time)
    {
        m_curTime = time;
    }


    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    public override bool ComportRes(string abName, string artName)
    {
        throw new NotImplementedException();
    }

    public override bool IsWaitArt(int key = 0)
    {
        throw new NotImplementedException();
    }
}