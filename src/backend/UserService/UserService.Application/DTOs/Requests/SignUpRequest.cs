﻿namespace UserService.Application.DTOs.Requests;

public class SignUpRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; } = "string";
}
