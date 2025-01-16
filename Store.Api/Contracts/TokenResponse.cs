namespace Store.Api.Contracts;
public record TokenResponse
{
    public string Token { get; init; } = string.Empty;
    public Exception Error { get; init; } = default!;
    public static TokenResponse Failed(Exception error)
    {
        return new TokenResponse { Error = error };
    }
    public static TokenResponse Success(string accessToken)
    {
        return new TokenResponse
        {
            Token = accessToken
        };
    }
}