using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public abstract class AbstractEvent : IEvent
    {
        private LogLevel _logLevel;
        protected Exception _exception;
        private bool _isPrepared;

        public string Level { get; private set; }
        public string Category { get; private set; }
        public string Type { get; private set; }
        public string Message { get; private set; }
        public DateTime DateTime { get; protected set; }
        public IEnumerable<ExceptionInfo> Exception { get; protected set; }
        public IEnumerable<string> Details { get; protected set; }

        public AbstractEvent(LogLevel level, string category, string message)
        {
            Initialize(level, category, message);
        }

        public AbstractEvent(LogLevel level, string category, string message, Exception ex)
        {
            Initialize(level, category, message);
            _exception = ex;
        }

        public AbstractEvent(LogLevel level, string category, Exception ex)
        {
            Initialize(level, category);
            _exception = ex;
        }

        private void Initialize(LogLevel logLevel, string category, string message = null)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");

            _logLevel = logLevel;
            Level = _logLevel.ToString();
            Category = category;
            Message = message;
            DateTime = DateTime.UtcNow;
        }

        public LogLevel GetLogLevel()
        {
            return _logLevel;
        }

        public virtual void Prepare()
        {
            if (_isPrepared)
                return;

            Type = TypeUtils.GetFriendlyName(this.GetType());

            if (_exception != null)
                this.Exception = ExceptionFormatter.Format(_exception);

            _isPrepared = true;
        }
    }

    public abstract class AbstractEvent<TState> : AbstractEvent
    {
        protected TState _state;

        public AbstractEvent(LogLevel level, string category, string message, TState state)
            : base(level, category, message)
        {
            if (state == null) throw new ArgumentNullException("state");
            _state = state;
        }

        public override void Prepare()
        {
            base.Prepare();

            // TODO: Podría estarse usando un custom formatter que rompe, debería hacerse un try catch que si rompe el custom formatter use el default, cosa de que el loggeo nunca se interrumpa y a lo sumo se vea un log medio feo.
            if (_state != null)
                this.Details = this.FormatState(_state);
        }

        protected abstract IEnumerable<string> FormatState(TState state);
    }
}
