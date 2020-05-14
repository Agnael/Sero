using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Gatekeeper
{
    public class GtkOptions
    {
        public string MainSalt { get; set; }
        public JwtConfiguration JwtGeneration { get; set; }

        public GtkOptions()
        {

        }
    }
}
