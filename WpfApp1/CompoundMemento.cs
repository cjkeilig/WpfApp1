#region license

/*
Copyright (c) 2007 Lu Yixiang

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
    /// <summary>
    /// A class used to group multiple mementos together, which can be pushed on to the undo stack as a single memento. 
    /// With this class, multiple consecutive actions can be recognized as a single action, which are undo as an entity. 
    /// It also implements the <see cref="IMemento<typeparamref name="T"/>"/> interface, which means one <see cref="CompoundMemento&lt;T&gt;"/> can be a 
    /// member of another <see cref="CompoundMemento<typeparamref name="T"/>"/>. Therefore it is possible to create hierachical mementos. 
    /// </summary>
    /// <seealso cref="IMemento<typeparam name="T"></typeparam>/>
    [Serializable]
    public class CompoundMemento<T> : IMemento<T>
    {
        private List<IMemento<T>> mementos = new List<IMemento<T>>();
        private int pointerIndex;


        public CompoundMemento(int pointerIndex) 
        {

            this.pointerIndex = pointerIndex;
        }



    /// <summary>
    /// Adds memento to this complex memento. Note that the order of adding mementos is critical.
    /// </summary>
    /// <param name="m"></param>
    public void Add(IMemento<T> m)
        {
            mementos.Add(m);
        }

        /// <summary>
        /// Gets number of sub-memento contained in this complex memento.
        /// </summary>
        public int Size
        {
            get { return mementos.Count; }
        }

        #region IMemento Members

        /// <summary>
        /// Implicity implememntation of <see cref="IMemento&lt;T&gt;.Restore(T)"/>, which returns <see cref="CompoundMemento<typeparamref name="T"/>"/>
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public CompoundMemento<T> Restore(T target)
        {
            CompoundMemento<T> inverse = new CompoundMemento<T>(pointerIndex);
            //starts from the last action
            for (int i = mementos.Count - 1; i >= 0; i--)
                inverse.Add(mementos[i].Restore(target));
            return inverse;
        }

        /// <summary>
        /// Explicity implememntation of <see cref="IMemento<typeparamref name="T"/>.Restore(T)"/>
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        IMemento<T> IMemento<T>.Restore(T target)
        {
            return Restore(target);
        }

        public T GetPointer()
        {
            // Return the last Memento
            return mementos[pointerIndex].GetPointer();
        }

        #endregion
    }
}
