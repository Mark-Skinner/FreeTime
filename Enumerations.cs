namespace FreeTime
{
    public class Enumerations
    {
        public enum PrintTreeMode
        {
            /// <summary>
            /// Print parent, then children.
            /// </summary>
            BinaryPrefix,
            /// <summary>
            /// Print left child, parent, then right child.
            /// </summary>
            BinaryInfix,
            /// <summary>
            /// Print children, then parent.
            /// </summary>
            BinaryPostfix,
            /// <summary>
            /// Print tree is zigzag fashion, starting from left to right.
            /// </summary>
            ZigZagStartLeft,
            /// <summary>
            /// Print tree is zigzag fashion, starting from right to left.
            /// </summary>
            ZigZagStartRight,
            /// <summary>
            /// Print tree left to right on each level.
            /// </summary>
            LeftToRight,
            /// <summary>
            /// Print tree right to left on each level.
            /// </summary>
            RightToLeft,
            /// <summary>
            /// Prints the entire tree as it is in the structure.
            /// </summary>
            Entire
        }
    }
}
