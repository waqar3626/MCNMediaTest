namespace AwesomeDal
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;

    /// <summary>
    /// Provides a profile of the main parameters required to connect to a database.
    /// </summary>
    public class DatabaseProfile
    {
        /// <summary>
        /// The defaultport used to connect ot the database.
        /// </summary>
        private const int DefaultPort = 3306;

        /// <summary>
        /// The defaultport used to connect ot the database.
        /// </summary>
        //private const string SessionId = "CND6MkQbUGGL7ZyGrt4i";

        /// <summary>
        /// The database queried and written to by <see cref="AwesomeDal" />.
        /// </summary>
        public readonly string Database;

        /// <summary>
        /// The <see cref="Database" /> <see cref="Password" /> for <see cref="User" />.
        /// </summary>
        public readonly string Password;

        /// <summary>
        /// The <see cref="Database" />'s <see cref="Port" /> of access.
        /// </summary>
        public readonly int Port;

        /// <summary>
        /// The server containing the <see cref="Database" />.
        /// </summary>
        public readonly string Server;

        /// <summary>
        /// The <see cref="User" /> that <see cref="AwesomeDal" /> uses for the <see cref="Database" />.
        /// </summary>
        public readonly string User;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProfile" /> class.
        /// </summary>
        /// <param name="server">
        /// See <see cref="Server" />.
        /// </param>
        /// <param name="database">
        /// See <see cref="Database" />.
        /// </param>
        /// <param name="user">
        /// See <see cref="User" />.
        /// </param>
        /// <param name="password">
        /// See <see cref="Password" />.
        /// </param>
        /// <param name="port">
        /// See <see cref="Port" />.
        /// </param>
        /// <param name="timeOut">
        /// See <see cref="TimeOut" />.
        /// </param>
        /// <param name="langIdParameter">
        /// See <see cref="LangIdParameter" />.
        /// </param>
        public DatabaseProfile(string server, string database, string user, string password, int timeOut,
            int port = DefaultPort, bool langIdParameter = false)
        {
            Server = server;
            Database = database;
            User = user;
            Password = password;
            Port = port;
            TimeOut = timeOut;
            LangIdParameter = langIdParameter;

            ValidateValues();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseProfile" /> class.
        /// In this instance, the <see cref="DatabaseProfile"/> properties are taken
        /// from the Web.Config.
        /// </summary>
        /// <example>
        /// <code language="xml" source="..\Examples\DatabaseProfile.xml" title="Web.Config" />
        /// </example>
        public DatabaseProfile()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();

            Server = root.GetConnectionString("AwesomeDal_Server");
            Database = root.GetConnectionString("AwesomeDal_Database");
            User = root.GetConnectionString("AwesomeDal_User");
            Password = root.GetConnectionString("AwesomeDal_Password");
            TimeOut = int.Parse(root.GetConnectionString("AwesomeDal_TimeOut"));

            var portString = root.GetConnectionString("AwesomeDal_Port");
            try
            {
                Port = portString != null
                    ? int.Parse(portString)
                    : DefaultPort;
            }
            catch (Exception)
            {
                throw new ArgumentException(string.Format(
                    "'AwesomeDal_Port' value '{0}' cannot be converted to an integer.", portString));
            }

            var langIdString = root.GetConnectionString("AwesomeDal_LangIdParameter");
            try
            {
                LangIdParameter = !string.IsNullOrEmpty(langIdString) && bool.Parse(langIdString);
            }
            catch (Exception)
            {
                throw new ArgumentException(
                    string.Format("'AwesomeDal_LangIdParameter' value '{0}' cannot be converted to a boolean.",
                        langIdString));
            }

            ValidateValues();
        }

        /// <summary>
        /// Automatically adds the <c>'lang_Id'</c> from the session as a parameter.
        /// </summary>
        public bool LangIdParameter { get; private set; }

        /// <summary>
        /// The time-out duration in seconds.
        /// </summary>
        public int TimeOut { get; private set; }

       

        /// <summary>
        /// Gets the session <see cref="DatabaseProfile" /> set by <see cref="SetSession" />.
        /// </summary>
        /// <returns>
        /// The session <see cref="DatabaseProfile" /> set by <see cref="SetSession" />.
        /// </returns>
        /// <example>
        /// Getting a <see cref="DatabaseProfile"/> From <see cref="HttpSessionState"/>
        /// <code language="cs" source="..\Examples\Example.cs" region="GetDatabaseProfileFromSession" />
        /// <code language="vb" source="..\Examples\Example.cs" region="GetDatabaseProfileFromSession" />
        /// </example>
        //public static DatabaseProfile GetSession()
        //{
        //    var sessionItem = HttpContext.Current.Session[SessionId];

        //    if (sessionItem == null)
        //    {
        //        throw new Exception("Session DatabaseProfile has not been set.");
        //    }

        //    return (DatabaseProfile)sessionItem;
        //}

        /// <summary>
        /// Sets a session <see cref="DatabaseProfile" /> that can be retrieved by <see cref="GetSession" />.
        /// </summary>
        /// <param name="databaseProfile">
        /// the <see cref="DatabaseProfile" /> to set into session.
        /// </param>
        /// <example>
        /// Setting a <see cref="DatabaseProfile"/> To <see cref="HttpSessionState"/>
        /// <code language="cs" source="..\Examples\Example.cs" region="SetDatabaseProfileToSession" />
        /// <code language="vb" source="..\Examples\Example.cs" region="SetDatabaseProfileToSession" />
        /// </example>
        //public static void SetSession(DatabaseProfile databaseProfile)
        //{
        //    HttpContext.Session.Add(SessionId, databaseProfile);
        //}

        /// <summary>
        /// Sets the timeout duration of any database queries.
        /// </summary>
        /// <param name="seconds">
        /// The number of seconds for the time-out.
        /// </param>
        public void SetTimeOut(int seconds)
        {
            TimeOut = seconds;
            ValidateValues();
        }

        /// <summary>
        /// The connection-string created using <see cref="Server" />, <see cref="Database" />, <see cref="Password" />
        /// and <see cref="User" /> that will be used by the <see cref="DatabaseConnect" />
        /// instance that consumes this <see cref="DatabaseProfile" />.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> with the <see cref="Database" /> connection-string value.
        /// </returns>
        public string ConnectionString()
        {
            string conStr =  string.Format("server={0};port={1};user={2};password={3};database={4};", Server, Port,User,Password, Database);
            return conStr;
        }

        private void ValidateValues()
        {
            if (string.IsNullOrEmpty(Server))
            {
                throw new ArgumentException(string.Format("Server cannot be empty."));
            }

            if (string.IsNullOrEmpty(Database))
            {
                throw new ArgumentException(string.Format("Database cannot be empty."));
            }

            if (string.IsNullOrEmpty(User))
            {
                throw new ArgumentException(string.Format("User cannot be empty."));
            }

            if (Port <= 0)
            {
                throw new ArgumentException(string.Format("Port cannot be {0}.", Port));
            }

            if (TimeOut <= 0)
            {
                throw new ArgumentException(string.Format("TimeOut cannot be {0}.", TimeOut));
            }
        }
    }
}