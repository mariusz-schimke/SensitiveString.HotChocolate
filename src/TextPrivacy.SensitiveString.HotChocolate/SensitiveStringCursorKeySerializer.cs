namespace TextPrivacy.SensitiveString.HotChocolate;

public sealed class SensitiveStringCursorKeySerializer : SensitiveStringAbstractCursorKeySerializer
{
    public override bool IsSupported(Type type) => type == typeof(SensitiveString);
    protected override SensitiveString AsSensitiveString(string formattedKey) => formattedKey.AsSensitive();
}