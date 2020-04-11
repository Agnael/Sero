using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Doorman
{
    public interface ICredentialPenaltyStore
    {
        /// <summary>
        ///     Devuelve una penalidad en caso de existir una cuyas fechas de inicio y fin contengan 
        ///     la "targetDate" pasada por parámetro.
        /// </summary>
        Task<CredentialPenalty> Get(string credentialId, DateTime targetDate);

        Task CreatePenalty(CredentialPenalty penalty);
    }
}
