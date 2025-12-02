namespace AdventOfCode;

public class AocClient(string sessionToken)
{
    private readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("https://adventofcode.com")
    };

    public async Task<string> GetInputAsync(int year, int day)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
        request.Headers.Add("Cookie", $"session={sessionToken}");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var input = await response.Content.ReadAsStringAsync();
        return input.TrimEnd('\n');
    }

    public async Task<string> SubmitAnswerAsync(int year, int day, int part, string answer)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/{year}/day/{day}/answer");
        request.Headers.Add("Cookie", $"session={sessionToken}");
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["level"] = part.ToString(),
            ["answer"] = answer
        });

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();

        if (html.Contains("That's the right answer"))
            return "✓ Correct!";
        if (html.Contains("That's not the right answer"))
            return "✗ Wrong answer";
        if (html.Contains("You gave an answer too recently"))
            return "⏳ Rate limited - wait before submitting again";
        if (html.Contains("You don't seem to be solving the right level"))
            return "Already solved this part";

        return "Unknown response";
    }
}
