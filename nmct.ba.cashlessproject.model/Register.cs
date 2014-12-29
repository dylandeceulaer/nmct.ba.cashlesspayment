using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Register
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _registerName;
        [DisplayName("Kassa naam")]
        [Required(ErrorMessage = "Kassa naam is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        public string RegisterName
        {
            get { return _registerName; }
            set { _registerName = value; }
        }
        private string _device;
        [DisplayName("Kassa model")]
        [Required(ErrorMessage = "Kassa model is verplicht")]
        [MaxLength(50, ErrorMessage = "Mag maximum 50 karakters bevatten.")]
        [MinLength(2, ErrorMessage = "Moet minimum 2 karakters bevatten")]
        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }
        private DateTime _purchaseDate;
        [Required(ErrorMessage = "Datum aangekocht is verplicht")]
        [DisplayName("Datum aangekocht")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }
        private DateTime _expiresDate;
        [Required(ErrorMessage = "Vervaldatum is verplicht")]
        [DisplayName("Vervaldatum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpiresDate
        {
            get { return _expiresDate; }
            set { _expiresDate = value; }
        }
        
        

    }
}
