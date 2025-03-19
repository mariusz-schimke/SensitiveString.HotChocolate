using System.Reflection;
using System.Text;
using GreenDonut.Data.Cursors.Serializers;

namespace TextPrivacy.SensitiveString.HotChocolate;

public abstract class SensitiveStringAbstractCursorKeySerializer : ICursorKeySerializer
{
    private static readonly Encoding _encoding = Encoding.UTF8;

    private static readonly MethodInfo _compareTo = typeof(SensitiveString)
        .GetMethod(nameof(IComparable<SensitiveString>.CompareTo), [typeof(SensitiveString)])!;

    public abstract bool IsSupported(Type type);

    public MethodInfo GetCompareToMethod(Type type) => _compareTo;

    public object Parse(ReadOnlySpan<byte> formattedKey)
    {
        var keyAsString = _encoding.GetString(formattedKey);
        return AsSensitiveString(keyAsString);
    }

    public bool TryFormat(object key, Span<byte> buffer, out int written)
    {
        var sensitiveKey = (SensitiveString) key;
        return _encoding.TryGetBytes(sensitiveKey.Reveal(), buffer, out written);
    }

    protected abstract SensitiveString AsSensitiveString(string formattedKey);
}