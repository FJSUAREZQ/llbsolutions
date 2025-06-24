using BlazorApp.MVVM.Models;
using QuestPDF.Fluent;
using Shared.DTOs;

namespace BlazorApp.MVVM.Helpers
{
    public interface IPdfInvoiceService
    {
        public byte[] Generate(VentaDTO venta)
        {
            var doc = new InvoicePdfDocument(venta);
            return doc.GeneratePdf();
        }
    }
}
