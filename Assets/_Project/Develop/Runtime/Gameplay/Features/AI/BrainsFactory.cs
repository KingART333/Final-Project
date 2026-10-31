using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets._Project.Develop.Runtime.Utilities.Conditions.CompositeCondition;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly IInputService _inputService;
        private readonly EntitiesLifeContext _entitiesLifeContext;
        private const float EnergySavingThreshold = 0.4f;
        private const float MouseRotationSensitivity = 10f;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
            _timerServiceFactory = _container.Resolve<TimerServiceFactory>();
            _brainsContext = _container.Resolve<AIBrainsContext>();
            _inputService = _container.Resolve<IInputService>();
            _entitiesLifeContext = container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity, ITargetSelector targetSelector)
        {
            AiStateMachine combatState = CreateAutoAttackStateMachine(entity);

            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromMovementToCombatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value != null))
                .Add(new FuncCondition(() => _inputService.Direction == Vector3.zero));

            ICompositeCondition fromCombattoMovementStateCondition = new CompositeCondition(LogicOperations.Or)
                .Add(new FuncCondition(() => currentTarget.Value == null))
                .Add(new FuncCondition(() => _inputService.Direction != Vector3.zero));

            AiStateMachine behaviour = new AiStateMachine();

            behaviour.AddState(movementState);
            behaviour.AddState(combatState);

            behaviour.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            behaviour.AddTransition(combatState, movementState, fromCombattoMovementStateCondition);

            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);

            AiStateMachine rootStateMachine = new AiStateMachine();
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;

        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AiStateMachine stateMachine = CreateRandomMovementStatMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateTeleporterRandomBrain(Entity entity, float teleportInterval)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            EmptyState waitState = new EmptyState();
            RandomTeleportState teleportState = new RandomTeleportState(entity);

            TimerService waitTimer = _timerServiceFactory.Create(teleportInterval);
            disposables.Add(waitTimer);
            disposables.Add(waitState.Entered.Subscribe(waitTimer.Restart));

            ICondition fromWaitToTeleportCondition = new FuncCondition(() => waitTimer.IsOver);
            ICondition fromTeleportToWaitCondition = new FuncCondition(() => true);

            AiStateMachine stateMachine = new AiStateMachine(disposables);

            stateMachine.AddState(waitState);
            stateMachine.AddState(teleportState);

            stateMachine.AddTransition(waitState, teleportState, fromWaitToTeleportCondition);
            stateMachine.AddTransition(teleportState, waitState, fromTeleportToWaitCondition);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateTeleporterSmartBrain(Entity entity, float teleportInterval)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            EmptyState waitState = new EmptyState();
            TeleportToTargetState teleportState = new TeleportToTargetState(entity);

            TimerService waitTimer = _timerServiceFactory.Create(teleportInterval);
            disposables.Add(waitTimer);
            disposables.Add(waitState.Entered.Subscribe(waitTimer.Restart));

            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;
            ReactiveVariable<float> currentEnergy = entity.CurrentEnergy;
            ReactiveVariable<float> maxEnergy = entity.MaxEnergy;

            ICompositeCondition fromWaitToTeleportCondition = new CompositeCondition()
                .Add(new FuncCondition(() => waitTimer.IsOver))
                .Add(new FuncCondition(() => currentTarget.Value != null))
                .Add(new FuncCondition(() => currentEnergy.Value >= maxEnergy.Value * EnergySavingThreshold));

            ICondition fromTeleportToWaitCondition = new FuncCondition(() => true);

            AiStateMachine behaviour = new AiStateMachine();

            behaviour.AddState(waitState);
            behaviour.AddState(teleportState);

            behaviour.AddTransition(waitState, teleportState, fromWaitToTeleportCondition);
            behaviour.AddTransition(teleportState, waitState, fromTeleportToWaitCondition);

            FindTargetState findTargetState = new FindTargetState(new LowestHealthTargetSelector(entity), _entitiesLifeContext, entity);
            AIParallelState parallelState = new AIParallelState(findTargetState, behaviour);

            AiStateMachine rootStateMachine = new AiStateMachine(disposables);
            rootStateMachine.AddState(parallelState);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateMainHeroManualBrain(Entity entity)
        {
            AiStateMachine combatState = CreateManualAttackStateMachine(entity);

            PlayerInputMovementState movementState = new PlayerInputMovementState(entity, _inputService);

            ICondition fromMovementToCombatStateCondition = new FuncCondition(() => _inputService.Direction == Vector3.zero);
            ICondition fromCombatToMovementStateCondition = new FuncCondition(() => _inputService.Direction != Vector3.zero);

            AiStateMachine rootStateMachine = new AiStateMachine();

            rootStateMachine.AddState(movementState);
            rootStateMachine.AddState(combatState);

            rootStateMachine.AddTransition(movementState, combatState, fromMovementToCombatStateCondition);
            rootStateMachine.AddTransition(combatState, movementState, fromCombatToMovementStateCondition);

            StateMachineBrain brain = new StateMachineBrain(rootStateMachine);
            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AiStateMachine CreateManualAttackStateMachine(Entity entity)
        {
            MouseRotationState mouseRotationState = new MouseRotationState(entity, _inputService, MouseRotationSensitivity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICompositeCondition fromRotationToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() => _inputService.IsAttackButtonPressed));

            ICondition fromAttackToRotationCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AiStateMachine stateMachine = new AiStateMachine();

            stateMachine.AddState(mouseRotationState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(mouseRotationState, attackTriggerState, fromRotationToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, mouseRotationState, fromAttackToRotationCondition);

            return stateMachine;
        }

        private AiStateMachine CreateRandomMovementStatMachine(Entity entity)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            RandomMovementState randomMovementState = new RandomMovementState(entity, 0.5f);

            EmptyState emptyState = new EmptyState();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new FuncCondition(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new FuncCondition(() => idleTimer.IsOver);

            AiStateMachine stateMachine = new AiStateMachine(disposables);

            stateMachine.AddState(randomMovementState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            stateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return stateMachine;
        }

        private AiStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));
                    return angleToTarget < 1f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICondition fromAttackToRotateStateCondition = new FuncCondition(() => inAttackProcess.Value == false);

            AiStateMachine stateMachine = new AiStateMachine();

            stateMachine.AddState(rotateToTargetState);
            stateMachine.AddState(attackTriggerState);

            stateMachine.AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition);
            stateMachine.AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);

            return stateMachine;

        }
    }
}
