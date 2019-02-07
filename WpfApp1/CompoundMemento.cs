using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
    [Serializable]
    public class CompoundListMemento<T,U> : IListTOfVMemento<T, U>
    {
        private List<IListTOfVMemento<T,U>> mementos = new List<IListTOfVMemento<T,U>>();
        private int pointerIndex;


        public CompoundListMemento(int pointerIndex) 
        {

            this.pointerIndex = pointerIndex;
        }

        public void Add(IListTOfVMemento<T, U> m)
        {
            mementos.Add(m);
        }

        public int Size
        {
            get { return mementos.Count; }
        }

        public CompoundListMemento<T, U> Restore(T target)
        {
            CompoundListMemento<T, U> inverse = new CompoundListMemento<T, U>(pointerIndex);
            //starts from the last action
            for (int i = mementos.Count - 1; i >= 0; i--)
                inverse.Add(mementos[i].Restore(target));
            return inverse;
        }

        IListTOfVMemento<T, U> IListTOfVMemento<T, U>.Restore(T target)
        {
            return Restore(target);
        }

        public U GetChild()
        {
            // Return the last Memento
            return mementos[pointerIndex].GetChild();
        }

    }
}
