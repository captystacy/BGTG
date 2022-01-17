using System;
using System.ComponentModel.DataAnnotations;

namespace POSWeb.Models.Attributes
{
    public class MinDateYearAttribute : ValidationAttribute
    {
        private readonly int _minYear;

        public MinDateYearAttribute(int minYear)
        {
            _minYear = minYear;
        }

        public override bool IsValid(object value)
        {
            var date = value is DateTime dateTime
                ? dateTime
                : default;

            return _minYear <= date.Year;
        }
    }
}
