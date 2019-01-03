using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 /* The database currently does not support escape characters like new lines, tabs and such. The separator character cannot be escaped as well so make sure to not use the separator character 
 as part of a token. */

namespace Indieteur.SimpleDatabaseFormat
{
    /// <summary>
    /// Provides the ability to retrieve and store information using basic database structure.
    /// </summary>
    public class SimpleDatabase
    {
        public const string DEFAULT_SEPARATOR = ";";

        /// <summary>
        /// List of string token array on the database.
        /// </summary>
        public List<SimpleDatabaseTokenList> SimpleDataList { get; set; } = new List<SimpleDatabaseTokenList>();

        /// <summary>
        /// Creates an empty basic database structure.
        /// </summary>
        public SimpleDatabase()
        {

        }

        /// <summary>
        /// Retrieves or parses a block of string into a basic database structure.
        /// </summary>
        /// <param name="data">The path to the file containing the string to be parsed. Or if dataIsPath is set to false, the string to be parsed itself. </param>
        /// <param name="separator">The character or string separating the tokens on a single line.</param>
        /// <param name="dataIsPath">If set to true, the data argument will be treated as a path instead of the string to be parsed itself.</param>
        public SimpleDatabase(string data, string separator = DEFAULT_SEPARATOR, bool dataIsPath = true)
        {
            if (dataIsPath)
                data = File.ReadAllText(data);
            Init(data);
        }

        void Init(string data, string separator = DEFAULT_SEPARATOR)
        {
            string[] lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries); //Split on new lines
            if (lines != null)
            {
                foreach (string line in lines)
                {
                    SimpleDataList.Add(new SimpleDatabaseTokenList(line, separator)); //Parse line to simple database data.
                }
            }
        }

        /// <summary>
        /// Saves database to a file.
        /// </summary>
        /// <param name="path">Indicates the path to where the database will be saved.</param>
        /// <param name="separator">The character or string that will be used to separate the tokens on a single line.</param>
        public void SaveToFile(string path, string separator = DEFAULT_SEPARATOR)
        {
            File.WriteAllText(path, ToString(separator));
        }

        /// <summary>
        /// Returns a parsable string version of the database. Uses the default separator ";" in between the tokens on a single line.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(DEFAULT_SEPARATOR);
        }

        /// <summary>
        /// Returns a parsable string version of the database.
        /// </summary>
        /// <param name="separator">The character or string that will be used to separate the tokens on a single line.</param>
        /// <returns></returns>
        public string ToString(string separator)
        {
            if (SimpleDataList != null && SimpleDataList.Count > 0)
            {
                StringBuilder sb = new StringBuilder(SimpleDataList[0].ToString(separator)); //Initialize with the first element's parsable string value.
                if (SimpleDataList.Count > 1)
                {
                    for (int i = 1; i < SimpleDataList.Count; ++i)
                    {
                        sb.Append(Environment.NewLine + SimpleDataList[i].ToString(separator));
                    }
                }                   
                return sb.ToString();
            }
            return null;
        }
    }

    public class SimpleDatabaseTokenList
    {
        /// <summary>
        /// A List of string tokens. Can be imagined as the columns on a single row.
        /// </summary>
        public List<string> Tokens { get; set; }

        /// <summary>
        /// Creates an empty list of string tokens.
        /// </summary>
        public SimpleDatabaseTokenList()
        {
            Tokens = new List<string>();
        }

        /// <summary>
        /// Parses a string containing the tokens.
        /// </summary>
        /// <param name="stringToParse">The string to be parsed.</param>
        /// <param name="separator">The character or string separating the tokens.</param>
        public SimpleDatabaseTokenList(string stringToParse, string separator = SimpleDatabase.DEFAULT_SEPARATOR)
        {
            Init(stringToParse.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)); //Split on separator.
        }

        /// <summary>
        /// Creates a list of tokens from a string array.
        /// </summary>
        /// <param name="array">The array where the list of tokens will be created from.</param>
        public SimpleDatabaseTokenList(string[] array)
        {
            Init(array);
        }
        void Init(string[] array)
        {
            Tokens = new List<string>(array);
        }

        /// <summary>
        /// Returms a parsable string version of the tokens with the default separator ";" separating the tokens.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToString(SimpleDatabase.DEFAULT_SEPARATOR);
        }
        /// <summary>
        /// Returms a parsable string version of the tokens.
        /// </summary>
        /// <param name="separator">The separator that will be in between the tokens when they are joined together.</param>
        /// <returns></returns>
        public string ToString(string separator)
        {
            if (Tokens != null)
                return string.Join(separator, Tokens); // Join string together with a separator in between.
            return null;
        }
    }
}
