namespace GenericFileTransferClient.Behavior
{
    /// <summary>
    /// A simple interface which must be supported by the ViewMode classes using the 
    /// ValidationExceptionBehavior.
    /// </summary>
    public interface IValidationExceptionHandler
    {
        void ValidationExceptionsChanged(int count);
    }
}