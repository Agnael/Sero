using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Core
{
    /// <summary>
    ///     Extensions to simplify controller testing.
    /// </summary>
    public static class ActionResultExtensions
    {
        public static StatusCodeResult AsStatusCode(this IActionResult result)
        {
            if (!(result is StatusCodeResult))
                throw new InvalidCastException("This IActionResult is not an StatusCodeResult instance.");

            return result as StatusCodeResult;
        }

        public static AcceptedResult AsAccepted(this IActionResult result)
        {
            if (!(result is AcceptedResult))
                throw new InvalidCastException("This IActionResult is not an AcceptedResult instance.");

            return result as AcceptedResult;
        }

        public static TViewModel AsElementView<TApiElement, TViewModel>(this IActionResult result)
            where TApiElement : Element
            where TViewModel : class, new()
        {
            ObjectResult objResult = (ObjectResult)result;
            object value = objResult.Value;

            if(objResult is CreatedAtActionResult
                || objResult is AcceptedAtActionResult)
            {
                value = (value as ObjectResult).Value;
            }

            if (!(value is ElementView<TApiElement>))
                throw new InvalidCastException("This IActionResult is NOT an ElementView generic wrapper for the " + typeof(TApiElement).Name + " element type.");

            var view = value as ElementView<TApiElement>;
            var viewModel = view.ViewModel as TViewModel;
            return viewModel;
        }

        public static IEnumerable<TViewModel> AsCollectionView<TApiElement, TViewModel>(this IActionResult result)
            where TApiElement : Element
            where TViewModel : class, new()
        {
            ObjectResult objResult = (ObjectResult)result;
            object value = objResult.Value;

            if (objResult is CreatedAtActionResult
                || objResult is AcceptedAtActionResult)
            {
                value = (value as ObjectResult).Value;
            }

            if (!(value is CollectionView<TApiElement>))
                throw new InvalidCastException("This IActionResult is NOT a CollectionView generic wrapper for the " + typeof(TApiElement).Name + " element type.");

            var view = value as CollectionView<TApiElement>;
            var viewModels = view.ViewModels as IEnumerable<TViewModel>;
            return viewModels;
        }

        public static AcceptedAtActionResult AsAcceptedAtActionResult(this IActionResult result)
        {
            if (!(result is AcceptedAtActionResult))
                throw new InvalidCastException("This IActionResult is not an AcceptedAtActionResult instance.");

            return result as AcceptedAtActionResult;
        }

        public static CreatedResult AsCreated(this IActionResult result)
        {
            if (!(result is CreatedResult))
                throw new InvalidCastException("This IActionResult is not an CreatedResult instance.");

            return result as CreatedResult;
        }

        public static NoContentResult AsNoContent(this IActionResult result)
        {
            if (!(result is NoContentResult))
                throw new InvalidCastException("This IActionResult is not an NoContentResult instance.");

            return result as NoContentResult;
        }

        /// <summary>
        ///     Assuming the IActionResult is an 'ObjectResult' instance, it's value will be casted into the
        ///     provided T type.
        /// </summary>
        public static T GetElement<T>(this IActionResult result)
            where T : class
        {
            if (!(result is ObjectResult)) 
                throw new InvalidCastException("This IActionResult is not an ObjectResult instance.");

            ObjectResult objectResult = result as ObjectResult;

            if (!(objectResult.Value is T))
                throw new InvalidCastException("This ObjectResult's value is not an instance of the provided T type.");

            T value = objectResult.Value as T;
            return value;
        }

        /// <summary>
        ///     Assuming the IActionResult is an 'ObjectResult' instance, it's value will be casted into a
        ///     Sero's CollectionResult instance, and then it's "ElementsToReturn" property will be casted
        ///     to IEnumerable<T> and returned.
        /// </summary>
        public static IEnumerable<T> GetCollection<T>(this IActionResult result)
        {
            if (!(result is ObjectResult))
                throw new InvalidCastException("This IActionResult is not an ObjectResult instance.");

            ObjectResult objectResult = result as ObjectResult;

            if (!(objectResult.Value is CollectionView))
                throw new InvalidCastException("This ObjectResult's value is not a Sero's CollectionResult instance.");

            CollectionView collectionResult = objectResult.Value as CollectionView;

            if (!(objectResult.Value is IEnumerable<T>))
                throw new InvalidCastException("This ObjectResult's value is not an IEnumerable of the provided T type.");

            IEnumerable<T> collection = collectionResult.ViewModels as IEnumerable<T>;
            return collection;
        }
    }
}
