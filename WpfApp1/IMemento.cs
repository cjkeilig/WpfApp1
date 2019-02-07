using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    public interface IMemento<T>
    {
        IMemento<T> Restore(T target);
    }

    public interface IListTOfVMemento<T,U>
    {
        IListTOfVMemento<T, U> Restore(T target);
        U GetChild();
    }

    public interface ITrack<T, U>
    {
        IUndoRedoHistory<T, U> GetTracker();
    }

}
