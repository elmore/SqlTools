using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SqlTools
{
    class RunFiles : RunFile
    {
        public RunFiles(IOutput console) : base(console) { }

        public override void Run(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("The RunFile method requires a foldername and a connection name");
            }

            _Arguments = args;

            string folder = args[1];

            if (!Directory.Exists(folder))
            {
                throw new ArgumentException(string.Format("Directory '{0}' does not exist", folder));
            }

            string connectionString = GetConnectionString();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                ProcessFolder(conn, args[1]);
                conn.Close();
            }

        }

        protected void ProcessFolder(SqlConnection conn, string folder)
        {
            foreach (string file in Directory.EnumerateFiles(folder))
            {
                ProcessFile(conn, file);
            }
        }
    }
}
