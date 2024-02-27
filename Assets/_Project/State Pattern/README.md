# State Pattern

This is a demonstration of how to use the State Pattern to create a player controller with many state. This pattern can
also be used to create AI and other stuff, but I feel like this is one of the best examples. If you have already
programmed a player controller where the player character had many possible states (like Metroidvania games) you
probably know what I'm talking about.

## Implementation

We start by creating the base IState interface.

~~~c#
public interface IState{
    IEnumerator Enter(Player player);
    void Update(Player player);
    void FixedUpdate(Player player);
    IEnumerator Exit(Player player);
}
~~~

This interface will be used to create all the possible player states. You'll notice I made it so that the Enter and Exit
methods should return an IEnumerator. This allows us to use them as coroutines and implement logic while transitioning
states like waiting for an animation to finish or anything. You can check the MorphBallState class to see how it works.
Also, the Player is passed as a parameter so that we can access character-specific data like max speed, health, or
whatever parameters your character has.

To create a new State, all we need to do is inherit from IState:

~~~c#
public class NewState : IState{
    public IEnumerator Enter(Player player){
        yield break;
    }
    
    public void Update(Player player){ }
    
    public void FixedUpdate(Player player){ }
    
    public IEnumerator Exit(Player player){
        yield break;
    }
}
~~~

**Note**: Because Enter and Exit return IEnumerator, we're forced to use _yield break_. If this bothers you, just create
a base State class and provide a default implementation.

Managing states is pretty straightforward. We instantiate an object of each state and add them to a Dictionary where
they key is an enum value with the name of the state:

~~~c#
public class Player : MonoBehaviour{
    [SerializeField] private PlayerState _currentState; // Serialized only to show the current state in the inspector tab
    private readonly Dictionary<PlayerState, IState> _states = new();
    
    private void Awake(){
        _states.Add(PlayerState.Idle, new IdleState());
        _states.Add(PlayerState.Running, new RunningState());
        _states.Add(PlayerState.Jumping, new JumpingState());
        // ... (Creating all states)   
        ChangeState(PlayerState.Idle); // Starts in Idle state     
    }
    
    public enum PlayerState{
        Idle,
        Running,
        Jumping,
        // ... (Rest of the states)
    }
}
~~~

To change between states, all we have to do is call the Exit() on the previous stage, wait for it to finish (so we need
to do this inside a Coroutine), change states and call Enter() on the new state.

~~~c#
public void ChangeState(PlayerState state){
    if (_changeStateCoroutine != null){ return; }
    _changeStateCoroutine = StartCoroutine(ChangeStateCoroutine(state));
}

private IEnumerator ChangeStateCoroutine(PlayerState state){
    yield return _states[_currentState]?.Exit(this);
    LastState = _currentState; // used to keep track of the last state, useful in specific cases
    _currentState = state;
    yield return _states[_currentState].Enter(this);
    _changeStateCoroutine = null;
}
~~~

To call the Update and FixedUpdate method on the current stage, we just delegate the execution based on
the right object inside the dictionary:

~~~c#
private void Update(){
    if (_changeStateCoroutine == null){ _states[_currentState].Update(this); }
}

private void FixedUpdate(){
    if (_changeStateCoroutine == null){ _states[_currentState].FixedUpdate(this); }
}
~~~

That's basically it. The skeleton for the State system is now ready. We can keep all the logic for a state inside that
specific script and change states simply by calling player.ChangeState(PlayerState.DesiredState). An example of the
running state:

~~~c#
public class RunningState : IState{

    public void Update(Player player){
        // ...
        
        float horizontal = Input.GetAxisRaw("Horizontal"); // Getting input
        if (Mathf.Abs(horizontal) > 0){ // If there's any, we move the character
            Vector2 velocity = player.Rigidbody.velocity;
            velocity.x = horizontal * player.Speed;
            player.Rigidbody.velocity = velocity;
            player.transform.localScale = new Vector3(horizontal, 1, 1);
        }
        else{ player.ChangeState(PlayerState.Idle); } // Otherwise, change to Idle state
    }
    
}
~~~

This makes our code more organized and easier to maintain. You might have noticed that we didn't change the velocity to
zero when changing to the Idle state. This is happening inside the Enter method of the Idle state, so that we guarantee
that everytime the player enters this state, the character velocity will be zero, and we also make the Idle state
responsible for setting its own velocity.

~~~c#
public class IdleState : IState{

    public IEnumerator Enter(Player player){
        player.Rigidbody.velocity = Vector2.zero;
    }
    
}
~~~

This is just a basic example. Feel free to look up the states inside the Scripts/States folder to understand how
each of them works. 😁

