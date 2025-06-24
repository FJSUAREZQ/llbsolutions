using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.DTOs;

public class InvoicePdfDocument : IDocument
{
    private readonly VentaDTO _venta;

    public InvoicePdfDocument(VentaDTO venta) => _venta = venta;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);

            // Header con factura y datos del cliente y venta
            page.Header()
                .Column(column =>
                {
                    column.Item().Text($"Factura #{_venta.Id}").FontSize(20).Bold();
                    column.Item().Text($"Cliente: {_venta.ClientName}");
                    column.Item().Text($"Método de pago: {_venta.PaymentMethod}");
                    column.Item().Text($"Puntos ganados: {_venta.PointsEarned}, puntos usados: {_venta.PointsUsed}");
                    column.Item().Text($"Subtotal: {_venta.Subtotal:C}, Descuento: {(_venta.Subtotal - _venta.Total):C}");
                    column.Item().PaddingTop(10); // espacio antes de la tabla
                });

            // Tabla de productos
            page.Content().Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.RelativeColumn(3); // producto
                    cols.RelativeColumn(1); // cantidad
                    cols.RelativeColumn(1); // total
                });

                table.Header(header =>
                {
                    header.Cell().Text("Producto").Bold();
                    header.Cell().Text("Cant.").AlignRight().Bold();
                    header.Cell().Text("Total").AlignRight().Bold();
                });

                foreach (var d in _venta.Details)
                {
                    table.Cell().Text(d.ProductName);
                    table.Cell().Text(d.Quantity.ToString()).AlignRight();
                    table.Cell().Text($"{d.Quantity * d.UnitPrice:C}").AlignRight();
                }

                table.Cell().Text("Subtotal").Bold();
                table.Cell().ColumnSpan(2).AlignRight().Text($"{_venta.Subtotal:C}").Bold();

                table.Cell().Text("Descuento").Bold();
                table.Cell().ColumnSpan(2).AlignRight().Text($"{(_venta.Subtotal - _venta.Total):C}").Bold();

                table.Cell().Text("Total").Bold();
                table.Cell().ColumnSpan(2).AlignRight().Text($"{_venta.Total:C}").Bold();
            });

            // Footer con fecha
            page.Footer()
                .AlignCenter()
                .Text($"Emitido: {_venta.Date:yyyy-MM-dd}");
        });
    }
}
