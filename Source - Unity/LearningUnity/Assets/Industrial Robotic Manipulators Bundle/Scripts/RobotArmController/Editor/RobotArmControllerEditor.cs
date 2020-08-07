// If you don't want an image at the top of the editor GUI in the inspector.
// Simply delete or comment out the line below.
#define SHOW_EDITOR_HEADER_IMAGE

using UnityEditor;

#if SHOW_EDITOR_HEADER_IMAGE
using UnityEngine;
#endif

namespace EpicCactus
{
    /// <summary>
    /// Custom editor for the RobotArmController.
    /// Used so we can use ranges defined in a RobotArmDescription
    /// within Sliders, rather than having to hard code limits.
    /// </summary>
    [CustomEditor(typeof(RobotArmController))]
    public class RobotArmControllerEditor : Editor
    {
        // The description component of the robot arm.
        // This contains all limits for slider properties
        private RobotArmDescription m_description;

        // All properties of the RobotArmController we wish to modify
        private SerializedProperty m_baseRotationProperty;
        private SerializedProperty m_armRotationProperty;
        private SerializedProperty m_upperArmRotationProperty;
        private SerializedProperty m_armEndRotationProperty;
        private SerializedProperty m_attachmentPointRotationProperty;

#if SHOW_EDITOR_HEADER_IMAGE
        // If enabled the image a style to use for
        // showing the header image in the editor.
        private Texture2D m_headerImage;
        private GUIStyle m_headerStyle;
        const string HeaderImageLocation = @"Assets\Scripts\RobotArmController\Editor\RobotArmControllerEditor.png";
#endif

        /// <summary>
        /// Unity Method.
        /// Called when the editor is enabled.
        /// </summary>
        public void OnEnable()
        {
#if SHOW_EDITOR_HEADER_IMAGE
            // Load the header image
            m_headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(HeaderImageLocation);
          
#endif

            // Grab the description from the RobotArmController we represent
            var owningController = serializedObject.targetObject as RobotArmController;
            if (owningController != null)
            {
                m_description = owningController.Description;
            }

            // Setup all properties that we are going to modify
            m_baseRotationProperty = serializedObject.FindProperty("BaseRotation");
            m_armRotationProperty = serializedObject.FindProperty("ArmRotation");
            m_upperArmRotationProperty = serializedObject.FindProperty("UpperArmRotation");
            m_armEndRotationProperty = serializedObject.FindProperty("ArmEndRotation");
            m_attachmentPointRotationProperty = serializedObject.FindProperty("AttachmentPointRotation");
        }

        /// <summary>
        /// Unity Method.
        /// Called when we should redraw/update the editor GUI
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawCustomUi();
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw our custom UI for the RobotArmController
        /// </summary>
        private void DrawCustomUi()
        {
#if SHOW_EDITOR_HEADER_IMAGE
            // If enabled and we have a header image, draw the image first.
            if (m_headerImage != null)
            {
                if (m_headerStyle == null)
                {
                    // Setup a style so we can center align the image.
                    m_headerStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                }

                // Draw the header image.
                GUILayout.Box(m_headerImage, m_headerStyle);
            }
#endif

            // If we don't have a valid description, then this robot arm
            // hasn't been setup correctly. Show an error message to the user.
            if (m_description == null)
            {
                EditorGUILayout.HelpBox(
                    "Failed to find a 'RobotArmDescription' on any children of the RobotArnController '" + name + "'.",
                    MessageType.Error);
                return;
            }

            // Draw all our properties, respecting limits where required.

            m_baseRotationProperty.floatValue = EditorGUILayout.FloatField(
                "Base Rotation",
                m_baseRotationProperty.floatValue);

            EditorGUILayout.Slider(m_armRotationProperty,
                m_description.ArmLimits.x,
                m_description.ArmLimits.y,
                "Arm Rotation");

            EditorGUILayout.Slider(m_upperArmRotationProperty,
                -m_description.UpperArmAnimationSamplePoints,
                m_description.UpperArmAnimationSamplePoints,
                "Upper Arm Rotation");

            m_armEndRotationProperty.floatValue = EditorGUILayout.FloatField(
                "Arm End Rotation",
                m_armEndRotationProperty.floatValue);

            EditorGUILayout.Slider(m_attachmentPointRotationProperty,
                m_description.AttachmentPointLimits.x,
                m_description.AttachmentPointLimits.y,
                "Wrist Rotation");
        }
    }
}
