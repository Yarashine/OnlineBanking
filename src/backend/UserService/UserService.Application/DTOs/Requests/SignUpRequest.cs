﻿namespace UserService.Application.DTOs.Requests;

public class SignUpRequest
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string DeviceId { get; set; }
}
