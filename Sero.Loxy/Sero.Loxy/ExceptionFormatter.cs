using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public static class ExceptionFormatter
    {
        public static IEnumerable<ExceptionInfo> Format(Exception ex)
        {
            var infoList = new List<ExceptionInfo>();

            if (ex is AggregateException)
            {
                var innerExceptions = (ex as AggregateException).Flatten().InnerExceptions;
                foreach (var innerException in innerExceptions)
                {
                    ExceptionInfo info = FormatExceptionSingle(innerException);
                    infoList.Add(info);
                }
            }
            else // Default handling
            {
                FillExceptionInfoListRecursive(infoList, ex);
            }

            return infoList;
        }

        private static ExceptionInfo FormatExceptionSingle(Exception ex)
        {
            ExceptionInfo info = new ExceptionInfo();
            info.ExceptionClass = ex.GetType().ToString();
            info.Message = ex.Message;

            var stackStepList = ex.StackTrace.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> stackStepListFormatted = new List<string>();

            foreach (string stackStep in stackStepList)
            {
                string stepFormatted = stackStep.Trim();

                stackStepListFormatted.Add(stepFormatted);
            }

            info.StackTrace = stackStepListFormatted.ToArray();

            return info;
        }

        private static void FillExceptionInfoListRecursive(ICollection<ExceptionInfo> destinationCollection, Exception ex)
        {
            if (destinationCollection == null) throw new ArgumentNullException(nameof(destinationCollection));
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            ExceptionInfo newInfo = FormatExceptionSingle(ex);
            destinationCollection.Add(newInfo);

            if (ex.InnerException != null)
                FillExceptionInfoListRecursive(destinationCollection, ex.InnerException);
        }
    }
}
