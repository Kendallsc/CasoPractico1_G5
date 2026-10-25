using System.Globalization;
using System.Web.Mvc;

namespace PAP.Mvc.ModelBinders
{
    /// <summary>
    /// Los inputs type="number" siempre envian el decimal con punto. Este binder lo lee en cultura
    /// invariante para que funcione igual aunque el servidor tenga cultura es-CR (coma decimal).
    /// Se registra para decimal y decimal? en Global.asax.
    /// </summary>
    public class InvariantDecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult result = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (result == null)
                return null;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, result);

            string raw = (result.AttemptedValue ?? string.Empty).Trim();
            if (raw.Length == 0)
                return null;

            decimal value;
            if (decimal.TryParse(raw, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out value))
                return value;

            if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.CurrentCulture, out value))
                return value;

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El valor decimal no es valido.");
            return null;
        }
    }
}
