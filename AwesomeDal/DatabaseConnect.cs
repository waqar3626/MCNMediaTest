namespace AwesomeDal
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Collections.Generic;
    using System.Data;
    //    using System.Data.Linq;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Xml;

    /// <summary>
    /// Allows querying of MySql Server databases via queries and <see cref="System.Data.CommandType.StoredProcedure" />s. All
    /// methods that return values
    /// undergo the de-null process in <see cref="Extensions" />. See ( <see cref="Extensions.DeNull(DataSet,bool)" />).
    /// </summary>
    /// <example>
    /// Returning A <see cref="DataTable" />
    /// <code language="cs" source="..\Examples\Example.cs" region="ReturnDataTable" />
    /// <code language="vb" source="..\Examples\Example.vb" region="ReturnDataTable" />
    /// </example>
    public class DatabaseConnect
    {
        /// <summary>
        /// 
        /// </summary>
        private bool _executeNonQuery;



        /// <summary>
        /// The <see cref="AwesomeDal.DatabaseProfile" /> the <see cref="DatabaseConnect" />
        /// instance uses to connect to a database.
        /// </summary>
        public readonly DatabaseProfile DatabaseProfile;

        /// <summary>
        /// The <see cref="SqlCommand" /> the <see cref="DatabaseConnect" /> instance uses internally.
        /// </summary>
        private MySqlCommand _mySqlCommand;

        /// <summary>
        /// The <see cref="SqlConnection" /> the <see cref="DatabaseConnect" /> instance uses internally.
        /// </summary>
        private MySqlConnection _mySqlConnection;

        /// <summary>
        /// Determines the <see cref="CommandType" />. At initialization, this value is
        /// defaulted to <see cref="CommandType" /> <c>StoredProcedure</c>.
        /// </summary>
        public CommandType CommandType;



        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnect" /> class thatconnects to the
        /// database detailed in the web.config, or session if <paramref name="fromSession" /> is true.
        /// </summary>
        /// <param name="fromSession">
        /// If set to <c>true</c> defines <see cref="DatabaseProfile" /> from <see cref="AwesomeDal.DatabaseProfile.GetSession" />.
        /// <br />
        /// If <c>false</c> defines <see cref="DatabaseProfile" /> from the web.Config.
        /// See <see cref="AwesomeDal.DatabaseProfile()" /> constructor.
        /// </param>
        public DatabaseConnect(bool fromSession = false)
        {
            DatabaseProfile = new DatabaseProfile();

            _mySqlCommand = new MySqlCommand
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            _mySqlConnection = new MySqlConnection
            {
                ConnectionString = DatabaseProfile.ConnectionString()
            };



            //if (DatabaseProfile.LangIdParameter)
            //{
            //    AddLangIdParameter();
            //}
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnect" /> class that connects
        /// to the database defined in <paramref name="databaseProfile" />.
        /// </summary>
        /// <param name="databaseProfile">
        /// The <see cref="AwesomeDal.DatabaseProfile" /> supplied to be used in this instance
        /// of <see cref="DatabaseConnect" />.
        /// </param>
        public DatabaseConnect(DatabaseProfile databaseProfile)
        {
            DatabaseProfile = databaseProfile;

            _mySqlCommand = new MySqlCommand
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };

            _mySqlConnection = new MySqlConnection
            {
                ConnectionString = DatabaseProfile.ConnectionString()
            };



            //if (DatabaseProfile.LangIdParameter)
            //{
            //    AddLangIdParameter();
            //}
        }

        /// <summary>
        /// A <see cref="bool" /> declaring if <see cref="AddLangIdParameter" /> has been called. The <c>Lang_Id</c>
        /// parameter can only be set once, any duplicate calls will be ignored.
        /// </summary>
        public bool HasLangIdParameter { get; private set; }

        /// <summary>
        /// A <see cref="bool" /> declaring if malicious tags were found. This value gets updated every time
        /// <see cref="AddParameter(string, string)" /> or <see cref="AddParameter(string, XmlDocument)" /> are called.
        /// </summary>
        public bool MaliciousTagsFound { get; private set; }

        /// <summary>
        /// Returns a pre-configured <see cref="DataContext" />.
        /// </summary>
        /// <param name="fromSession">
        /// If set to <c>true</c> defines <see cref="DatabaseProfile" /> from <see cref="AwesomeDal.DatabaseProfile.GetSession" />.
        /// <br /> If <c>false</c>
        /// defines <see cref="DatabaseProfile" /> from the web.Config. See <see cref="AwesomeDal.DatabaseProfile()" />
        /// constructor.
        /// </param>
        /// <returns>
        /// The configured <see cref="DataContext" />.
        /// </returns>
        //public static DataContext GetDataContext(bool fromSession = false)
        //{
        //    var databaseProfile = fromSession
        //        ? DatabaseProfile.GetSession()
        //        : new DatabaseProfile();

        //    return new DataContext(databaseProfile.ConnectionString());
        //}

        /// <summary>
        /// Returns a pre-configured <see cref="DataContext" />.
        /// </summary>
        /// <param name="databaseProfile">
        /// The <see cref="AwesomeDal.DatabaseProfile" />.
        /// </param>
        /// <returns>
        /// The configured <see cref="DataContext" />.
        /// </returns>
        //public static DataContext GetDataContext(DatabaseProfile databaseProfile)
        //{
        //    return new DataContext(databaseProfile.ConnectionString());
        //}

        ///// <summary>
        ///// Adds an <see cref="SqlDbType.Int" /> type <see cref="MySqlParameter" /> from the value of session item 'i18n' with the
        ///// name '@Lang_Id'. The
        ///// Lang_Id parameter can only be set once, any duplicate calls will be ignored.
        ///// </summary>
        //public void AddLangIdParameter()
        //{
        //    if (HasLangIdParameter)
        //    {
        //        return;
        //    }


        //}

        /// <summary>
        /// Adds the <paramref name="MySqlParameter" />.
        /// </summary>
        public void AddParameter(MySqlParameter mySqlParameter)
        {
            _mySqlCommand.Parameters.Add(mySqlParameter);
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.Bit" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, bool value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Bit).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.Int" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, byte value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Int32).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.NVarChar" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, char value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.VarChar, -1).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.DateTime" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, DateTime value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.DateTime).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.DateTime" /> type <see cref="MySqlParameter" />. This value will be
        /// <see cref="DBNull.Value" /> if <paramref name="value" /> is not assigned.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, DateTime? value)
        {
            if (value == null)
            {
                _mySqlCommand.Parameters.Add(name, MySqlDbType.DateTime).Value = DBNull.Value;
            }
            else
            {
                _mySqlCommand.Parameters.Add(name, MySqlDbType.DateTime).Value = (DateTime)value;
            }
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.Decimal" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, decimal value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Decimal, -1).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.Float" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, double value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Float, -1).Value = value;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.Float" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, float value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Float).Value = value;
        }

        /// <summary>
        /// Adds an <see cref="SqlDbType.Int" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, int value)
        {
            _mySqlCommand.Parameters.Add(name, MySqlDbType.Int32).Value = value;
        }

        /// <summary>
        /// Converts <paramref name="value" /> to an XML list and adds the value
        /// as a <see cref="SqlDbType.Xml" /> <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        /// <example>
        /// <code language="sql" source="..\Examples\..\Examples\IntArray.sql" />
        /// </example>
        public void AddParameter(string name, params int[] value)
        {
            var xmlValue = value.Serialize();
            _mySqlCommand.Parameters.Add(name, MySqlDbType.JSON).Value = xmlValue.OuterXml;
        }

        /// <summary>
        /// Converts <paramref name="value" /> to an XML list and adds the value
        /// as a <see cref="SqlDbType.Xml" /> <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        /// <example>
        /// <code language="sql" source="..\Examples\IntArray.sql" />
        /// </example>
        public void AddParameter(string name, List<int> value)
        {
            var xmlValue = value.Serialize();
            _mySqlCommand.Parameters.Add(name, MySqlDbType.JSON).Value = xmlValue.OuterXml;
        }

        /// <summary>
        /// Adds a <see cref="SqlDbType.NVarChar" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
            else
            {
                ContainsMaliciousTags(name, value);
            }

            _mySqlCommand.Parameters.Add(name, MySqlDbType.VarChar, -1).Value = value;
        }

        /// <summary>
        /// Adds an <see cref="SqlDbType.Xml" /> type <see cref="MySqlParameter" />.
        /// </summary>
        /// <param name="name">
        /// The name to be given to the <see cref="MySqlParameter"/>.
        /// </param>
        /// <param name="value">
        /// The parameter value.
        /// </param>
        public void AddParameter(string name, XmlDocument value)
        {
            ContainsMaliciousTags(name, value.ToString());

            _mySqlCommand.Parameters.Add(name, MySqlDbType.JSON).Value = value.OuterXml;
        }

        /// <summary>
        /// Clears the <see cref="DatabaseConnect" />'s parameters.
        /// This automatically happens after <see cref="Execute" />
        /// or a 'Return' method is called.
        /// </summary>
        public void ClearParameters()
        {
            _mySqlCommand.Parameters.Clear();
            MaliciousTagsFound = false;
        }

        /// <summary>
        /// Closes and disposes the <see cref="SqlConnection" />.
        /// </summary>
        public void CloseAndDispose()
        {
            var Dbstate = _mySqlConnection.State.ToString();
            if (Dbstate == "Open") {
                ClearParameters();
                _mySqlConnection.Close();
                _mySqlConnection.Dispose();
            }
           
        }

        /// <summary>
        /// Executes the specified <see cref="System.Data.CommandType.StoredProcedure" />, and returns the number of rows affected by the <see cref="System.Data.CommandType.StoredProcedure" />. 
        ///  </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="int" />. Returns the number of rows affected.
        /// </returns>
        public int Execute(string storedProcedureName)
        {
            _executeNonQuery = true;
            var dataCell = ReturnDataCell(storedProcedureName);
            return (int)dataCell;
        }

        /// <summary>
        /// Refreshes the instance, allowing the <see cref="DatabaseConnect" /> to be reconfigured.
        /// </summary>
        public void Refresh()
        {
            MaliciousTagsFound = false;
            CloseAndDispose();

            _mySqlCommand = new MySqlCommand();
            _mySqlConnection = new MySqlConnection
            {
                ConnectionString = DatabaseProfile.ConnectionString()
            };
            _executeNonQuery = false;
        }

        /// <summary>
        /// Returns a <see cref="bool" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="bool" />.
        /// </returns>
        public bool ReturnBool(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return false;
            if (!dataSet.HasDataRows()) return false;

            var dataCell = dataSet.Tables[0].Rows[0][0];
            return (bool)dataCell;
        }

        /// <summary>
        /// Returns a <see cref="byte" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="byte" />.
        /// </returns>
        public int ReturnByte(string storedProcedureName)
        {
            var dataCell = ReturnDataCell(storedProcedureName);
            return (byte)dataCell;
        }

        /// <summary>
        /// Returns a <see cref="char" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="char" />.
        /// </returns>
        public char ReturnChar(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return '\0';
            if (!dataSet.HasDataRows()) return '\0';

            var dataCell = dataSet.Tables[0].Rows[0][0];
            return (char)dataCell;
        }

        /// <summary>
        /// Returns a <see cref="DataRow" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataRow" /> of the first <see cref="DataTable" />.
        /// </returns>
        public DataRow ReturnDataRow(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null)
            {
                var dt = new DataTable();
                return dt.NewRow();
            }
            if (!dataSet.HasDataRows())
            {
                var dt = dataSet.Tables[0];
                return dt.NewRow();
            }

            //var translationService = new TranslationService();

            //if (translationService.LocalisationActive)
            //{

            //    var translatedDataTable = translationService.TranslateDataTable(dataSet.Tables[0]);
            //    return translatedDataTable.Rows[0];

            //}

            return dataSet.Tables[0].Rows[0];

        }

        /// <summary>
        /// Returns a <see cref="DataSet" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet" />.
        /// </returns>
        public DataSet ReturnDataSet(string storedProcedureName)
        {

            var dataSet = Connect(storedProcedureName);

            if (dataSet == null) return new DataSet();
            if (!dataSet.HasDataTables()) return new DataSet();

            //var translatedDataSet = new DataSet();
            //var translationService = new TranslationService();
            //if (translationService.LocalisationActive)
            //{

            //    for (int i = 0; i < dataSet.Tables.Count; i++)
            //    {

            //        var translatedDateTable = translationService.TranslateDataTable(dataSet.Tables[i].Copy());
            //        translatedDateTable.TableName = "Table " + i;
            //        translatedDataSet.Tables.Add(translatedDateTable);
            //    }
            //    if (!translatedDataSet.HasDataTables()) return new DataSet();
            //    return translatedDataSet;

            //}
            return dataSet;
        }

        /// <summary>
        /// Returns a <see cref="DataTable" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <param name="translateData">
        /// Whether the data has to be translated.
        /// </param> 
        /// <returns>
        /// The first <see cref="DataTable" /> of the <see cref="DataSet" />.
        /// </returns>
        public DataTable ReturnDataTable(string storedProcedureName, bool translateData = true)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return new DataTable();
            if (!dataSet.HasDataTables()) return new DataTable();

            var dataTable = dataSet.Tables[0];

            //var translationService = new TranslationService();
            //if (translationService.LocalisationActive)
            //{
            //    if (translateData)
            //    {
            //        return translationService.TranslateDataTable(dataTable);
            //    }
            //}
            return dataTable;

        }

        /// <summary>
        /// Returns a <see cref="DateTime" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="DateTime" />.
        /// </returns>
        public DateTime ReturnDateTime(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return new DateTime();
            if (!dataSet.HasDataRows()) return new DateTime();

            var dataCell = dataSet.Tables[0].Rows[0][0];
            return (DateTime)dataCell;
        }

        /// <summary>
        /// Returns a <see cref="decimal" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="decimal" />.
        /// </returns>
        public decimal ReturnDecimal(string storedProcedureName)
        {
            var dataCell = ReturnDataCell(storedProcedureName);
            return (decimal)dataCell;
        }

        /// <summary>
        /// Returns a <see cref="float" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="float" />.
        /// </returns>
        public float ReturnFloat(string storedProcedureName)
        {
            var dataCell = ReturnDataCell(storedProcedureName);
            return (float)dataCell;
        }

        /// <summary>
        /// Returns an <see cref="int" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="int" />.
        /// </returns>
        public int ReturnInt(string storedProcedureName)
        {
            var dataCell = ReturnDataCell(storedProcedureName);
            return Convert.ToInt32(dataCell);
        }

        /// <summary>
        /// Returns the <see cref="Array" /> of type <see cref="int" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The <see cref="Array" /> of type <see cref="int" />.
        /// </returns>
        public IEnumerable<int> ReturnIntArray(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return null;
            if (!dataSet.HasDataRows()) return null;

            var dataTable = dataSet.Tables[0];

            return from DataRow dataRow
                in dataTable.Rows
                   select (int)dataRow[0];
        }

        /// <summary>
        /// Returns a <see cref="string" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="string" />.
        /// </returns>
        public string ReturnString(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return string.Empty;
            if (!dataSet.HasDataRows()) return string.Empty;

            var dataCell = dataSet.Tables[0].Rows[0][0];
            return (string)dataCell;
        }

        /// <summary>
        /// Returns the an <see cref="Array" /> of type <see cref="string" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The <see cref="Array" /> of type <see cref="string" />.
        /// </returns>
        public IEnumerable<string> ReturnStringArray(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return null;
            if (!dataSet.HasDataRows()) return null;

            var dataTable = dataSet.Tables[0];

            return from DataRow dataRow
                in dataTable.Rows
                   select (string)dataRow[0];
        }

        /// <summary>
        /// Returns an <see cref="XmlDocument" />.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// The first <see cref="DataColumn" /> of the first <see cref="DataRow" /> of the first <see cref="DataTable" /> as
        /// directly cast <see cref="XmlDocument" />.
        /// </returns>
        public XmlDocument ReturnXml(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return null;
            if (!dataSet.HasDataRows()) return null;

            var dataCell = dataSet.Tables[0].Rows[0][0];
            var xDoc = new XmlDocument();
            xDoc.LoadXml(dataCell.ToString());
            return xDoc;
        }

        /// <summary>
        /// Sets the time out for the <see cref="System.Data.CommandType.StoredProcedure" /> execution.
        /// </summary>
        /// <param name="seconds">
        /// The value of the time-out in seconds.
        /// </param>
        public void SetTimeOut(int seconds)
        {
            DatabaseProfile.SetTimeOut(seconds);
        }

        /// <summary>
        /// Executes the specified <see cref="System.Data.CommandType.StoredProcedure" /> for <see cref="Execute" /> and all
        /// <see cref="DatabaseConnect" /> 'Return' methods. Only method this method truly contacts the database.
        /// </summary>
        /// <param name="storedProcedureName">
        /// Name of the <see cref="System.Data.CommandType.StoredProcedure" />.
        /// </param>
        /// <returns>
        /// A <see cref="DataSet" /> representing the selection of the <see cref="System.Data.CommandType.StoredProcedure" />. This
        /// can be <see cref="Nullable" /> if no selection from the database is made.
        /// </returns>
        internal DataSet Connect(string storedProcedureName)
        {
            
                if (MaliciousTagsFound)
                    return null;
              
                _mySqlConnection.Open();

                _mySqlCommand.CommandText = storedProcedureName;
                _mySqlCommand.Connection = _mySqlConnection;
                _mySqlCommand.CommandTimeout = DatabaseProfile.TimeOut;

                _mySqlCommand.CommandType = CommandType == CommandType.Query
                    ? System.Data.CommandType.Text
                    : System.Data.CommandType.StoredProcedure;

                var ds = new DataSet { EnforceConstraints = true };

                var da = new MySqlDataAdapter(_mySqlCommand);

                //if (_executeNonQuery)
                //{
                //    var dt = new DataTable("RowsAffected");

                //    dt.Columns.Add(new DataColumn("count", typeof(int)));
                //    DataRow dr = dt.NewRow();
                //    da.SelectCommand.StatementCompleted += delegate (object sender, StatementCompletedEventArgs e)
                //    {
                //        dr[0] = e.RecordCount;
                //    };
                //    dt.Rows.Add(dr);
                //    ds.Tables.Add(dt);
                //    da.Fill(ds);
                //    Refresh();

                //    return ds;
                //}

                da.Fill(ds);

                Refresh();

                ds = ds.DeNull(true);
                return ds;
            
        }

        /// <summary>
        /// Determines whether <paramref name="value" /> has any malicious tags, and sets <see cref="MaliciousTagsFound" /> to
        /// <c>true</c> is so.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        private void ContainsMaliciousTags(string name, string value)
        {
            if (Security.ContainsMaliciousTags(value))
            {
                CloseAndDispose();
                MaliciousTagsFound = true;

                var messsage = string.Format("Potentially malicious data contained in parameter '{0}.", name);
                throw new ArgumentException(messsage, value);
            }
        }

        private object ReturnDataCell(string storedProcedureName)
        {
            var dataSet = Connect(storedProcedureName);
            if (dataSet == null) return 0;
            if (!dataSet.HasDataRows()) return 0;

            var dataCell = dataSet.Tables[0].Rows[0][0];
            return dataCell;
        }
    }
}