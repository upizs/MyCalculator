

namespace BetterCalculator
{
    public class Operations
    {
        /// <summary>
        /// Left side of the operation
        /// </summary>
        public string LeftSide { get; set; }

        /// <summary>
        /// Right side of the operation
        /// </summary>
        public string RightSide { get; set; }

        /// <summary>
        /// Type of operation to perform
        /// </summary>
        public OperationType OperationType { get; set; }

        public Operations()
        {
            //sets empthy so that there is no null
            LeftSide = string.Empty;
            RightSide = string.Empty;
        }
    }
}
