// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotFakeable.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   The NotFakeable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;

#endregion

namespace M.Radwan.DevMagicFake.Attributes
{
    /// <summary>
    ///  The NotFakeable attribute used to mark the classes so it can be excluded from the generated and faked classes
    /// </summary>
    public class NotFakeable : Attribute
    {
    }
}
