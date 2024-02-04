# Command Pattern

You'll find all the scripts inside the Scripts folder.

This is a basic undo/redo system for navigation on a 2D isometric grid. It allows the player to undo or redo their actions without any restriction, but can easily be tweaked to impose limitations.

## Implementation

First of all, there's the ICommand interface, which is the base for the pattern.

~~~c#
public interface ICommand{
    void Execute();
    void Undo();
}
~~~

Then, we implement our own Walk Command, which will encapsulate all the necessary data. In this case, the Character that will move and the path it'll walk. We create a clone of the stack to make sure we'll always have a reference to the original path. 

~~~c#
public class WalkCommand : ICommand{
    private readonly Character _character;
    private readonly Stack<Vector2Int> _path;

    public WalkCommand(Stack<Vector2Int> path, Character character){
        _path = path.Clone();
        _character = character;
    }

    public void Execute(){
        Stack<Vector2Int> path = _path.Clone();
        path.Pop();
        _character.WalkPath(path);
    }

    public void Undo(){
        Stack<Vector2Int> reversePath = _path.Reverse();
        reversePath.Pop();
        _character.WalkPath(reversePath);
    }
}
~~~

Because we're focusing on the pattern itself, the actual implementation of the navigation algorithm is out of scope, but you can take a look at the Character, GridMovement and Pathfinder classes to see how it works (it's really just a basic A* algorithm).

Finally, in the GridMovement class, we create a list that will be used to save all the commands a player has executed. Notice that it's a List of ICommand and not WalkCommand, this way you can support undoing/redoing other types of commands. We'll also use an int to keep track of the index of the current command.

~~~c#
public class GridMovement : MonoBehaviour{
    // ...
    private readonly List<ICommand> _commands = new();
    private int _currentCommandIndex;
    // ...
}
~~~

Now we're able to provide the undo/redo behaviour simply by swithing the current command and calling Execute/Undo:

~~~c#
public class GridMovement : MonoBehaviour{
    //...
    public void RedoWalkCommand(){
        _currentCommandIndex++;
        _commands[_currentCommandIndex].Execute();
    }
    
    public void UndoWalkCommand(){
        _commands[_currentCommandIndex].Undo();
        _currentCommandIndex--;
    }
    //...
}
~~~

Remember that whenever we perform a new command, we also have to discard everything after the current command:

~~~c#
public class GridMovement : MonoBehaviour{
    // ... 
    private void AddWalkCommand(){
        if (_currentCommandIndex < _commands.Count - 1){ ClearRedoCommands(); }
        WalkCommand walkCommand = new(_pathfinder.Search(), _character);
        _commands.Add(walkCommand);
        // ...
    }
    
    private void ClearRedoCommands(){
        if (_currentCommandIndex < -1 || _currentCommandIndex > _commands.Count - 1){ return; }
    
        int removeCount = _commands.Count - _currentCommandIndex - 1;
        _commands.RemoveRange(_currentCommandIndex + 1, removeCount);
    }
    // ...
}
~~~

And that's it! Now you have and undo/redo system in your game! 😁
