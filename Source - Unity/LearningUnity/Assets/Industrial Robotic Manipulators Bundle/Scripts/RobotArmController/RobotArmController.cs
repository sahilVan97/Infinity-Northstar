using UnityEngine;

namespace EpicCactus
{
    /// <summary>
    /// The controller for the robot arm.
    /// Handles the update logic for moving and transforming
    /// each part of the robot arm, with respect to the values
    /// set by the user.
    /// </summary>
    [ExecuteAlways]
    public class RobotArmController : MonoBehaviour
    {
        /// <summary>
        /// Public access to the description this robot arm controller
        /// is using. Cached on first access, so it is presumed the
        /// description will not change in the lifetime of this
        /// robot arm controller.
        /// </summary>
        private RobotArmDescription m_description;
        public RobotArmDescription Description
        {
            get
            {
                if (m_description == null)
                {
                    m_description = GetComponentInChildren<RobotArmDescription>();
                }
                return m_description;
            }
        }

        /// <summary>
        /// All rotation values for each part of the robot arm.
        /// </summary>
        public float BaseRotation;
        public float ArmRotation;
        public float UpperArmRotation;
        public float ArmEndRotation;
        public float AttachmentPointRotation;

        /// <summary>
        /// Unity method.
        /// Called when this script is first started.
        /// Used to validate we have a description and to perform
        /// an initial setup required.
        /// </summary>
        public void Start()
        {
            // Locate the robot arm animator.
            if (Description == null)
            {
				Debug.LogErrorFormat("[RobotArmController] Failed to find a robot arm description on any child of '{0}'. The Robot Arm Controller will be disabled.", name);
                enabled = false;
                return;
            }

            // Set the playback speed to zero, as the animation will be controlled by script.
            Description.RobotArmAnimator.speed = 0.0f;
        }

        /// <summary>
        /// Unity method.
        /// Called on every frame.
        /// Used to update the rotation of the robot arm with
        /// respect to the values the user has set in the inspector.
        /// </summary>
        public void Update()
        {
            // This should never happen as we disable this controller
            // if a description isn't found.
            if (Description == null)
            {
                Debug.LogErrorFormat("[RobotArmController] Failed to find a robot arm description on any child of '{0}'. The Robot Arm Controller will be disabled.", name);
                enabled = false;
                return;
            }

            // Call the update to the animation first.
            UpdateUpperArmRotation();

            // Call all updates for each part of the arm.
            UpdateBaseRotation();
            UpdateArmRotation();
            UpdateArmEndRotation();
            UpdateAttachmentPointRotation();
        }

        /// <summary>
        /// Updates the z rotation value of the base of the robot.
        /// </summary>
        private void UpdateBaseRotation()
        {
            Description.BaseTransform.localEulerAngles = new Vector3(0.0f, 0.0f, BaseRotation);
        }

        /// <summary>
        /// Updates the y rotation value of the main arm of the robot.
        /// </summary>
        private void UpdateArmRotation()
        {
            Description.ArmTransform.localEulerAngles = new Vector3(0.0f, ArmRotation, 0.0f);
        }

        /// <summary>
        /// Update the x rotation value of the wrist of the arm.
        /// </summary>
        private void UpdateArmEndRotation()
        {
            Description.ArmEndTransform.localEulerAngles = new Vector3(ArmEndRotation, 0.0f, 0.0f);
        }

        /// <summary>
        /// Update the y rotation value of the attachment point of the arm.
        /// </summary>
        private void UpdateAttachmentPointRotation()
        {
            Description.AttachmentPointTransform.localEulerAngles = new Vector3(0.0f, AttachmentPointRotation, 0.0f);
        }

        /// <summary>
        /// Samples from an animation to update the upper arms rotation.
        /// We use an animation so that multiple parts of the arm move in sequence.
        /// Simulating inverse kinematics.
        /// </summary>
        private void UpdateUpperArmRotation()
        {
            // Calculate the offset into the animation, based upon the request rotation value.
            var offset = UpperArmRotation < 0
                ? UpperArmRotation + Description.UpperArmAnimationSamplePoints
                : UpperArmRotation;

            // Normalize the offset.
            var samplePoint = offset / Description.UpperArmAnimationSamplePoints;

            // Set the correct animation and update the frame to the required value.
            var animState = UpperArmRotation >= 0 ? "Anim01" : "Anim02";
            Description.RobotArmAnimator.Play(animState, -1, samplePoint);

#if UNITY_EDITOR
            // Execute in edit mode, doesn't work for animations.
            // So if we are in the editor and the editor is playing.
            // Manually update the animation.
            if (!Application.isPlaying)
            {
                Description.RobotArmAnimator.Update(Time.deltaTime);
            }
#endif
        }
    }
}
