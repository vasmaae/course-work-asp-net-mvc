using System;
using System.Globalization;
using System.Web.Mvc;

namespace MovieStudioWebApplication.Binders
{
    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == null)
            {
                return null;
            }

            var value = valueProviderResult.AttemptedValue;

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            // Replace comma with a period for invariant culture parsing
            value = value.Replace(",", ".");
            
            decimal myValue;
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out myValue))
            {
                return myValue;
            }

            bindingContext.ModelState.AddModelError(
                bindingContext.ModelName, "Неверный формат числа."
            );

            return null;
        }
    }
}
