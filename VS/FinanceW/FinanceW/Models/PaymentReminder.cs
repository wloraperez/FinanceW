using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceW.Models
{
    [Table("PaymentReminder")]
    public class PaymentReminder
    {
        public int PaymentReminderId { get; set; }

        [Required(ErrorMessage = "Campo requerido."), Display(Name ="Tipo de recordatorio")]
        public int ReminderTypeId { get; set; }
        [ForeignKey("ReminderTypeId"), Display(Name = "Tipo de recordatorio")]
        public ReminderType ReminderType { get; set; }

        [Display(Name ="Producto")]
        public int? ProductId { get; set; }
        [ForeignKey("ProductId"), Display(Name ="Producto")]
        public Product Product { get; set; }

        [Display(Name ="Gasto recurrente")]
        public int? ExpenseId { get; set; }
        [ForeignKey("ExpenseId"), Display(Name = "Gasto")]
        public Expense Expense { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Date),Display(Name ="Fecha de inicio")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Time), Display(Name = "Hora de inicio")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Date), Display(Name = "Fecha final")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [DataType(DataType.Time), Display(Name = "Hora Final")]
        public TimeSpan EndTime { get; set; }

        [DataType(DataType.Date), Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Estado de recordatorio")]
        public Enum.StatusPaymentReminder StatusPaymentReminder { get; set; }
    }
}
