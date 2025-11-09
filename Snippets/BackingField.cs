private static class BackingField
{
    private static void SetPropertyValue(PropertyInfo propertyInfo, object target, object value)
    {
        var prop = propertyInfo;
        if (prop != null && prop.CanWrite) // todo check this
            prop.SetValue(target, value, null);
    }

    private static void SetPropertyValue(PropertyInfo propertyInfo, object target, object value)
    {
        var prop = propertyInfo;
        if (!prop.CanWrite)
        {
            var field = GetBackingField(propertyInfo);
            if (field is not null)
            {
                field.SetValue(target, value);
                return;
            }
            prop = propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name);
        }

        if (prop != null && prop.CanWrite) // todo check this
            prop.SetValue(target, value, null);
    }

    private static FieldInfo? GetBackingField(PropertyInfo property)
    {
        if (property == null)
            throw new ArgumentNullException(nameof(property));

        if (!property.CanRead
            || property.GetMethod is null
            || !property.GetMethod.IsDefined(typeof(CompilerGeneratedAttribute)))
            return null;

        var backingFieldName = $"<{property.Name}>k__BackingField";
        var backingField = property.DeclaringType?.GetField(
            backingFieldName,
            BindingFlags.Instance | BindingFlags.NonPublic);

        return backingField;
    }
}