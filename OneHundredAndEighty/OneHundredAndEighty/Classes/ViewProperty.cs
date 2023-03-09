using MyToolkit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Classes
{
    public class ViewProperty<T> : ObservableObject
    {
        public T _val;
        public T Val
        {
            get => _val;
            set => Set(ref _val, value);
        }
    }
}
