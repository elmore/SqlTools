using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace SqlTools
{
    class Insert2Update : MethodBase
    {
        //private IOutput _Console;

        public Insert2Update(IOutput console) : base(console) { }

        private Regex _InsertRe = new Regex("^INSERT.*VALUES.*$", RegexOptions.Singleline & RegexOptions.IgnoreCase & RegexOptions.Compiled);
        /*
        public Insert2Update(IOutput console)
        {
            _Console = console;
        }
        */
        public override void Run(string[] args)
        {
            if (args.Length == 0)
            {
                _Console.WriteLine("Please supply a filename as the second argument");

                throw new ArgumentException("The Insert2Update method should be run specifying a filename");
            }

            if (args.Length == 1)
            {
                _Console.WriteLine("Please supply the index column as the third argument");

                throw new ArgumentException("The Insert2Update method should be run specifying a field name");
            }

            var converter = new InsertToUpdateSqlConverter(InsertToUpdateSqlConverter.Where(args[1])); // "[TransmissionSettingId]"

            string filename = args[0];

            List<string> insertStatements = GetInsertLines(filename);

            List<string> updateStatements = new List<string>();

            foreach(string insert in insertStatements)
            {
                updateStatements.Add(converter.Convert(insert));
            }

            string newFilename = MakeNewName(filename);

            WriteFile(newFilename, updateStatements);
        }

        private void WriteFile(string newFilename, List<string> updateStatements)
        {
            using (StreamWriter sw = File.CreateText(newFilename))
            {
                foreach (string ln in updateStatements)
                {
                    sw.WriteLine(ln);
                }
            }
        }

        private string MakeNewName(string filename)
        {
            return string.Format("{0}_UPDATE.sql", StripExtension(filename));
        }

        private string StripExtension(string filename)
        {
            return Path.GetFullPath(filename).Replace( Path.GetFileName(filename), Path.GetFileNameWithoutExtension(filename));
        }

        private List<string> GetInsertLines(string p)
        {
            string[] lines = GetLines(p);

            return lines.Where(l => _InsertRe.IsMatch(l)).ToList();
        }

        private string[] GetLines(string p)
        {
            return File.ReadAllLines(p);
        }
    }
}
