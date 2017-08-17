// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Book.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The book.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using M.Radwan.DevMagicFake.Attributes;

namespace M.Radwan.EntitiesTest
{
    /// <summary>
    /// The book.
    /// </summary>
    [NotFakeable]
    public class Book
    {
       #region Properties

        /// <summary>
        /// Gets or sets Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Publisher.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public string Title { get; set; }

       #endregion
    }
}
