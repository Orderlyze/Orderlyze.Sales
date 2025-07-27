namespace SharedModels.Dtos.Contacts
{
    /// <summary>
    /// Represents the status of a call or contact in the sales workflow.
    /// </summary>
    public enum CallStatus
    {
        /// <summary>
        /// New contact that has not been called yet.
        /// </summary>
        New = 0,
        
        /// <summary>
        /// Call has been scheduled for a future date.
        /// </summary>
        Scheduled = 1,
        
        /// <summary>
        /// Contact was successfully reached during the call.
        /// </summary>
        Reached = 2,
        
        /// <summary>
        /// Contact could not be reached during the call attempt.
        /// </summary>
        NotReached = 3,
        
        /// <summary>
        /// Call workflow has been completed for this contact.
        /// </summary>
        Completed = 4,
        
        /// <summary>
        /// Call has been postponed to a later date.
        /// </summary>
        Postponed = 5
    }
}