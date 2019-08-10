using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceW.Models
{
    [Table("ReminderType")]
    public class ReminderType
    {
        public int ReminderTypeId { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        public string Description { get; set; }

        public int RecurrenceHour { get; set; }
        public int RecurrenceDay { get; set; }
        public int RecurrenceMonth { get; set; }
        public int RecurrenceYear { get; set; }

        public List<PaymentReminder> PaymentReminder { get; set; }
    }
}
