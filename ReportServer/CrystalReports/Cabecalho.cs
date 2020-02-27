using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServer.CrystalReports
{
    public class Cabecalho
    {
        private Dictionary<string, string> camposRelatorio;
        public DataTable logo;

        public Cabecalho()
        {
            camposRelatorio = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chave">Campo no arquivo de relatório (.rpt)</param>
        /// <param name="valor">Valor a ser atribuido no campo do relatório</param>
        public void AddCampo(string chave, string valor)
        {
            camposRelatorio.Add(chave, valor);
        }

        public IList<string> GetKeys()
        {
            return camposRelatorio.Keys.ToList();
        }

        public string GetValue(string key)
        {
            return camposRelatorio[key];
        }
    }
}
