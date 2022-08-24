﻿namespace Moby.Services.Coupon.API.Models.Dtos;

public class ResponseDto
{
    public bool IsSuccess { get; set; } = true;

    public object Results { get; set; }

    public string Message { get; set; } = "";

    public List<string> Errors { get; set; }
}
