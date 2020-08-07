using UnityEngine;

namespace EpicCactus
{
    /// <summary>
    /// A description of the Robot Arm, used by other scripts.
    /// This allows the 'bones' and 'limits' to be defined in
    /// the editor, rather than being hard coded.
    /// </summary>
    public class RobotArmDescription : MonoBehaviour
    {
        #region Components

        public Transform BaseTransform;
        public Transform ArmTransform;
        public Transform ArmEndTransform;
        public Transform AttachmentPointTransform;
        public Animator RobotArmAnimator;

        #endregion

        #region Limits 

        public Vector2 ArmLimits = new Vector2(-30.0f, 30.0f);
        public Vector2 AttachmentPointLimits = new Vector2(-90.0f, 90.0f);
        public int UpperArmAnimationSamplePoints = 100;

        #endregion
    }
}
