using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public class Page<T>
    {
        /// <summary>
        ///     Total de registros existentes, independientemente de la cantidad devuelta en la pagina actual.
        /// </summary>
        public readonly int Total;
        public readonly IEnumerable<T> Items;

        public bool IsEmpty => Total == 0;

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
