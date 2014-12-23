using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class RegisterEmployee : IDataErrorInfo
    {
        public static bool DoValidation { get; set; }

        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }
        private int _employeeID;
        [Required(ErrorMessage = "Selecteer een medewerker.")]
        [Range(1,100000, ErrorMessage = "Selecteer een medewerker.")]
        public int EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
        private DateTime _from;

        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }
        private DateTime _until;

        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }

        public string Error
        {
            get { return null; }
        }

        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }

        public string this[string columnName]
        {
            get
            {
                if (!DoValidation)
                {
                    return null;
                }
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }
                return String.Empty;
            }
        }

    }
}
