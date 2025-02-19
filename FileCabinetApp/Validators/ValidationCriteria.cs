using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class ValidationCriteria
    {
        public FirstNameValidator FirstName { get; set; }
        public LastNameValidator LastName { get; set; }
        public DateOfBirthValidator DateOfBirth { get; set; }
    }
}
