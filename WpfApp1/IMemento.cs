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
}
