using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model 
{
    public class Customer : ObservableObject, IDataErrorInfo 
    {
        public static bool DoValidation { get; set; }

        private int _id;
        
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _customerName;
        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De naam moet tussen de 3 en 50 karakters bevatten")]
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }
        
        private float _balance;
        [Range(0,100000, ErrorMessage = "Vul een geldig getal in.")]
        public float Balance
        {
            get { return _balance; }
            set { _balance = value; RaisePropertyChanged("Balance"); }
        }

        private byte[] _picture;

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }
        
        private string _firstName;
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "De voornaam moet tussen de 2 en 50 karakters bevatten")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        
        private string _street;
        [Required(ErrorMessage = "Straat is verplicht")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "De straat moet tussen de 3 en 50 karakters bevatten")]
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }
        
        private string _number;
        [Required(ErrorMessage = "Nummer is verplicht")]
        [RegularExpression(@"^[0-9]{1,6}[A-Za-z]{0,2}$", ErrorMessage = "Nummer mag enkel cijfers en evt. letters bevatten.")]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        
        private string _postalCode;
        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Postcode mag enkel cijfers bevatten.")]
        public string PostalCode
        {
            get { return _postalCode; }
            set { _postalCode = value; }
        }
        
        private string _city;
        [Required(ErrorMessage = "Plaats is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Plaats moet tussen de 2 en 50 karakters bevatten")]
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        private string _card;

        public string Card
        {
            get { return _card; }
            set { _card = value; }
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
