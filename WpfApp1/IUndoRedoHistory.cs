namespace WpfApp1
{
    public interface IUndoRedoHistory<T, U>
    {
        bool CanRedo { get; }
        bool CanUndo { get; }
        bool InUndoRedo { get; }
        int RedoCount { get; }
        bool SupportRedo { get; set; }
        int UndoCount { get; }

        void BeginCompoundDo(int pointerIndex);
        void CheckPoint(IListTOfVMemento<T, U> m);
        void Clear();
        void DiscardTop();
        void EndCompoundDo();
        IListTOfVMemento<T, U> PeekRedo();
        IListTOfVMemento<T, U> PeekUndo();
        U Redo();
        U Undo();
    }
}