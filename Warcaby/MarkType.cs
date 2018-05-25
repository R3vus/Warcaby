using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby
{
    /// <summary>
    /// The type of value a cell in the gamer is currently at 
    /// </summary>
    public enum MarkType
    {
        /// <summary>
        /// The cell doesn't have pawn on it
        /// </summary>
        Free,
        /// <summary>
        /// The cell has white pawn
        /// </summary>
        White,
        /// <summary>
        /// The cell has black pawn
        /// </summary>
        Black
    }
}