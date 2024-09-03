using Microsoft.Extensions.DependencyInjection;
using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public class ValidationOption
    {
        private readonly IServiceCollection _services;

        public ValidationOption(IServiceCollection services)
        {
            _services = services;
        }

        public void MeasureValidator<TValidator>() where TValidator : IMeasurePropertyValidator
        {
            _services.AddScoped(typeof(IMeasurePropertyValidator), typeof(TValidator));
        }
        public void DimensionValidator<TValidator>() where TValidator : IDimensionPropertyValidator
        {
            _services.AddScoped(typeof(IDimensionPropertyValidator), typeof(TValidator));
        }
        public void TimeDimensionValidator<TValidator>() where TValidator : ITimeDimensionPropertyValidator
        {
            _services.AddScoped(typeof(ITimeDimensionPropertyValidator), typeof(TValidator));
        }
    }
}
