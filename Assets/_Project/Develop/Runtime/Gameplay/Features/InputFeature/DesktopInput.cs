using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class DesktopInput : IInputService
    {
        private const string HorizontalAxisName = "Horizontal";
        private const string VerticalAxisName = "Vertical";
        private const string MouseXAxisName = "Mouse X";

        private const int AttackMouseButton = 0;

        public bool IsEnabled { get; set; } = true;

        public Vector3 Direction
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                return new Vector3(Input.GetAxisRaw(HorizontalAxisName), 0, Input.GetAxisRaw(VerticalAxisName));
            }
        }

        public float MouseRotationDelta
        {
            get
            {
                if (IsEnabled == false)
                    return 0f;

                return Input.GetAxis(MouseXAxisName);
            }
        }

        public bool IsAttackButtonPressed
        {
            get
            {
                if (IsEnabled == false)
                    return false;

                return Input.GetMouseButtonDown(AttackMouseButton);
            }
        }
    }
}