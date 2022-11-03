namespace Library.Utils
{
    /// <summary>
    /// Primary key in database
    /// </summary>
    public class PrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Primary key in database that increments id value
    /// </summary>
    public class PrimaryKeyAutoIncrementAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute that flags functions needed to be completed
    /// </summary>
    public class TODO : Attribute
    {
    }
}
