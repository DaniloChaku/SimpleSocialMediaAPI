using Ganss.Xss;

namespace SocialMediaApi.Services;

public class SanitizerService : ISanitizerService
{
    private readonly HtmlSanitizer _sanitizer;

    public SanitizerService()
    {
        _sanitizer = new HtmlSanitizer();
        ConfigureAllowedTags();
    }

    private void ConfigureAllowedTags()
    {
        _sanitizer.AllowedTags.Clear();
        _sanitizer.AllowedTags.Add("b");
        _sanitizer.AllowedTags.Add("i");
        _sanitizer.AllowedTags.Add("u");
    }

    public string Sanitize(string input)
    {
        return _sanitizer.Sanitize(input);
    }
}
