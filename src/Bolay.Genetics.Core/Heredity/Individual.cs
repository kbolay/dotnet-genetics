namespace Bolay.Genetics.Core.Heredity
{
    /// <summary>
    /// This class represents an individual member of any given species.
    /// </summary>
    public class Individual<TId>
    {
        /// <summary>
        /// Gets or sets the identifier of the individual
        /// </summary>
        public TId Id { get; set; }

        /// <summary>
        /// Gets or sets the paternal identifier.
        /// </summary>
        public TId? PaternalId { get; set; }

        /// <summary>
        /// Gets or sets the maternal identifier.
        /// </summary>
        public TId? MaternalId { get; set; }
    } // end class

    public class Individual : Individual<Guid> 
    { 
        public Individual()
        {
            Id = Guid.NewGuid();
        } // end method
    } // end class
} // end namespace