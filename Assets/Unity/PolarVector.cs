using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct PolarVector
{
    public float Angle; // in degrees
    public float Magnitude; // range 0-1
}


// Everything below is just there to make it show up as a table in the Inspector, like Vector2 does.
// Rougly based on https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
// But mostly provided by MechWarrior99#6608 on the Unity Discord
// This still has some problems, need to try https://docs.unity3d.com/ScriptReference/EditorGUI.MultiFloatField.html
// "You will need to use EditorGUI.Begin/EndChangeCheck(), and set the serialized properties in the if for the endChangeCheck."
// "(There is also a scoped(?) ChangeCheck if you prefer that)"

[CustomPropertyDrawer(typeof(PolarVector))]
public class PolarVectorDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property); // This makes it so that things like the blue lines for prefabs showup properly and it handles overrides.

        Rect valuesRect = EditorGUI.PrefixLabel(position, label); // Makes label without any field and moves the rect over to the end.
        valuesRect.width = 75;
        EditorGUIUtility.labelWidth = 10; // Unity has spacing between a label and a field, this sets what that space is. I just guessed on the size.
        EditorGUI.PropertyField(valuesRect, property.FindPropertyRelative("Angle"), new GUIContent("A"));

        EditorGUIUtility.labelWidth = 12;
        // Move over to draw the next field.
        valuesRect.x += valuesRect.width + 3;

        EditorGUI.PropertyField(valuesRect, property.FindPropertyRelative("Magnitude"), new GUIContent("M"));
        EditorGUIUtility.labelWidth = 0; // Setting to 0 resets it to the default value.

        EditorGUI.EndProperty();
    }
}