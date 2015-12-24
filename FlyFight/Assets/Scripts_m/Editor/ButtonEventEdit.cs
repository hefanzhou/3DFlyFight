using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ButtonEvent), true)]
public class ButtonEventEdit : Editor{

	public override void OnInspectorGUI()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(110f);
		base.OnInspectorGUI();
		DrawCommonProperties();
	}

	protected void DrawCommonProperties()
	{
		ButtonEvent be = target as ButtonEvent;

		//if (NGUIEditorTools.DrawHeader("Notific"))
		//{
		//	NGUIEditorTools.BeginContents();
		//	NGUIEditorTools.SetLabelWidth(110f);

			

		//	if (GUI.changed)
		//	{
				
		//	}
		//	NGUIEditorTools.EndContents();
		//}

		//NGUIEditorTools.SetLabelWidth(80f);
		//EventDelegateEditor.Field(be, be.onPointerEnter);
	}
}
