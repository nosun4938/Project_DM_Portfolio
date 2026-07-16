using UnityEngine;

public class ArtifactStateMachine
{
    private Artifact _artifact;
    private ArtifactStateBase _currentState;
    public ArtifactStateBase CurrentState { get { return _currentState; } }

    public ArtifactStateMachine(Artifact artifact)
    {
        _artifact = artifact;
    }

    public void ChangeState(ArtifactStateBase newState)
    {
        //Debug.Log($"ChangeState: {_currentState?.GetType().Name} => {newState?.GetType().Name}");
        if (_currentState == newState)
        {
            _currentState.ReEnter();
            return;
        }

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
