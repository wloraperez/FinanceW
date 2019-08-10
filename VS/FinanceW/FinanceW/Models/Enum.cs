using System.ComponentModel.DataAnnotations;

namespace FinanceW.Models
{
    public class Enum
    {
        public enum StatusProduct
        {
            Activo,
            Inactivo,
            Pagado
        }

        public enum StatusUser
        {
            Activo,
            Inactivo,
            Bloqueado
        }

        public enum StatusExpense
        {
            Activo,
            Inactivo
        }

        public enum StatusBank
        {
            Activo,
            Inactivo
        }

        public enum StatusPayment
        {
            Activo,
            Inactivo
        }

        public enum StatusPaymentReminder
        {
            Activo,
            Inactivo
        }

        public enum StatusCurrency
        {
            Activa,
            Inactiva
        }

        public enum StatusCashFlow
        {
            Activo,
            Anulado
        }

        public enum ProductType
        {
            [Display(Name = "Cuenta de Ahorro")]
            CuentaAhorro = 0,
            [Display(Name = "Cuenta Corriente")]
            CuentaCorriente = 1,
            [Display(Name = "Certificado Financiero")]
            CertificadoFinanciero = 2,
            [Display(Name = "San")]
            San = 3,
            [Display(Name = "Tarjeta de Crédito Visa")]
            TarjetaCreditoVis = 10,
            [Display(Name = "Tarjeta de Crédito MasterCard")]
            TarjetaCreditoMC = 11,
            [Display(Name = "Tarjeta de Crédito American Express")]
            TarjetaCreditoAE = 12,
            [Display(Name = "Préstamo Personal")]
            PrestamoPersonal = 20,
            [Display(Name = "Préstamo Hipotecario")]
            PrestamoHipotecario = 21,
            [Display(Name = "Préstamo de Vehículo")]
            PrestamoVehiculo = 22,
            [Display(Name = "Efectivo")]
            Efectivo = 30
        }

        public enum Country
        {
            [Display(Name = "República Dominicana")]
            RepublicaDominicana
        }

        public enum ExpenseType
        {
            [Display(Name = "Renta vivienda")]
            RentaVivienda = 1,
            [Display(Name = "Mantenimiento vivienda")]
            MantenimientoVivienda = 10,
            [Display(Name = "Energía eléctrica")]
            Energia = 20,
            [Display(Name = "Telefonía fija")]
            TelefonoFijo = 30,
            [Display(Name = "Internet Fijo")]
            InternetFijo = 31,
            [Display(Name = "TV por Cable")]
            TVCable = 32,
            [Display(Name = "Telefonía - Internet - TV")]
            TelefonoIntTV = 33,
            [Display(Name = "Teléfono móvil")]
            TelefonoMovil = 40,
            [Display(Name = "Internet móvil")]
            InternetMovil = 41,
            [Display(Name = "Streaming audio")]
            StreamingAudio = 50,
            [Display(Name = "Streaming video")]
            StreamingVideo = 51,
            [Display(Name = "Servicio en línea")]
            ServiciosOnline = 52
        }
    }
}
