using TextPrivacy.SensitiveString;
using TextPrivacy.SensitiveString.HotChocolate;
using Xunit;

namespace TextPrivacy.SensitiveString.HotChocolate.Tests;

public class CursorKeySerializerTests
{
    [Fact]
    public void SensitiveStringSerializer_IsSupported_OnlyForSensitiveString()
    {
        var serializer = new SensitiveStringCursorKeySerializer();

        Assert.True(serializer.IsSupported(typeof(SensitiveString)));
        Assert.False(serializer.IsSupported(typeof(SensitiveEmail)));
        Assert.False(serializer.IsSupported(typeof(string)));
    }

    [Fact]
    public void SensitiveEmailSerializer_IsSupported_OnlyForSensitiveEmail()
    {
        var serializer = new SensitiveEmailCursorKeySerializer();

        Assert.True(serializer.IsSupported(typeof(SensitiveEmail)));
        Assert.False(serializer.IsSupported(typeof(SensitiveString)));
    }

    [Fact]
    public void SensitiveStringSerializer_GetCompareToMethod_IsNotNull()
    {
        var serializer = new SensitiveStringCursorKeySerializer();

        Assert.NotNull(serializer.GetCompareToMethod(typeof(SensitiveString)));
    }

    [Fact]
    public void SensitiveStringSerializer_FormatThenParse_RoundTrips()
    {
        var serializer = new SensitiveStringCursorKeySerializer();
        var original = "Ada Lovelace".AsSensitive();

        var buffer = new byte[256];
        var formatted = serializer.TryFormat(original, buffer, out var written);

        Assert.True(formatted);

        var parsed = (SensitiveString) serializer.Parse(buffer.AsSpan(0, written));

        Assert.Equal(original.Reveal(), parsed.Reveal());
    }

    [Fact]
    public void SensitiveEmailSerializer_FormatThenParse_RoundTrips()
    {
        var serializer = new SensitiveEmailCursorKeySerializer();
        var original = "ada@example.com".AsSensitiveEmail();

        var buffer = new byte[256];
        var formatted = serializer.TryFormat(original, buffer, out var written);

        Assert.True(formatted);

        var parsed = (SensitiveString) serializer.Parse(buffer.AsSpan(0, written));

        Assert.Equal(original.Reveal(), parsed.Reveal());
    }
}
