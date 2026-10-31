using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class MouseRotationState : State, IUpdatableState
    {
        private IInputService _inputService;
        private ReactiveVariable<Vector3> _rotationDirection;
        private Rigidbody _rigidbody;

        private float _sensitivity;
        private float _yaw;

        public MouseRotationState(Entity entity, IInputService inputService, float sensitivity)
        {
            _inputService = inputService;
            _rotationDirection = entity.RotationDirection;
            _rigidbody = entity.Rigidbody;

            _sensitivity = sensitivity;
        }

        public override void Enter()
        {
            base.Enter();

            _yaw = _rigidbody.rotation.eulerAngles.y;
            _rotationDirection.Value = Quaternion.Euler(0f, _yaw, 0f) * Vector3.forward;
        }

        public void Update(float deltaTime)
        {
            _yaw += _inputService.MouseRotationDelta * _sensitivity;

            _rotationDirection.Value = Quaternion.Euler(0f, _yaw, 0f) * Vector3.forward;
        }
    }
}