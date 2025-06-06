﻿namespace AccountService.DAL.Entities;

public class Account : Entity
{
    public int UserId { get; set; }
    public decimal Balance { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}