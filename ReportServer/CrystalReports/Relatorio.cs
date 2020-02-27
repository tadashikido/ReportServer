using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ReportServer.CrystalReports;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServer.Model
{
    public class Relatorio : IDisposable
    {
        private Cabecalho cabecalho;
        public ReportDocument Report { get; private set; }

        public Stream ExportStream(ReportExportFormat format = ReportExportFormat.Pdf)
        {
            Stream tmpStrem = null;
            switch (format)
            {
                case ReportExportFormat.Excel:
                    tmpStrem = Report.ExportToStream(ExportFormatType.ExcelWorkbook);
                    break;

                case ReportExportFormat.Word:
                    tmpStrem = Report.ExportToStream(ExportFormatType.WordForWindows);
                    break;

                case ReportExportFormat.Html:
                    tmpStrem = Report.ExportToStream(ExportFormatType.HTML40);
                    break;

                case ReportExportFormat.Pdf:
                    tmpStrem = Report.ExportToStream(ExportFormatType.PortableDocFormat);
                    break;
            }

            return tmpStrem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sReportPath"></param>
        /// <param name="ds"></param>
        /// <param name="cabecalho"></param>
        /// <exception cref="FileNotFoundException" >
        /// <exception cref="ArgumentException" >
        /// <exception cref="FileNotFoundException" >
        public Relatorio(string sReportPath, DataSet ds, Cabecalho cabecalho)
        {
            if (!File.Exists(sReportPath))
                throw new FileNotFoundException("O arquivo " + sReportPath + " não está acessível!");

            if (ds.Tables.Count > 0)
            {
                int somaLinhas = 0;
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    somaLinhas = +ds.Tables[i].Rows.Count;
                }

                if (somaLinhas == 0)
                    throw new ArgumentException("Não foram encontrados registros.");
            }

            this.cabecalho = cabecalho;

            BuildReport(sReportPath, ds);
        }

        private void BuildReport(string sReportPath, DataSet ds)
        {
            Report = new ReportDocument();
            Report.Load(sReportPath);
            Report.SetDataSource(ds);

            ConfiguraCabecalho();
        }

        private void ConfiguraCabecalho()
        {
            foreach (string campo in cabecalho.GetKeys())
            {
                (Report.ReportDefinition.ReportObjects[campo] as TextObject).Text = cabecalho.GetValue(campo);
            }
        }

        #region IDisposable Support
        private bool disposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Report?.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
