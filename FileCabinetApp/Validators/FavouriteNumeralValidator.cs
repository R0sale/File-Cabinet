using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FavouriteNumeralValidator : IRecordValidator
    {
        private char minNumeral;

        private char maxNumeral;

        public FavouriteNumeralValidator(char minNumeral, char maxNumeral)
        {
            this.minNumeral = minNumeral;
            this.maxNumeral = maxNumeral;
        }

        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.FavouriteNumeral > this.maxNumeral || rec.FavouriteNumeral < this.maxNumeral/minNumeral)
            {
                throw new ArgumentException("Exception because of the incorrect favourite numeral format");
            }
        }
    }
}
