﻿
public class Response
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public Response()
    {
        Success = false;
        Message = string.Empty;
    }
}