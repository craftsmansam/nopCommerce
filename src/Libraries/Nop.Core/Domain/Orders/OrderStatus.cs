namespace Nop.Core.Domain.Orders;

/// <summary>
/// Represents an order status enumeration
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// Pending
    /// </summary>
    Pending = 10,

    /// <summary>
    /// Processing
    /// </summary>
    Processing = 20,

    /// <summary>
    /// Complete
    /// </summary>
    Complete = 30,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40,

        /// <summary>
        /// On Hold
        /// </summary>
        OnHold = 50, 

        /// <summary>
        /// Ordered
        /// </summary>
        Ordered = 60,
        
        /// <summary>
        /// Work In Progress
        /// Use this for At Subcontractor, Ready for Subcontractor and Work in progress
        /// </summary>
        WorkInProgress = 70


}
