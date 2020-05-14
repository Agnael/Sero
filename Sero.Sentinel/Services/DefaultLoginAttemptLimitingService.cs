using Sero.Sentinel.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sero.Sentinel
{
    public class DefaultLoginAttemptLimitingService : ILoginAttemptLimitingService
    {
        public async Task<LoginAttemptRateLimiterResult> Check(string fromIp, ILoginAttemptStore attemptStore)
        {
            LoginAttempt latestAttempt = await attemptStore.GetLatest(fromIp);

            if (latestAttempt == null)
                return new LoginAttemptRateLimiterResult { IsApproved = true };

            TimeSpan timeSinceLatestAttempt = DateTime.UtcNow - latestAttempt.AttemptDate;

            // Cinco minutos es el tiempo de castigo, asique si el ultimo intento fue hace más de 5 minutos no es necesario
            // verificar nada más, en ese caso sería válido el intento
            if (timeSinceLatestAttempt < TimeSpan.FromMinutes(5))
            {
                // Pasó menos tiempo que el de castigo asique es posible que EXISTA un castigo a aplicar actualmente.
                // La regla es "Si se hacen 5 o más intentos fallidos en 5minutos, entonces no se puede volver a intentar sino hasta dentro de otros 5mins".
                // Entonces: Toma el último intento hecho y le resta 5 minutos, y una vez definida esa ventana temporal, busca todos los intentos que se hayan
                // hecho en ese tiempo. Si la cuenta da 5 o más, significa que en esos 5 minutos se violó la regla, y ahora mismo el usuario está en penitencia
                // hasta que se cumplan 5 minutos después del último intento.
                DateTime fromDt = latestAttempt.AttemptDate.AddMinutes(-5);
                DateTime toDt = latestAttempt.AttemptDate;
                int countInLast5minuteWindow = await attemptStore.Count(fromIp, fromDt, toDt);

                if(countInLast5minuteWindow < 5)
                {
                    return new LoginAttemptRateLimiterResult { IsApproved = true };
                }
                else
                {
                    TimeSpan timeToWait = latestAttempt.AttemptDate.AddMinutes(5) - DateTime.UtcNow;

                    var result = new LoginAttemptRateLimiterResult
                    {
                        IsApproved = false,
                        TimeToWait = timeToWait,
                        ErrorMessage = string.Format("Too many failed login attempts. Please wait {0} seconds before trying again.", timeToWait.TotalSeconds)
                    };

                    return result;
                }
            }
            else
            {
                return new LoginAttemptRateLimiterResult { IsApproved = true };
            }
        }
    }
}
