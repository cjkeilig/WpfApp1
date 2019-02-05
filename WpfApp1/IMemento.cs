using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    public interface IMementoWithRef<T>
    {
        IMemento<T> Restore(T target);
        T GetPointer();
    }

    public interface IMemento<T>
    {
        IMemento<T> Restore(T target);
        T GetPointer();
    }

    public  interface ITrack<T>
    {
        UndoRedoHistory<T> GetTracker();

    }

}
