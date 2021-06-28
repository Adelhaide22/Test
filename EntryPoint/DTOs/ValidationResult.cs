namespace Test.DTOs
{
    public class ValidationResult<T>
    {
        public T Result { get; set; }
        public bool IsValid { get; set; }
    }
    
    public class ValidationResult
    {
        public bool IsValid { get; set; }
    }
}