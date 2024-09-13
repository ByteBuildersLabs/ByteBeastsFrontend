using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// CustomEditor attribute specifies that this class is a custom editor for the Waypoint class.
// It customizes how Waypoint behaves in the Unity Editor's Scene view.
[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    // Property that returns the inspected object (target) cast as a Waypoint.
    // This allows easy access to Waypoint-specific properties and methods.
    Waypoint WaypointTarget => target as Waypoint;

    // OnSceneGUI is called when the Scene view is rendered.
    // It is used to draw handles and interact with objects directly within the Scene view.
    private void OnSceneGUI()
    {
        // Set the color of the handles (e.g., points and lines) to red.
        Handles.color = Color.red;

        // Check if the Waypoint has points; if the array is null, exit the function.
        if (WaypointTarget.Points == null) return;

        // Iterate over each point in the Waypoint's points array.
        for (int i = 0; i < WaypointTarget.Points.Length; i++)
        {
            // BeginChangeCheck is used to monitor if the handle's position is modified by the user.
            EditorGUI.BeginChangeCheck();

            // Calculate the actual position of the current point by adding the waypoint's position to the point's relative position.
            Vector3 actualPoint = WaypointTarget.ActualPosition + WaypointTarget.Points[i];

            // Draw a draggable sphere handle (FreeMoveHandle) at the actual position of the point.
            // The parameters define the position, size of the handle, snapping, and the handle's visual representation (sphere).
            var fmh_21_68_638618263363875210 = Quaternion.identity; 
            Vector3 newPoint = Handles.FreeMoveHandle(actualPoint, 0.7f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

            // Create a GUIStyle for the text label that displays the point's index number.
            GUIStyle text = new GUIStyle();
            text.fontStyle = FontStyle.Bold; // Set the text style to bold.
            text.fontSize = 16; // Set the font size to 16.
            text.normal.textColor = Color.black; // Set the text color to black.

            // Define an offset to adjust the label's position relative to the point for better readability.
            Vector3 alignment = Vector3.down * 0.3f + Vector3.right * 0.3f;

            // Draw a label next to the current point, displaying its index (i + 1) in the Scene view.
            Handles.Label(WaypointTarget.ActualPosition + WaypointTarget.Points[i] + alignment, $"{i + 1}", text);

            // Check if any changes were made to the handle's position.
            if (EditorGUI.EndChangeCheck()) 
            {
                // Record the change for undo functionality in Unity; allows changes to be reverted if needed.
                Undo.RecordObject(target, "Free Move Handle");

                // Update the point's position in the Waypoint's points array, adjusting for the Waypoint's actual position.
                WaypointTarget.Points[i] = newPoint - WaypointTarget.ActualPosition;
            }
        }
    }
}
