using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace UEDS
{
	/// <summary>
	/// Editor Update class has a queue of functions and actions. 
	/// You can register a function, which is called everytime the Unity Editor 
	/// runs its update method. 
	/// 
	/// If this function returns true, the OnDone Action will be triggered, 
	/// your function/action pair will be dequeued and the next queue entry will be run. 
	/// 
	/// This is useful if you want to simulate coroutines at editor time or if you want to run async code. 
	/// </summary>
	internal class EditorUpdate
	{
		class Entry
		{
			public Func<bool> mIsDone;
			public Action mOnDone;
			public Entry (Func<bool> pIsDone, Action pOnDone)
			{
				mIsDone = pIsDone;
				mOnDone = pOnDone;
			}
		}

		static Queue<Entry> mEntries = new Queue<Entry>();
		
		/// <summary>
		/// Calls the specified function pIsDone every update. Once the function returns true it will stop being called and pOnDone will be executed. 
		/// </summary>
		/// <param name='pIsDone'>
		/// update polling function
		/// </param>
		/// <param name='pOnDone'>
		/// Callback when polling returns true. 
		/// </param>
		public static void Run(Func<bool> pIsDone , Action pOnDone)
		{
			if(mEntries.Count == 0)
			{
				EditorApplication.update += OnEditorUpdate;
			}
			mEntries.Enqueue(new Entry(pIsDone, pOnDone));

		}

		static void OnEditorUpdate()
		{
			if(mEntries.Count == 0)
			{
				EditorApplication.update -= OnEditorUpdate;
			}
			else
			{
				var e = mEntries.Peek();
				if(e.mIsDone())
				{
					mEntries.Dequeue();
					e.mOnDone();
				}
			}
		}
	}
}

