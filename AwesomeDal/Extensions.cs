namespace AwesomeDal
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// A collection of extension methods used in <see cref="AwesomeDal" />, and available for use in the
    /// containing solution.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Replaces <see cref="DBNull" /> values in <paramref name="dataRow" /> with:
        /// <ul>
        /// <li>
        /// <see cref="string.Empty" /> for <see cref="string" />
        /// </li>
        /// <li>
        /// '\0' for <see cref="char" />
        /// </li>
        /// <li>
        /// <c>false</c> for <see cref="bool" />
        /// </li>
        /// <li>
        /// <c>0</c> for <see cref="byte" />, <see cref="int" />, <see cref="double" /> and <see cref="decimal" />
        /// </li>
        /// </ul>
        /// </summary>
        /// <param name="dataRow">
        /// The <see cref="DataRow" /> from which to remove <see cref="DBNull" /> values.
        /// </param>
        /// <param name="trim">
        /// An option to <see cref="string.Trim(char[])" /> all <see cref="string" />s in <paramref name="dataRow" />
        /// while cleaning. This is <c>false</c> by default.
        /// </param>
        /// <returns>
        /// The cleaned <see cref="DataRow" />, <paramref name="dataRow" />.
        /// </returns>
        public static DataRow DeNull(this DataRow dataRow, bool trim = false)
        {
            if (dataRow == null)
            {
                return null;
            }

            var dt = DeNull(dataRow.Table.Clone(), trim);
            return dt.Rows[0];
        }

        /// <summary>
        /// Replaces <see cref="DBNull" /> values in <paramref name="dataSet" /> with:
        /// <ul>
        /// <li>
        /// <see cref="string.Empty" /> for <see cref="string" />
        /// </li>
        /// <li>
        /// '\0' for <see cref="char" />
        /// </li>
        /// <li>
        /// <c>false</c> for <see cref="bool" />
        /// </li>
        /// <li>
        /// <c>0</c> for <see cref="byte" />, <see cref="int" />, <see cref="double" /> and <see cref="decimal" />
        /// </li>
        /// <li>
        /// A new <see cref="XmlDocument" /> for <see cref="XmlDocument" />.
        /// </li>
        /// </ul>
        /// </summary>
        /// <param name="dataSet">
        /// The <see cref="DataSet" /> from which to remove <see cref="DBNull" /> values.
        /// </param>
        /// <param name="trim">
        /// An option to <see cref="string.Trim(char[])" /> all <see cref="string" />s in <paramref name="dataSet" />
        /// while cleaning. This is <c>false</c> by default.
        /// </param>
        /// <returns>
        /// The cleaned <see cref="DataSet" />, <paramref name="dataSet" />.
        /// </returns>
        public static DataSet DeNull(this DataSet dataSet, bool trim = false)
        {
            if (dataSet == null)
            {
                return null;
            }

            var newDataSet = new DataSet();

            foreach (DataTable dataTable in dataSet.Tables)
            {
                var newDataTable = DeNull(dataTable, trim);
                newDataSet.Tables.Add(newDataTable.Copy());
            }

            return newDataSet;
        }

        /// <summary>
        /// Replaces <see cref="DBNull" /> values in <paramref name="dataTable" /> with:
        /// <ul>
        /// <li>
        /// <see cref="string.Empty" /> for <see cref="string" />
        /// </li>
        /// <li>
        /// '\0' for <see cref="char" />
        /// </li>
        /// <li>
        /// <c>false</c> for <see cref="bool" />
        /// </li>
        /// <li>
        /// <c>0</c> for <see cref="byte" />, <see cref="int" />, <see cref="double" /> and <see cref="decimal" />
        /// </li>
        /// </ul>
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> from which to remove <see cref="DBNull" /> values.
        /// </param>
        /// <param name="trim">
        /// An option to <see cref="string.Trim(char[])" /> all <see cref="string" />s in <paramref name="dataTable" />
        /// while cleaning. This is <c>false</c> by default.
        /// </param>
        /// <returns>
        /// The cleaned <see cref="DataTable" />, <paramref name="dataTable" />.
        /// </returns>
        public static DataTable DeNull(this DataTable dataTable, bool trim = false)
        {
            if (dataTable == null)
            {
                return null;
            }

            var columnIndex = dataTable.Columns.Count - 1;

            foreach (DataRow dr in dataTable.Rows)
            {
                for (var i = 0; i <= columnIndex; i++)
                {
                    dynamic columnType = dataTable.Columns[i].DataType.Name;

                    if (!ReferenceEquals(dr[i], DBNull.Value))
                    {
                        continue;
                    }

                    if (columnType == "Boolean")
                    {
                        dr[i] = false;
                        continue;
                    }

                    if (columnType == "Byte")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "Char")
                    {
                        dr[i] = '\0';
                        continue;
                    }

                    if (columnType == "Decimal")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "Double")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "Int16")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "Int32")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "Int64")
                    {
                        dr[i] = 0;
                        continue;
                    }

                    if (columnType == "String")
                    {
                        dr[i] = string.Empty;
                        continue;
                    }

                    if (columnType == "Xml")
                    {
                        dr[i] = new XmlDocument();
                    }
                }
            }

            if (trim)
            {
                dataTable = Trim(dataTable);
            }

            return dataTable;
        }

        /// <summary>
        /// Replaces <see cref="DBNull" /> value <see cref="DateTime" /> with a native null-able <see cref="Nullable{DateTime}" />.
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime" /> to be cleaned.
        /// </param>
        /// <returns>
        /// <paramref name="dateTime" /> as nullable <see cref="Nullable{DateTime}" />.
        /// </returns>
        public static DateTime? DeNull(this object dateTime)
        {
            return ReferenceEquals(dateTime, DBNull.Value)
                ? new DateTime?()
                : (DateTime?) dateTime;
        }

        /// <summary>
        /// Replaces <see cref="DBNull" /> value <see cref="DateTime" /> with a native null-able <see cref="Nullable{DateTime}" />.
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime" /> to be cleaned.
        /// </param>
        /// <param name="filterYear">
        /// A <see cref="DateTime.Year" /> to return as a new <see cref="Nullable{DateTime}" />.
        /// For example, dates defaulted to <c>1900</c>.
        /// </param>
        /// <returns>
        /// <paramref name="dateTime" /> as nullable <see cref="Nullable{DateTime}" />.
        /// </returns>
        public static DateTime? DeNull(this object dateTime, int filterYear)
        {
            if (ReferenceEquals(dateTime, DBNull.Value))
            {
                return new DateTime?();
            }

            var dt = (DateTime?) dateTime;

            return dt.Value.Year == filterYear
                ? new DateTime?()
                : dt;
        }

        /// <summary>
        /// Determines whether <paramref name="dataSet" />'s first <see cref="DataTable" /> has <see cref="DataRow" />s.
        /// </summary>
        /// <param name="dataSet">
        /// The <see cref="DataSet" /> the method is called on.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> result of whether <paramref name="dataSet" />'s first <see cref="DataTable" /> has
        /// <see cref="DataRow" />s.
        /// </returns>
        public static bool HasDataRows(this DataSet dataSet)
        {
            if (!dataSet.HasDataTables()) return false;
            return dataSet.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Determines whether <paramref name="dataTable" /> has <see cref="DataRow" />s.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> the method is called on.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> result of whether <paramref name="dataTable" /> has <see cref="DataRow" />s.
        /// </returns>
        public static bool HasDataRows(this DataTable dataTable)
        {
            return dataTable.Rows.Count > 0;
        }

        /// <summary>
        /// Determines whether <paramref name="dataSet" /> has <see cref="DataTable" />s.
        /// </summary>
        /// <param name="dataSet">
        /// The <see cref="DataTable" /> the method is called on.
        /// </param>
        /// <returns>
        /// The <see cref="bool" /> result of whether the <see cref="DataSet" /> has <see cref="DataTable" />s.
        /// </returns>
        public static bool HasDataTables(this DataSet dataSet)
        {
            return dataSet.Tables.Count > 0;
        }

        /// <summary>
        /// <see cref="string.Trim(char[])" />s all <see cref="string" />s in <paramref name="dataTable" />.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be trimmed.
        /// </param>
        /// <returns>
        /// The trimmed <paramref name="dataTable" />.
        /// </returns>
        public static DataTable Trim(this DataTable dataTable)
        {
            if (dataTable == null)
            {
                return null;
            }

            var columnIndex = dataTable.Columns.Count - 1;

            foreach (DataRow dr in dataTable.Rows)
            {
                for (var i = 0; i <= columnIndex; i++)
                {
                    dynamic columnType = dataTable.Columns[i].DataType.Name;

                    if (ReferenceEquals(dr[i], DBNull.Value))
                    {
                        continue;
                    }

                    if (columnType == "String")
                    {
                        dr[i] = dr[i].ToString().Trim();
                        continue;
                    }

                    if (columnType == "Char")
                    {
                        dr[i] = char.Parse(dr[i].ToString().Trim());
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Returns a <see cref="DataTable" /> with only unique rows based on
        /// the filter of <see cref="DataColumn" />s provided in <paramref name="columnNames" />.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> the unique values will be extracted from.
        /// </param>
        /// <param name="columnNames">
        /// A <see cref="List{T}" /> of type <see cref="string" /> representing the
        /// <see cref="DataColumn" />s <paramref name="dataTable" /> should be filtered to.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" /> of unique values after being filtered by the <see cref="DataColumn" />s
        /// specified in <paramref name="columnNames" />.
        /// </returns>
        public static DataTable UniqueFilter(this DataTable dataTable, IEnumerable<string> columnNames)
        {
            var dv = dataTable.DefaultView;
            return dv.ToTable(true, columnNames.ToArray());
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, byte value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, bool value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, char value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, DateTime value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, decimal value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, double value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, float value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, int value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }

        /// <summary>
        /// Filters <paramref name="dataTable" /> by the <see cref="DataColumn" />
        /// name <paramref name="columnName" /> for the <paramref name="value" /> provided.
        /// </summary>
        /// <param name="dataTable">
        /// The <see cref="DataTable" /> to be filtered.
        /// </param>
        /// <param name="value">
        /// The value to filter by.
        /// </param>
        /// <param name="columnName">
        /// The column name.
        /// </param>
        /// <returns>
        /// The <see cref="DataTable" />.
        /// </returns>
        public static DataTable ValueFilter(this DataTable dataTable, string value, string columnName)
        {
            var filteredDt = dataTable.Select(columnName + " = " + value).CopyToDataTable();
            return filteredDt;
        }
    }
}