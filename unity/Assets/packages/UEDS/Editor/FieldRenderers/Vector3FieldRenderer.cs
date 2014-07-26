using System;
using UnityEditor;
using UnityEngine;

public class Vector3FieldRenderer : FieldRendererBase
{
	public Vector3FieldRenderer () : base(typeof(UnityEngine.Vector3))
	{
	}


	#region implemented abstract members of FieldRendererBase
	public override void OnEditorGUI (UEDS.Setting pSetting)
	{
		pSetting.SetValue( 
			EditorGUILayout.Vector3Field(pSetting.DisplayName, (UnityEngine.Vector3)pSetting.GetValue()) );
	}
	#endregion
}

