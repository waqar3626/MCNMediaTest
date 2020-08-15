namespace AwesomeDal
{
    /// <summary>
    /// Determines what command type to execute in <see cref="DatabaseConnect"/>.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        /// Calls an existing stored procedure in the database.
        /// This is the default value set at <see cref="DatabaseConnect"/>'s initialisation.
        /// </summary>
        StoredProcedure,

        /// <summary>
        /// A query is passed as a string instead of a stored procedure name.
        /// </summary>
        Query
    }
}
