using System;
using UnityEditor;
using UnityEngine;

public class LEDeleteSelectedCommand : LESceneMenuCommand
{
	public LEDeleteSelectedCommand ()
	{
		mName = "Delete Selected";
		mKeyCode = KeyCode.Delete;
		mModifiers = EventModifiers.FunctionKey;
	}
	public override void Execute ()
	{

		GameObject go = Selection.activeGameObject;
		Undo.DestroyObjectImmediate(go);

	}
}

