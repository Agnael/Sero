using Sero.Loxy.EfCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public static class FormattingUtils
    {
        public static IEnumerable<KeyValuePair<String, Object>> AsDictionary(object state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            return state as IEnumerable<KeyValuePair<String, Object>>;
        }

        public static bool IsNullKvp(KeyValuePair<String, Object> value)
        {
            return value.Equals(Constants.Null_Kvp);
        }

        public static IEnumerable<string> FormatCommandExecuting<TState>(TState state, Func<TState, Exception, string> formatter)
        {
            IEnumerable<string> details = new List<string>();
            var stateMap = AsDictionary(state);

            var templateKvp = stateMap.FirstOrDefault(x => x.Key == Constants.Key_MessageTemplate);
            string template = null;

            if (!IsNullKvp(templateKvp))
            {
                template = templateKvp.Value.ToString();
                var paramMap = stateMap.Where(x => x.Key != Constants.Key_MessageTemplate);

                foreach (var paramKvp in paramMap)
                {
                    if (paramKvp.Key == Constants.Key_CommandText)
                    {
                        string formattedCommand = paramKvp.Value.ToString();
                        formattedCommand = formattedCommand.UnescapeDoubleQuotes();

                        var newLineKvp = paramMap.FirstOrDefault(x => x.Key == Constants.Key_NewLine);

                        if (!IsNullKvp(newLineKvp))
                            formattedCommand = formattedCommand.Replace(newLineKvp.Value.ToString(), " ");

                        template = template.ReplaceParam(paramKvp.Key, formattedCommand);
                    }
                    else
                    {
                        template = template.ReplaceParam(paramKvp);
                    }
                }

                template = template.RemoveExtraWhitespaces();
                details = template.SplitByNewLine();
            }

            return details;
        }

        public static IEnumerable<string> FormatCommandExecuted<TState>(TState state, Func<TState, Exception, string> formatter)
        {
            IEnumerable<string> details = new List<string>();
            var stateMap = AsDictionary(state);

            var templateKvp = stateMap.FirstOrDefault(x => x.Key == Constants.Key_MessageTemplate);
            string template = null;

            if(!IsNullKvp(templateKvp))
            {
                template = templateKvp.Value.ToString();
                var paramMap = stateMap.Where(x => x.Key != Constants.Key_MessageTemplate);

                foreach(var paramKvp in paramMap)
                {
                    if(paramKvp.Key == Constants.Key_CommandText)
                    {
                        string formattedCommand = paramKvp.Value.ToString();
                        formattedCommand = formattedCommand.UnescapeDoubleQuotes();

                        var newLineKvp = paramMap.FirstOrDefault(x => x.Key == Constants.Key_NewLine);

                        if (!IsNullKvp(newLineKvp))
                            formattedCommand = formattedCommand.Replace(newLineKvp.Value.ToString(), " ");

                        template = template.ReplaceParam(paramKvp.Key, formattedCommand);
                    }
                    else
                    {
                        template = template.ReplaceParam(paramKvp);
                    }
                }

                template = template.RemoveExtraWhitespaces();
                details = template.SplitByNewLine();
            }

            return details;
        }
    }
}
