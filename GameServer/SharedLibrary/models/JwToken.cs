﻿namespace SharedLibrary.models;
[Serializable]
public class JwToken
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}