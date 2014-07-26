using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct SGameEventType
{
	static SGameEventType()
	{
		Add(0,"invalid");
	}
	
	public static SGameEventType invalid
	{
		get { return new SGameEventType(0); }
	}
	
	private static Dictionary<int,string> mStates = new Dictionary<int, string>();
	
	public static void Add(int pIndex, string pName)
	{
		mStates.Add(pIndex, pName);
	}
	
	private string mName;
	private int mIndex;
	
	public SGameEventType(string pName)
	{
		mName = "invalid";
		mIndex = 0;
		foreach(KeyValuePair<int, string>kv in mStates)
		{
			if(kv.Value == pName)
			{
				mName = pName;
				mIndex = kv.Key;
				return;
			}
		}
		
		
	}
	
	public SGameEventType(int pIndex)
	{
		mName = "invalid";
		mIndex = 0;
		
		if(!mStates.ContainsKey(pIndex))
		{
			pIndex = 0;
		}
		
		mIndex = pIndex;
		mName = mStates[mIndex];
		
	}
	
	
	public override bool Equals (object obj)
	{
		if(obj is SGameEventType)
			return Equals((SGameEventType)obj);
		return false;
	}
	
	public bool Equals(SGameEventType pState)
	{
		return pState.mIndex == this.mIndex;
	}
	
	public static bool operator !=(SGameEventType a, SGameEventType b)
	{
		return a.mIndex != b.mIndex;
	}
	
	public static bool operator ==(SGameEventType a, SGameEventType b)
	{
		return a.mIndex == b.mIndex;
	}
	public override int GetHashCode ()
	{
		return mIndex;
	}
	
	public static implicit operator int(SGameEventType a)
	{
	    return a.mIndex;
	}
	
	public static implicit operator string(SGameEventType a)
	{
	    return a.mName;
	}
	
	public static implicit operator SGameEventType(int a)
	{
	    return new SGameEventType(a);
	}
	
	public static implicit operator SGameEventType(string a)
	{
	    return new SGameEventType(a);
	}
	public override string ToString ()
	{
		return string.Format ("[{0},{1}]",mIndex,mName);
	}
}


