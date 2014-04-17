using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class RunFile : MethodBase
    {
        protected string[] _Arguments;

        public RunFile(IOutput console) : base(console)  { }

        public override void Run(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("The RunFile method requires a filename and a connection name");
            }

            _Arguments = args;

            string connectionString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                ProcessFile(conn, args[1]);
                conn.Close();
            }
        }

        protected void ProcessFile(SqlConnection conn, string filepath)
        {
            string sql = GetSql(filepath);

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Execute(cmd, filepath);
            }
        }

        protected void Execute(SqlCommand command, string filepath)
        {
            using (var da = new SqlDataAdapter(command))
            using(var ds = new DataSet())
            {
                try
                {
                    da.Fill(ds);

                    HandleData(ds, filepath);
                }
                catch (SqlException e)
                {
                    HandleError(e, filepath);
                }
            }
        }

        protected void HandleError(SqlException e, string filepath)
        {
            string resultpath = MakeResultPath(filepath, false);

            _Console.Write(e.ToString());

            File.WriteAllText(resultpath, e.ToString());
        }

        protected string MakeResultPath(string filepath, bool success)
        {
            string dir = Path.GetDirectoryName(filepath);

            string newDir = string.Format("{0}/results/{1}", dir, success ? "succeeded" : "failed");

            if (!Directory.Exists(newDir))
            {
                Directory.CreateDirectory(newDir);
            }

            string filename = Path.GetFileNameWithoutExtension(filepath);

            return String.Format("{0}/{1}.xml", newDir, filename);
        }

        private void HandleData(DataSet ds, string filepath)
        {
            ds.WriteXml(MakeResultPath(filepath, true));
        }

        protected string GetSql(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new ArgumentException(string.Format("The file '{0}' does not exist", filename));
            }

            return File.ReadAllText(filename);
        }

        protected string GetConnectionString()
        {
            string connectionName = _Arguments[0];

            if (ConfigurationManager.ConnectionStrings[connectionName] == null)
            {
                throw new ArgumentException(String.Format("The connection name '{0}' does not exist in the configuration file", connectionName));
            }

            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }

    }
}
