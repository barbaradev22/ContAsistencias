using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ContAsistencias.Pages.Reportes
{
    public class VistaReporteModel : PageModel
    {
        private static readonly TimeSpan HoraAtraso = TimeSpan.Parse("09:30:00");
        private static readonly TimeSpan HoraSalidaAnticipada = TimeSpan.Parse("17:30:00");

        private readonly dhelperAsistencias _helperAsistencias;
        private readonly dhelperUsuario _helperUsuario;

        public VistaReporteModel(dhelperAsistencias helperAsistencias, dhelperUsuario helperUsuario)
        {
            _helperAsistencias = helperAsistencias;
            _helperUsuario = helperUsuario;
        }

        public List<Asistencia> ListaAtrasos { get; set; } = new();
        public List<Asistencia> ListaSalidasAntictipadas { get; set; } = new();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            await CargarReporteAsync(DateTime.Now.AddDays(-7), DateTime.Now);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            if (DateTime.TryParse(Request.Form["txtFechaInicio"], out DateTime fechaInicio) &&
                DateTime.TryParse(Request.Form["txtFechaFin"], out DateTime fechaFin))
            {
                await CargarReporteAsync(fechaInicio, fechaFin);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostPdfAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            if (!DateTime.TryParse(Request.Form["txtFechaInicio"], out DateTime fechaInicio) ||
                !DateTime.TryParse(Request.Form["txtFechaFin"], out DateTime fechaFin))
            {
                return Page();
            }

            await CargarReporteAsync(fechaInicio, fechaFin);

            var pdf = GenerarPdf();
            var nombreArchivo = $"Reporte_Asistencia_{FechaInicio:yyyyMMdd}_{FechaFin:yyyyMMdd}.pdf";
            return File(pdf, "application/pdf", nombreArchivo);
        }

        private async Task CargarReporteAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;

            var todasLasAsistencias = await _helperAsistencias.ObtenerTodasLasAsistencias();
            var asistenciasRango = todasLasAsistencias
                .Where(a => a.Fecha.Date >= FechaInicio.Date && a.Fecha.Date <= FechaFin.Date)
                .ToList();

            ListaAtrasos = asistenciasRango
                .Where(a => string.Equals(a.Tipo, "entrada", StringComparison.OrdinalIgnoreCase) && a.Hora > HoraAtraso)
                .ToList();

            ListaSalidasAntictipadas = asistenciasRango
                .Where(a => string.Equals(a.Tipo, "salida", StringComparison.OrdinalIgnoreCase) && a.Hora < HoraSalidaAnticipada)
                .ToList();
        }

        private byte[] GenerarPdf()
        {
            using var stream = new MemoryStream();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(header =>
                    {
                        header.Item().Text("Reportes de Asistencia").FontSize(18).SemiBold();
                        header.Item().PaddingTop(4).Text($"Período: {FechaInicio:dd/MM/yyyy} - {FechaFin:dd/MM/yyyy}").FontSize(11);
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(14);
                        RenderSection(column, "Registros con Atrasos", "Hora Entrada", ListaAtrasos, "Atraso");
                        RenderSection(column, "Registros con Salidas Anticipadas", "Hora Salida", ListaSalidasAntictipadas, "Salida Anticipada");
                    });
                });
            }).GeneratePdf(stream);

            return stream.ToArray();
        }

        private static void RenderSection(ColumnDescriptor column, string titulo, string horaEncabezado, IReadOnlyCollection<Asistencia> registros, string estado)
        {
            column.Item().Text(titulo).FontSize(13).SemiBold();
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(70);
                    columns.ConstantColumn(80);
                    columns.ConstantColumn(85);
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("ID Asistencia").SemiBold();
                    header.Cell().Element(CellStyle).Text("ID Usuario").SemiBold();
                    header.Cell().Element(CellStyle).Text("Fecha").SemiBold();
                    header.Cell().Element(CellStyle).Text(horaEncabezado).SemiBold();
                    header.Cell().Element(CellStyle).Text("Estado").SemiBold();
                });

                if (registros.Count == 0)
                {
                    table.Cell().ColumnSpan(5).Element(CellStyle).PaddingVertical(8).Text($"No hay registros de {estado.ToLowerInvariant()} en el período seleccionado").Italic();
                    return;
                }

                foreach (var registro in registros)
                {
                    table.Cell().Element(CellStyle).Text(registro.IdAsistencia.ToString());
                    table.Cell().Element(CellStyle).Text(registro.IdUsuario.ToString());
                    table.Cell().Element(CellStyle).Text(registro.Fecha.ToString("dd/MM/yyyy"));
                    table.Cell().Element(CellStyle).Text(registro.Hora.ToString(@"hh\:mm\:ss"));
                    table.Cell().Element(CellStyle).Text(estado);
                }
            });
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(4).PaddingRight(2);
        }
    }
}
