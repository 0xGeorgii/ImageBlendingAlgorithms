using IBALib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IBALib.Types
{
    public class ImageWrapper<T> : IMatrix<T>
    {
        private Func<int> _getWidth;
        private Func<int> _getHeight;
        private Func<int, int, T> _getT;
        private Action<int, int, T> _setT;

        private object _source;
        private PropertyInfo _widthPF;
        private PropertyInfo _heigthPF;
        private PropertyInfo _indexedPF;

        public ImageWrapper(object source)
        {
            _source = source;
            _widthPF = _source
                .GetType()
                .GetProperty("Width", BindingFlags.Public | BindingFlags.Instance);
            _getWidth = () => (int)_widthPF.GetValue(_source);
            _heigthPF = _source
                .GetType()
                .GetProperty("Height", BindingFlags.Public | BindingFlags.Instance);
            _getHeight = () => (int)_heigthPF.GetValue(_source);
            _indexedPF = _source.GetType().GetProperty("Item");
            _getT = (x, y) => (T)_indexedPF.GetValue(_source, new object[] { x, y });
            _setT = (x, y, val) => _indexedPF.SetValue(source, new object[] { x, y, val });
        }

        public T this[int x, int y]
        {
            get => _getT(x, y);
            set => _setT(x, y, value);
        }

        public int Width => _getWidth();

        public int Height => _getHeight();

        public object GetSource => _source;
    }
}
