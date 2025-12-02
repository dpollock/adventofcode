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

    public async Task<string> GetProblemAsync(int year, int day)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}");
        request.Headers.Add("Cookie", $"session={sessionToken}");

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();

        // Extract article content and convert to readable text
        var start = html.IndexOf("<article");
        var end = html.LastIndexOf("</article>") + "</article>".Length;
        if (start < 0 || end <= start) return html;

        var articles = html[start..end];

        // Basic HTML to text conversion
        return ConvertHtmlToText(articles);
    }

    private static string ConvertHtmlToText(string html)
    {
        // Remove scripts and styles
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<style[^>]*>[\s\S]*?</style>", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Convert common elements
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<h2[^>]*>(.*?)</h2>", "\n## $1\n", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<p>", "\n");
        html = System.Text.RegularExpressions.Regex.Replace(html, @"</p>", "\n");
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<br\s*/?>", "\n");
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<li>", "\n- ");
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<code>(.*?)</code>", "`$1`", System.Text.RegularExpressions.RegexOptions.Singleline);
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<pre><code>(.*?)</code></pre>", "\n```\n$1\n```\n", System.Text.RegularExpressions.RegexOptions.Singleline);
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<em[^>]*>(.*?)</em>", "*$1*", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove remaining tags
        html = System.Text.RegularExpressions.Regex.Replace(html, @"<[^>]+>", "");

        // Decode HTML entities
        html = System.Net.WebUtility.HtmlDecode(html);

        // Clean up whitespace
        html = System.Text.RegularExpressions.Regex.Replace(html, @"\n{3,}", "\n\n");
        return html.Trim();
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
