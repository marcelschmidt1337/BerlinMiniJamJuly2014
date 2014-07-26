using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Game event manager. 
/// 
/// WARNING!
/// 
/// Do NOT use this class to ensure game functionality. Use it ONLY for fx and nice to have visual stuff. If you have more than 5 event types, use another approach!
/// Using this Manager will mangle up code readability. Understanding and debugging WILL become a nightmare and small dwarfs with an evil grin will come and haunt you at full moon nights. 
/// 
/// This class allows to dispatch and receive events throughout the game without knowing the dispatching instance or the receivers. 
/// All receivers are statically registered with AddEventListener and can be deregistered RemoveEventListener. 
/// 
/// </summary>
public class GameEventManager
{
	public delegate void GameEventCallBack(GameEvent pEvent);

	/// <summary>
	/// Adds an event listener for a given event type. WARNING: Please read the GameEventManager documentation!
	/// </summary>
	/// <param name="pEventType">P event type.</param>
	/// <param name="pCallBack">P call back.</param>
	public static void AddEventListener(SGameEventType pEventType, GameEventCallBack pCallBack )
	{
		mInstance.RegisterEventListener(pEventType, pCallBack);
	}
	
	public static void RemoveEventListener(SGameEventType pEventType, GameEventCallBack pCallBack)
	{
		mInstance.DeRegisterEventListener(pEventType, pCallBack);
	}

	/// <summary>
	/// Dispatch the specified Event. WARNING: Please read the GameEventManager documentation!
	/// </summary>
	/// <param name="pEvent">P event.</param>
	public static void Dispatch(GameEvent pEvent)
	{
		mInstance.DispatchEvent(pEvent);
	}
	
	
	static GameEventManager mSInstance;
	
	private static GameEventManager mInstance
	{
		get{
			if(mSInstance == null)
			{
				mSInstance = new GameEventManager();
			}
			return mSInstance;
		}
	}
	
	private GameEventManager()
	{
		
	}
	
	Dictionary<int,List<GameEventCallBack>> mListeners = new Dictionary<int, List<GameEventCallBack>>();
	
	void RegisterEventListener(SGameEventType pEventType, GameEventCallBack pCallBack )
	{
		int type = (int)pEventType;
		if(!mListeners.ContainsKey(type))
		{
			mListeners[type] = new List<GameEventCallBack>();
		}else if(Application.isEditor)
		{
			// check if listener is already registered
			if(mListeners[type].Contains(pCallBack))
			{
				Debug.LogError(System.String.Format( "{0}, {1}, Event Listener is already registered!", pCallBack, pEventType));
			}
		}
		
		mListeners[(int)pEventType].Add(pCallBack);
	}
	
	void DeRegisterEventListener(SGameEventType pEventType, GameEventCallBack pCallBack)
	{
		int type = (int)pEventType;
		if(mListeners.ContainsKey(type))
		{
			if(mListeners[type].Contains(pCallBack))
			{
				mListeners[type].Remove(pCallBack);
			}
		}
	}
	void DispatchEvent(GameEvent pEvent)
	{
		int type = (int)pEvent.eventType;
		if(mListeners.ContainsKey(type))
		{
			foreach(GameEventCallBack cb in mListeners[type])
			{
				cb(pEvent);
			}
		}
	}
}
