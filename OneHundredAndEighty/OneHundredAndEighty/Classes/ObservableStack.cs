using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Classes
{
    public class ObservableStack<T> : Stack<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public void PushObservable(T item) 
        {
            base.Push(item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, 0));
        }

        public T PopObservable()
        {
            T item = base.Pop();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, 0));
            return item;
        }

        public new virtual void Clear()
        {
            base.Clear();
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
