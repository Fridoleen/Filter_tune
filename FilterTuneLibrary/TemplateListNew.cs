using FilterTuneWPF_dll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FilterTuneLibrary
{
    class TemplateListNew : IList<FilterTemplate>
    {
        private List<FilterTemplate> Templates;
        public FilterTemplate this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(FilterTemplate item)
        {
            Templates.Add(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(FilterTemplate item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(FilterTemplate[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<FilterTemplate> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(FilterTemplate item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, FilterTemplate item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(FilterTemplate item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
