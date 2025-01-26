using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    internal class CustomFavouriteNumeralValidator
    {
        public void ValidateParameters(ParameterObject rec)
        {
            if (rec.FavouriteNumeral > '5' || rec.FavouriteNumeral < '0')
            {
                throw new ArgumentException("Exception because of the incorrect favourite numeral format");
            }
        }
    }
}
