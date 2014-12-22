using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Product : IDataErrorInfo
    {
        public static bool DoValidation { get; set; }

        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _productName;
        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "De naam moet tussen de 2 en 50 karakters bevatten")]
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }
        private float _price;
        [Range(0, 100000, ErrorMessage = "Vul een geldig getal in.")]
        public float Price
        {
            get { return _price; }
            set { _price = value; }
        }
        private int _category;
        [Required(ErrorMessage = "Categorie is verplicht")]
        [Range(1, 10000, ErrorMessage = "Kies een Categorie.")]
        public int Category
        {
            get { return _category; }
            set { _category = value; }
        }
        private byte[] _image;

        public byte[] Image
        {
            get { return _image; }
            set { _image = value; }
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
