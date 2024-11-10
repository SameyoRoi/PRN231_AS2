namespace PE_SE173338_PE.DTO
{
    public class ErrorResponseDTO
    {
        public ErrorDetail Error { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
