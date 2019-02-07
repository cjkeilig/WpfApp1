using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WpfApp1
{
    /// <summary>
    /// This class represents an undo and redo history.
    /// </summary>
    [Serializable]
    public class ListUndoRedoHistory<T, U> : IUndoRedoHistory<T, U>
    {
        private const int DEFAULT_CAPACITY = 100;

        private bool supportRedo = true;
        private bool inUndoRedo = false;

        [NonSerialized]
        private CompoundListMemento<T, U> tempMemento = null;

        protected T subject;

        protected Stack<IListTOfVMemento<T,U>> undoStack = new Stack<IListTOfVMemento<T,U>>(DEFAULT_CAPACITY);

        protected Stack<IListTOfVMemento<T,U>> redoStack = new Stack<IListTOfVMemento<T,U>>(DEFAULT_CAPACITY);

        public ListUndoRedoHistory(T subject)
        {
            this.subject = subject;
        }

        public bool InUndoRedo
        {
            get { return inUndoRedo; }
        }

        public int UndoCount
        {
            get { return undoStack.Count; }
        }

        public int RedoCount
        {
            get { return redoStack.Count; }
        }

        public bool SupportRedo
        {
            get { return supportRedo; }
            set { supportRedo = value; }
        }

        public void BeginCompoundDo(int pointerIndex)
        {
            if (tempMemento != null)
                throw new InvalidOperationException("Previous complex memento wasn't commited.");

            tempMemento = new CompoundListMemento<T, U>(pointerIndex);
        }

        public void EndCompoundDo()
        {
            if (tempMemento == null)
                throw new InvalidOperationException("Ending a non-existing complex memento");

            _Do(tempMemento);
            tempMemento = null;
        }

        public void CheckPoint(IListTOfVMemento<T,U> m)
        {
            if (inUndoRedo)
                throw new InvalidOperationException("Involking do within an undo/redo action.");

            if (tempMemento == null)
            {
                _Do(m);
            }
            else
            {
                tempMemento.Add(m);
            }
        }


        private void _Do(IListTOfVMemento<T,U> m)
        {
            redoStack.Clear();
            undoStack.Push(m);
        }

        public U Undo()
        {
            if (tempMemento != null)
                throw new InvalidOperationException("The complex memento wasn't commited.");

            inUndoRedo = true;
            IListTOfVMemento<T,U> top = undoStack.Pop();
            redoStack.Push(top.Restore(subject));
            inUndoRedo = false;

            return top.GetChild();
        }

        public U Redo()
        {
            if (tempMemento != null)
                throw new InvalidOperationException("The complex memento wasn't commited.");

            inUndoRedo = true;
            IListTOfVMemento<T,U> top = redoStack.Pop();
            undoStack.Push(top.Restore(subject));
            inUndoRedo = false;

            return top.GetChild();
        }

        public bool CanUndo
        {
            get { return (undoStack.Count != 0); }
        }

        public bool CanRedo
        {
            get { return (redoStack.Count != 0); }
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        public IListTOfVMemento<T,U> PeekUndo()
        {
            if (undoStack.Count > 0)
                return undoStack.Peek();
            else
                return null;
        }

        public IListTOfVMemento<T,U> PeekRedo()
        {
            if (redoStack.Count > 0)
                return redoStack.Peek();
            else
                return null;
        }

        public void DiscardTop()
        {
            if (undoStack.Count > 0)
                undoStack.Pop();
        }

    }

}