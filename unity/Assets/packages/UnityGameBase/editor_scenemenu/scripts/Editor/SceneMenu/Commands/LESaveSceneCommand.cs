using System;
using UnityEngine;
using UnityEditor;

public class LESaveSceneCommand : LESceneMenuCommand
{
	public LESaveSceneCommand ()
	{
		mName = "Save Scene";
		mModifiers = UnityEngine.EventModifiers.Alt;
		mKeyCode = UnityEngine.KeyCode.S;
	}
	public override void Execute ()
	{
		EditorApplication.SaveScene();
	}
}

