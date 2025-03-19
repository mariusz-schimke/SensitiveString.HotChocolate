namespace TextPrivacy.SensitiveString.HotChocolate;

public sealed class SensitiveEmailCursorKeySerializer : SensitiveStringAbstractCursorKeySerializer
{
    public override bool IsSupported(Type type) => type == typeof(SensitiveEmail);
    protected override SensitiveString AsSensitiveString(string formattedKey) => formattedKey.AsSensitiveEmail();
}