﻿using Sero.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Doorman
{
    public abstract class SuccessView
    {
        public Dictionary<string, string> _links { get; set; }
        public Dictionary<string, object> _actions { get; set; }
        public object _embeded { get; set; }

        public SuccessView()
        {
            _links = new Dictionary<string, string>();
            _actions = new Dictionary<string, object>();
        }
    }
}