using UnityEditor;


namespace HW10
{
    [CustomEditor(typeof(Star)), CanEditMultipleObjects]
    public class StarEditor : Editor
    {
        private SerializedProperty _center;
        private SerializedProperty _points;
        private SerializedProperty _frequency;
        private void OnEnable()
        {
            _center = serializedObject.FindProperty("_center");
            _points = serializedObject.FindProperty("_points");
            _frequency = serializedObject.FindProperty("_frequency");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(_center);
            EditorGUILayout.PropertyField(_points);
            EditorGUILayout.IntSlider(_frequency, 1, 20);
            var totalPoints = _frequency.intValue * _points.arraySize;
            if (totalPoints < 3)
            {
                EditorGUILayout.HelpBox("At least three points are needed.",
                    MessageType.Warning);
            }
            else
            {
                EditorGUILayout.HelpBox(totalPoints + " points in total.",
                    MessageType.Info);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}