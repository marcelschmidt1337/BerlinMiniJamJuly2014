using System;
using UnityEngine;

public class GameEvent
{
	public object data;
	
	public SGameEventType eventType
	{
		get;
		protected set;
	}
	public MonoBehaviour dispatcher
	{
		get;
		protected set;
	}
	
	public GameEvent ( SGameEventType pEventType, MonoBehaviour pDispatcher )
	{
		eventType = pEventType;
		dispatcher = pDispatcher;
	}
}

