using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.ba.cashlessproject.model
{
    public class Organistion_Register
    {
        private int _organisationID;
        [Required]
        [DisplayName("Organisatie ")]
        public int OrganisationID
        {
            get { return _organisationID; }
            set { _organisationID = value; }
        }
        private int _registerID;
        [Required]
        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }
        private DateTime _fromDate;
        [Required(ErrorMessage = "Van is verplicht")]
        [DataType(DataType.DateTime)]
        [DisplayName("Van")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }
        private DateTime _untilDate;
        [Required(ErrorMessage = "Tot is verplicht")]
        [DataType(DataType.Date)]
        [DisplayName("Tot")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime UntilDate
        {
            get { return _untilDate; }
            set { _untilDate = value; }
        }
        
    }
}
