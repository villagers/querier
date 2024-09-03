using Querier.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Options
{
    public interface IOptionValidator
    {
        void MeasureValidator<TValidator>() where TValidator : IMeasurePropertyValidator;
        void DimensionValidator<TValidator>() where TValidator : IDimensionPropertyValidator;
        void TimeDimensionValidator<TValidator>() where TValidator : ITimeDimensionPropertyValidator;

    }
}
