using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sero.Core
{
    public class UrlBuilder<TCollectionFilter>
    {
        private List<KeyValuePair<string, string>> _params;
        private string _urlBase;

        public UrlBuilder(string urlBase)
        {
            _urlBase = urlBase;
            _params = new List<KeyValuePair<string, string>>();
        }

        // Sería más performante usar "nameof(obj.property)" porque se resuelve en compile time pero
        // prefiero segmentar el codigo por ahora porque es un asco el choclazo que se arma
        public void AddParam<U>(Expression<Func<TCollectionFilter, U>> keyNameSelector, object value)
        {
            string key = ReflectionUtils.GetPropertyName(keyNameSelector);
            _params.Add(new KeyValuePair<string, string>(key.ToLower(), value.ToString()));
        }

        // Sería más performante usar "nameof(obj.property)" porque se resuelve en compile time pero
        // prefiero segmentar el codigo por ahora porque es un asco el choclazo que se arma
        public void AddParam(string name, string value)
        {
            _params.Add(new KeyValuePair<string, string>(name.ToLower(), value));
        }

        public string Build()
        {
            var qsParts = new List<string>();

            foreach (var param in _params)
                qsParts.Add(string.Format("{0}={1}", param.Key.ToLower(), param.Value));

            string url = _urlBase;

            if (qsParts.Count > 0)
                url += "?" + string.Join("&", qsParts);

            return url;
        }
    }
}
