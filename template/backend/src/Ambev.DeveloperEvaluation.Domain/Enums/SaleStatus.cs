namespace Ambev.DeveloperEvaluation.Domain.Enums
{
    /// <summary>
    /// Represents the current status of a sale in the system.
    /// </summary>
    public enum SaleStatus
    {
        /// <summary>
        /// The sale is active and valid.
        /// </summary>
        Active = 1,

        /// <summary>
        /// The sale has been cancelled.
        /// </summary>
        Cancelled = 2
    }
}