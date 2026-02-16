using System.Collections.Generic;

namespace ChessLogic.Commands
{
    public class MoveManager
    {
        private readonly Stack<MoveCommand> undoStack = new Stack<MoveCommand>();
        private readonly Stack<MoveCommand> redoStack = new Stack<MoveCommand>();

        public void ExecuteMove(MoveCommand command)
        {
            command.Execute();
            undoStack.Push(command);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }
    }
}

