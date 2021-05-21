using UnityEditor;
using UnityEngine;
using static UnityEngine.Mathf;

[System.Serializable]
public struct VectorP
{
    public float angle; // in degrees, with +Y (North) being angle 0 and going clockwise
    public float magnitude; // distance, speed, whatever

    public VectorP(float angle, float magnitude)
    {
        this.angle = angle;
        this.magnitude = magnitude;
    }

    public VectorP(Vector2 vector)
    {
        // New struct replaces itself with the results of Vector2.ToVectorP()
        this = vector.ToVectorP(); 
    }
    public VectorP(Vector3 vector)
    {
        // New struct replaces itself with the results of Vector3.ToVectorP()
        this = vector.ToVectorP();
    }

    public Vector2 ToVector2()
    {
        // Converts angle and magnitude to x and y.
        return new Vector2(Sin(angle * Deg2Rad) * magnitude, Cos(angle * Deg2Rad) * magnitude);
    }
    public Vector3 ToVector3()
    {
        // Converts angle and magnitude to x and y. Leaves z as 0.
        return new Vector3(Sin(angle * Deg2Rad) * magnitude, Cos(angle * Deg2Rad) * magnitude, 0);
    }
}

// This class is required before we can declare extension methods ...for some reason
public static class VectorPExtensionMethods
{
    // adds ToVectorP() method to Vector2 class
    public static VectorP ToVectorP(this Vector2 vector)
    {
        // For angle, use Atan2, which is an alternate version of Atan that is better at dealing with a full 360.
        // Atan2 normally uses +X (East) for angle 0, and goes counter-clockwise. Swapping x and y makes angle 0 now +Y (North). Negating x makes it go clockwise.
        // Edit: NOT negating x makes it go clockwise. I don't understand... but it's working
        // For magnitude, we use the pythagorean theorem.
        return new VectorP(Atan2(vector.x, vector.y) * Rad2Deg, Sqrt((vector.x * vector.x) + (vector.y * vector.y)));
    }

    // adds ToVectorP() method to Vector3 class
    public static VectorP ToVectorP(this Vector3 vector)
    {
        // Same thing as above, just ignore z.
        return new VectorP(Atan2(vector.x, vector.y) * Rad2Deg, Sqrt((vector.x * vector.x) + (vector.y * vector.y)));
    }
}

// Everything below is just there to make it show up as a table in the Inspector, like Vector2 does.
// Rougly based on https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
// But mostly provided by MechWarrior99#6608 on the Unity Discord
// This still has some problems, need to try https://docs.unity3d.com/ScriptReference/EditorGUI.MultiFloatField.html
// "You will need to use EditorGUI.Begin/EndChangeCheck(), and set the serialized properties in the if for the endChangeCheck."
// "(There is also a scoped(?) ChangeCheck if you prefer that)"

[CustomPropertyDrawer(typeof(VectorP))]
public class PolarVectorDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property); // This makes it so that things like the blue lines for prefabs showup properly and it handles overrides.

        Rect valuesRect = EditorGUI.PrefixLabel(position, label); // Makes label without any field and moves the rect over to the end.
        valuesRect.width = 75;
        EditorGUIUtility.labelWidth = 10; // Unity has spacing between a label and a field, this sets what that space is. I just guessed on the size.
        EditorGUI.PropertyField(valuesRect, property.FindPropertyRelative("angle"), new GUIContent("A"));

        EditorGUIUtility.labelWidth = 12;
        // Move over to draw the next field.
        valuesRect.x += valuesRect.width + 3;

        EditorGUI.PropertyField(valuesRect, property.FindPropertyRelative("magnitude"), new GUIContent("M"));
        EditorGUIUtility.labelWidth = 0; // Setting to 0 resets it to the default value.

        EditorGUI.EndProperty();
    }
}