using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Core
{
    public class Page<T> : IPage<T>
    {
        /// <summary>
        ///     Total de registros existentes, independientemente de la cantidad devuelta en la pagina actual.
        /// </summary>
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }

        public bool IsEmpty => Items.Count() == 0;

        public Page()
        {
            this.Total = 0;
            this.Items = new List<T>();
        }

        public Page(int total, IEnumerable<T> items)
        {
            this.Total = total;
            this.Items = items ?? new List<T>();
        }
    }
}
