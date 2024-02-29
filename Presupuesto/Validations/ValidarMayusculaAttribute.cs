using System.ComponentModel.DataAnnotations;

namespace Presupuesto.Validations
{
    //Validación por atributo

    public class ValidarMayusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firsLetter = value.ToString()[0].ToString();

            if (firsLetter != firsLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }
            return ValidationResult.Success;
        }

    }
}
